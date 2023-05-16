using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormulaUtils
{
    /// <summary>Struct for return values of formula that affect damage and to-hit chance.</summary>
    public struct ToHitAndDamageMods
    {
        public int damageMod;
        public int toHitMod;
    }

    /// <summary>
    /// Calculate the damage caused by an attack.
    /// </summary>
    /// <param name="attacker">Attacking entity</param>
    /// <param name="target">Target entity</param>
    /// <param name="isEnemyFacingAwayFromPlayer">Whether enemy is facing away from player, used for backstabbing</param> // 背对
    /// <param name="weaponAnimTime">Time the weapon animation lasted before the attack in ms, used for bow drawing </param>
    /// <param name="weapon">The weapon item being used</param>
    /// <returns>Damage inflicted to target, can be 0 for a miss or ineffective hit</returns>
    // 对目标造成的伤害，可以为0为未命中或无效命中
    public static int CalculateAttackDamage(Actor attacker, Actor target, bool isEnemyFacingAwayFromPlayer, int weaponAnimTime, Item weapon)
    {
        if (attacker == null || target == null)
            return 0;

        int damageModifiers = 0;
        int damage = 0;
        int chanceToHitMod = 0;
        int backstabChance = 0;
        var player = Main.Inst.hero;
        short skillID = 0;

        // Choose whether weapon-wielding enemies use their weapons or weaponless attacks.
        // In classic, weapon-wielding enemies use the damage values of their weapons
        // instead of their weaponless values.
        // For some enemies this gives lower damage than similar-tier monsters
        // and the weaponless values seems more appropriate, so here
        // enemies will choose to use their weaponless attack if it is more damaging.
        // 如果空手的伤害比持武器伤害大，默认空手
        var AIAttacker = attacker as Monster;
        if (AIAttacker != null && weapon != null)
        {
            int weaponAverage = (weapon.GetBaseDamageMin() + weapon.GetBaseDamageMax()) / 2;
            int noWeaponAverage = (AIAttacker.MinDamage + AIAttacker.MaxDamage) / 2;

            if (noWeaponAverage > weaponAverage)
            {
                // Use hand-to-hand
                weapon = null;
            }
        }

        if (weapon != null)
        {
            // 如果目标的材料大于武器的材料，无效攻击
            // If the attacker is using a weapon, check if the material is high enough to damage the target
            if (target.MinMetalToHit > (WeaponMaterialTypes)weapon.NativeMaterialValue)
            {
                return 0;
            }
            // Get weapon skill used
            skillID = weapon.GetWeaponSkillIDAsShort();
        }
        else
        {
            skillID = (short)Skills.HandToHand;
        }

        chanceToHitMod = attacker.tSkills.GetLiveSkillValue(skillID);

        // 玩家
        if (attacker == player)
        {
            // Apply swing modifiers
            // ToHitAndDamageMods swingMods = CalculateSwingModifiers(GameManager.Instance.WeaponManager.ScreenWeapon);
            // damageModifiers += swingMods.damageMod;
            // chanceToHitMod += swingMods.toHitMod;

            // 熟练度
            // Apply proficiency modifiers
            ToHitAndDamageMods proficiencyMods = CalculateProficiencyModifiers(attacker, weapon);
            damageModifiers += proficiencyMods.damageMod;
            chanceToHitMod += proficiencyMods.toHitMod;

            // 种族克制
            // Apply racial bonuses
            ToHitAndDamageMods racialMods = CalculateRacialModifiers(attacker, weapon, player);
            damageModifiers += racialMods.damageMod;
            chanceToHitMod += racialMods.toHitMod;

            // 背刺，命中增加
            backstabChance = CalculateBackstabChance(player, null, isEnemyFacingAwayFromPlayer);
            chanceToHitMod += backstabChance;
        }

        // Choose struck body part
        int struckBodyPart = CalculateStruckBodyPart();

        // Get damage for weaponless attacks
        // 空手攻击  
        if (skillID == (short)Skills.HandToHand)
        {
            // 如果是玩家或种族敌人
            if (attacker == player || (AIAttacker != null && AIAttacker.EntityType == EntityTypes.EnemyClass))
            {
                if (CalculateSuccessfulHit(attacker, target, chanceToHitMod, struckBodyPart))
                {
                    damage = CalculateHandToHandAttackDamage(attacker, target, damageModifiers, attacker == player);
                    // 背刺成功，3倍伤害
                    damage = CalculateBackstabDamage(damage, backstabChance);
                }
            }
            else if (AIAttacker != null) // attacker is a monster
            {
                // Handle multiple attacks by AI
                int minBaseDamage = 0;
                int maxBaseDamage = 0;
                int attackNumber = 0;
                while (attackNumber < 3) // Classic supports up to 5 attacks but no monster has more than 3
                {
                    if (attackNumber == 0)
                    {
                        minBaseDamage = AIAttacker.MinDamage;
                        maxBaseDamage = AIAttacker.MaxDamage;
                    }
                    else if (attackNumber == 1)
                    {
                        minBaseDamage = AIAttacker.MinDamage2;
                        maxBaseDamage = AIAttacker.MaxDamage2;
                    }
                    else if (attackNumber == 2)
                    {
                        minBaseDamage = AIAttacker.MinDamage3;
                        maxBaseDamage = AIAttacker.MaxDamage3;
                    }

                    int reflexesChance = 50 - (10 * ((int)player.Reflexes - 2));

                    int hitDamage = 0;
                    if (DFRandom.rand() % 100 < reflexesChance && minBaseDamage > 0 && CalculateSuccessfulHit(attacker, target, chanceToHitMod, struckBodyPart))
                    {
                        hitDamage = UnityEngine.Random.Range(minBaseDamage, maxBaseDamage + 1);
                        // Apply special monster attack effects
                        if (hitDamage > 0)
                            OnMonsterHit(AIAttacker, target, hitDamage);

                        damage += hitDamage;
                    }

                    // Apply bonus damage only when monster has actually hit, or they will accumulate bonus damage even for missed attacks and zero-damage attacks
                    if (hitDamage > 0)
                        damage += GetBonusOrPenaltyByEnemyType(attacker, target);

                    ++attackNumber;
                }
            }
        }
        // Handle weapon attacks
        else if (weapon != null)
        {
            // Apply weapon material modifier.
            chanceToHitMod += CalculateWeaponToHit(weapon);

            // Mod hook for adjusting final hit chance mod and adding new elements to calculation. (no-op in DFU)
            // chanceToHitMod = AdjustWeaponHitChanceMod(attacker, target, chanceToHitMod, weaponAnimTime, weapon);

            if (CalculateSuccessfulHit(attacker, target, chanceToHitMod, struckBodyPart))
            {
                damage = CalculateWeaponAttackDamage(attacker, target, damageModifiers, weaponAnimTime, weapon);

                damage = CalculateBackstabDamage(damage, backstabChance);
            }

            // Handle poisoned weapons
            if (damage > 0 && weapon.poisonType != Poisons.None)
            {
                InflictPoison(attacker, target, weapon.poisonType, false);
                weapon.poisonType = Poisons.None;
            }
        }

        damage = Mathf.Max(0, damage);

        DamageEquipment(attacker, target, damage, weapon, struckBodyPart);

        // Apply Ring of Namira effect
        // if (target == player)
        // {
        //     DaggerfallUnityItem[] equippedItems = target.ItemEquipTable.EquipTable;
        //     DaggerfallUnityItem item = null;
        //     if (equippedItems.Length != 0)
        //     {
        //         if (IsRingOfNamira(equippedItems[(int)EquipSlots.Ring0]) || IsRingOfNamira(equippedItems[(int)EquipSlots.Ring1]))
        //         {
        //             IEntityEffect effectTemplate = GameManager.Instance.EntityEffectBroker.GetEffectTemplate(RingOfNamiraEffect.EffectKey);
        //             effectTemplate.EnchantmentPayloadCallback(EnchantmentPayloadFlags.None,
        //                 targetEntity: AIAttacker.EntityBehaviour,
        //                 sourceItem: item,
        //                 sourceDamage: damage);
        //         }
        //     }
        // }
        //Debug.LogFormat("Damage {0} applied, animTime={1}  ({2})", damage, weaponAnimTime, GameManager.Instance.WeaponManager.ScreenWeapon.WeaponState);

        Debug.Log("CalculateAttackDamage > " + damage);
        return damage;
    }

    public static float specialInfectionChance = 0.6f;

    /// <summary>
    /// Execute special monster attack effects.
    /// </summary>
    /// <param name="attacker">Attacking entity</param>
    /// <param name="target">Target entity</param>
    /// <param name="damage">Damage done by the hit</param>
    public static void OnMonsterHit(Monster attacker, Actor target, int damage)
    {
        Diseases[] diseaseListA = { Diseases.Plague };
        Diseases[] diseaseListB = { Diseases.Plague, Diseases.StomachRot, Diseases.BrainFever };
        Diseases[] diseaseListC = {
            Diseases.Plague, Diseases.YellowFever, Diseases.StomachRot, Diseases.Consumption,
            Diseases.BrainFever, Diseases.SwampRot, Diseases.Cholera, Diseases.Leprosy, Diseases.RedDeath,
            Diseases.TyphoidFever, Diseases.Dementia
        };
        float random;
        switch (attacker.CareerIndex)
        {
            case (int)MonsterCareers.Rat:
                // In classic rat can only give plague (diseaseListA), but DF Chronicles says plague, stomach rot and brain fever (diseaseListB).
                // Don't know which was intended. Using B since it has more variety.
                if (Dice100.SuccessRoll(5))
                    InflictDisease(attacker, target, diseaseListB);
                break;
            case (int)MonsterCareers.GiantBat:
                // Classic uses 2% chance, but DF Chronicles says 5% chance. Not sure which was intended.
                if (Dice100.SuccessRoll(2))
                    InflictDisease(attacker, target, diseaseListB);
                break;
            case (int)MonsterCareers.Spider:
            case (int)MonsterCareers.GiantScorpion:
                throw new System.NotSupportedException();
                // EntityEffectManager targetEffectManager = target.GetComponent<EntityEffectManager>();
                // if (targetEffectManager.FindIncumbentEffect<Paralyze>() == null)
                // {
                //     SpellRecord.SpellRecordData spellData;
                //     GameManager.Instance.EntityEffectBroker.GetClassicSpellRecord(66, out spellData);
                //     EffectBundleSettings bundle;
                //     GameManager.Instance.EntityEffectBroker.ClassicSpellRecordDataToEffectBundleSettings(spellData, BundleTypes.Spell, out bundle);
                //     EntityEffectBundle spell = new EntityEffectBundle(bundle, attacker.EntityBehaviour);
                //     EntityEffectManager attackerEffectManager = attacker.EntityBehaviour.GetComponent<EntityEffectManager>();
                //     attackerEffectManager.SetReadySpell(spell, true);
                // }
                break;
            case (int)MonsterCareers.Werewolf:
                throw new System.NotSupportedException();
                // random = UnityEngine.Random.Range(0f, 100f);
                // if (random <= specialInfectionChance && target.EntityType == EntityTypes.Player)
                // {
                //     // Werewolf
                //     EntityEffectBundle bundle = GameManager.Instance.PlayerEffectManager.CreateLycanthropyDisease(LycanthropyTypes.Werewolf);
                //     GameManager.Instance.PlayerEffectManager.AssignBundle(bundle, AssignBundleFlags.SpecialInfection);
                //     Debug.Log("Player infected by werewolf.");
                // }
                break;
            case (int)MonsterCareers.Nymph:
                FatigueDamage(attacker, target, damage);
                break;
            case (int)MonsterCareers.Wereboar:
                throw new System.NotSupportedException();
                // random = UnityEngine.Random.Range(0f, 100f);
                // if (random <= specialInfectionChance && target.EntityType == EntityTypes.Player)
                // {
                //     // Wereboar
                //     EntityEffectBundle bundle = GameManager.Instance.PlayerEffectManager.CreateLycanthropyDisease(LycanthropyTypes.Wereboar);
                //     GameManager.Instance.PlayerEffectManager.AssignBundle(bundle, AssignBundleFlags.SpecialInfection);
                //     Debug.Log("Player infected by wereboar.");
                // }
                break;
            case (int)MonsterCareers.Zombie:
                // Nothing in classic. DF Chronicles says 2% chance of disease, which seems like it was probably intended.
                // Diseases listed in DF Chronicles match those of mummy (except missing cholera, probably a mistake)
                if (Dice100.SuccessRoll(2))
                    InflictDisease(attacker, target, diseaseListC);
                break;
            case (int)MonsterCareers.Mummy:
                if (Dice100.SuccessRoll(5))
                    InflictDisease(attacker, target, diseaseListC);
                break;
            case (int)MonsterCareers.Vampire:
            case (int)MonsterCareers.VampireAncient:
                throw new System.NotSupportedException();
                // random = UnityEngine.Random.Range(0f, 100f);
                // if (random <= specialInfectionChance && target.EntityType == EntityTypes.Player)
                // {
                //     // Inflict stage one vampirism disease
                //     EntityEffectBundle bundle = GameManager.Instance.PlayerEffectManager.CreateVampirismDisease();
                //     GameManager.Instance.PlayerEffectManager.AssignBundle(bundle, AssignBundleFlags.SpecialInfection);
                //     Debug.Log("Player infected by vampire.");
                // }
                // else if (random <= 2.0f)
                // {
                //     InflictDisease(attacker, target, diseaseListA);
                // }
                break;
            case (int)MonsterCareers.Lamia:
                // Nothing in classic, but DF Chronicles says 2 pts of fatigue damage per health damage
                FatigueDamage(attacker, target, damage);
                break;
            default:
                break;
        }
    }

    public static void FatigueDamage(Monster attacker, Actor target, int damage)
    {
        // In classic, nymphs do 10-30 fatigue damage per hit, and lamias don't do any.
        // DF Chronicles says nymphs have "Energy Leech", which is a spell in
        // the game and not what they use, and for lamias "Every 1 pt of health damage = 2 pts of fatigue damage".
        // Lamia health damage is 5-15. Multiplying this by 2 may be where 10-30 came from. Nymph health damage is 1-5.
        // Not sure what was intended here, but using the "Every 1 pt of health damage = 2 pts of fatigue damage"
        // seems sensible, since the fatigue damage will scale to the health damage and lamias are a higher level opponent
        // than nymphs and will do more fatigue damage.
        target.SetFatigue(target.CurrentFatigue - ((damage * 2) * 64));

        // TODO: When nymphs drain the player's fatigue level to 0, the player is supposed to fall asleep for 14 days
        // and then wake up, according to DF Chronicles. This doesn't work correctly in classic. Classic does advance
        // time 14 days but the player dies like normal because of the "collapse from exhaustion near monsters = die" code.
    }

    /// <summary>
    /// Inflict a classic disease onto player.
    /// </summary>
    /// <param name="attacker">Source entity. Can be the same as target</param>
    /// <param name="target">Target entity - must be player.</param>
    /// <param name="diseaseList">Array of disease indices matching Diseases enum.</param>
    public static void InflictDisease(Actor attacker, Actor target, Diseases[] diseaseList)
    {
        // Must have a valid disease list
        if (diseaseList == null || diseaseList.Length == 0 || target.EntityType != EntityTypes.Player)
            return;

        // Only allow player to catch a disease this way
        var playerEntity = Main.Inst.hero;
        if (target != playerEntity)
            return;

        // Player cannot catch diseases at level 1 in classic. Maybe to keep new players from dying at the start of the game.
        if (playerEntity.Level != 1)
        {
            throw new System.NotSupportedException();
            // // Return if disease resisted
            // if (SavingThrow(DFCareer.Elements.DiseaseOrPoison, DFCareer.EffectFlags.Disease, target, 0) == 0)
            //     return;

            // // Select a random disease from disease array and validate range
            // int diseaseIndex = UnityEngine.Random.Range(0, diseaseList.Length);

            // // Infect player
            // Diseases diseaseType = diseaseList[diseaseIndex];
            // EntityEffectBundle bundle = GameManager.Instance.PlayerEffectManager.CreateDisease(diseaseType);
            // GameManager.Instance.PlayerEffectManager.AssignBundle(bundle, AssignBundleFlags.BypassSavingThrows);

            // Debug.LogFormat("Infected player with disease {0}", diseaseType.ToString());
        }
    }

    /// <summary>
    /// Allocate any equipment damage from a strike, and reduce item condition.
    /// </summary>
    public static void DamageEquipment(Actor attacker, Actor target, int damage, Item weapon, int struckBodyPart)
    {
        // // If damage was done by a weapon, damage the weapon and armor of the hit body part.
        // // In classic, shields are never damaged, only armor specific to the hitbody part is.
        // // Here, if an equipped shield covers the hit body part, it takes damage instead.
        // if (weapon != null && damage > 0)
        // {
        //     // TODO: If attacker is AI, apply Ring of Namira effect
        //     ApplyConditionDamageThroughPhysicalHit(weapon, attacker, damage);

        //     Item shield = target.ItemEquipTable.GetItem(EquipSlots.LeftHand);
        //     bool shieldTakesDamage = false;
        //     if (shield != null)
        //     {
        //         BodyParts[] protectedBodyParts = shield.GetShieldProtectedBodyParts();

        //         for (int i = 0; (i < protectedBodyParts.Length) && !shieldTakesDamage; i++)
        //         {
        //             if (protectedBodyParts[i] == (BodyParts)struckBodyPart)
        //                 shieldTakesDamage = true;
        //         }
        //     }

        //     if (shieldTakesDamage)
        //         ApplyConditionDamageThroughPhysicalHit(shield, target, damage);
        //     else
        //     {
        //         EquipSlots hitSlot = DaggerfallUnityItem.GetEquipSlotForBodyPart((BodyParts)struckBodyPart);
        //         DaggerfallUnityItem armor = target.ItemEquipTable.GetItem(hitSlot);
        //         if (armor != null)
        //             ApplyConditionDamageThroughPhysicalHit(armor, target, damage);
        //     }
        // }
    }

    /// <summary>
    /// Inflict a classic poison onto entity.
    /// </summary>
    /// <param name="attacker">Source entity. Can be the same as target</param>
    /// <param name="target">Target entity</param>
    /// <param name="poisonType">Classic poison type</param>
    /// <param name="bypassResistance">Whether it should bypass resistances</param>
    public static void InflictPoison(Actor attacker, Actor target, Poisons poisonType, bool bypassResistance)
    {
        throw new System.NotSupportedException();
        // // Target must have an entity behaviour and effect manager
        // EntityEffectManager effectManager = null;
        // if (target.EntityBehaviour != null)
        // {
        //     effectManager = target.EntityBehaviour.GetComponent<EntityEffectManager>();
        //     if (effectManager == null)
        //         return;
        // }
        // else
        // {
        //     return;
        // }

        // // Note: In classic, AI characters' immunity to poison is ignored, although the level 1 check below still gives rats immunity
        // DFCareer.Tolerance toleranceFlags = target.Career.Poison;
        // if (toleranceFlags == DFCareer.Tolerance.Immune)
        //     return;

        // // Handle player with racial resistance to poison
        // if (target is Hero)
        // {
        //     RaceTemplate raceTemplate = (target as PlayerEntity).GetLiveRaceTemplate();
        //     if ((raceTemplate.ImmunityFlags & DFCareer.EffectFlags.Poison) == DFCareer.EffectFlags.Poison)
        //         return;
        // }

        // if (bypassResistance || SavingThrow(DFCareer.Elements.DiseaseOrPoison, DFCareer.EffectFlags.Poison, target, 0) != 0)
        // {
        //     if (target.Level != 1)
        //     {
        //         // Infect target
        //         EntityEffectBundle bundle = effectManager.CreatePoison(poisonType);
        //         effectManager.AssignBundle(bundle, AssignBundleFlags.BypassSavingThrows);
        //     }
        // }
        // else
        // {
        //     Debug.LogFormat("Poison resisted by {0}.", target.EntityBehaviour.name);
        // }
    }

    public static int CalculateWeaponAttackDamage(Actor attacker, Actor target, int damageModifier, int weaponAnimTime, Item weapon)
    {
        int damage = UnityEngine.Random.Range(weapon.GetBaseDamageMin(), weapon.GetBaseDamageMax() + 1) + damageModifier;

        if (target != Main.Inst.hero)
        {
            if ((target as Monster).CareerIndex == (int)MonsterCareers.SkeletalWarrior)
            {
                // Apply edged-weapon damage modifier for Skeletal Warrior
                if ((weapon.flags & 0x10) == 0)
                    damage /= 2;

                // Apply silver weapon damage modifier for Skeletal Warrior
                // Arena applies a silver weapon damage bonus for undead enemies, which is probably where this comes from.
                if (weapon.NativeMaterialValue == (int)WeaponMaterialTypes.Silver)
                    damage *= 2;
            }
        }
        // TODO: Apply strength bonus from Mace of Molag Bal

        // Apply strength modifier
        damage += DamageModifier(attacker.Stats.LiveStrength);

        // Apply material modifier.
        // The in-game display in Daggerfall of weapon damages with material modifiers is incorrect. The material modifier is half of what the display suggests.
        damage += weapon.GetWeaponMaterialModifier();
        if (damage < 1)
            damage = 0;

        damage += GetBonusOrPenaltyByEnemyType(attacker, target);

        // Mod hook for adjusting final weapon damage. (no-op in DFU)
        // damage = AdjustWeaponAttackDamage(attacker, target, damage, weaponAnimTime, weapon);

        return damage;
    }

    public static int CalculateHandToHandAttackDamage(Actor attacker, Actor target, int damageModifier, bool player)
    {
        int minBaseDamage = CalculateHandToHandMinDamage(attacker.tSkills.GetLiveSkillValue(Skills.HandToHand));
        int maxBaseDamage = CalculateHandToHandMaxDamage(attacker.tSkills.GetLiveSkillValue(Skills.HandToHand));
        int damage = UnityEngine.Random.Range(minBaseDamage, maxBaseDamage + 1);

        // Apply damage modifiers.
        damage += damageModifier;

        // Apply strength modifier for players. It is not applied in classic despite what the in-game description for the Strength attribute says.
        if (player)
            damage += DamageModifier(attacker.Stats.LiveStrength);

        damage += GetBonusOrPenaltyByEnemyType(attacker, target);

        return damage;
    }

    public static int GetBonusOrPenaltyByEnemyType(Actor attacker, Actor target)
    {
        if (attacker == null || target == null)
            return 0;

        int damage = 0;
        // Apply bonus or penalty by opponent type.
        // In classic this is broken and only works if the attack is done with a weapon that has the maximum number of enchantments.
        if (target is Monster)
        {
            var enemyTarget = target as Monster;
            if (enemyTarget.Affinity == MobileAffinity.Human)
            {
                if (((int)attacker.Career.HumanoidAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
                    damage += attacker.Level;
                if (((int)attacker.Career.HumanoidAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
                    damage -= attacker.Level;
            }
            else if (enemyTarget.GetEnemyGroup() == DFCareer.EnemyGroups.Undead)
            {
                if (((int)attacker.Career.UndeadAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
                    damage += attacker.Level;
                if (((int)attacker.Career.UndeadAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
                    damage -= attacker.Level;
            }
            else if (enemyTarget.GetEnemyGroup() == DFCareer.EnemyGroups.Daedra)
            {
                if (((int)attacker.Career.DaedraAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
                    damage += attacker.Level;
                if (((int)attacker.Career.DaedraAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
                    damage -= attacker.Level;
            }
            else if (enemyTarget.GetEnemyGroup() == DFCareer.EnemyGroups.Animals)
            {
                if (((int)attacker.Career.AnimalsAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
                    damage += attacker.Level;
                if (((int)attacker.Career.AnimalsAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
                    damage -= attacker.Level;
            }
        }
        else if (target is Hero)
        {
            throw new System.NotSupportedException();
            // if (GameManager.Instance.PlayerEffectManager.HasVampirism()) // Vampires are undead, therefore use undead modifier
            // {
            //     if (((int)attacker.Career.UndeadAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
            //         damage += attacker.Level;
            //     if (((int)attacker.Career.UndeadAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
            //         damage -= attacker.Level;
            // }
            // else
            // {
            //     // Player is assumed humanoid
            //     if (((int)attacker.Career.HumanoidAttackModifier & (int)DFCareer.AttackModifier.Bonus) != 0)
            //         damage += attacker.Level;
            //     if (((int)attacker.Career.HumanoidAttackModifier & (int)DFCareer.AttackModifier.Phobia) != 0)
            //         damage -= attacker.Level;
            // }
        }

        return damage;
    }

    public static int DamageModifier(int strength)
    {
        return (int)Mathf.Floor((float)(strength - 50) / 5f);
    }

    public static int CalculateHandToHandMinDamage(int handToHandSkill)
    {
        return (handToHandSkill / 10) + 1;
    }

    public static int CalculateHandToHandMaxDamage(int handToHandSkill)
    {
        // Daggerfall Chronicles table lists hand-to-hand skills of 80 and above (45 through 79 are omitted)
        // as if they give max damage of (handToHandSkill / 5) + 2, but the hand-to-hand damage display in the character sheet
        // in classic Daggerfall shows it as continuing to be (handToHandSkill / 5) + 1
        return (handToHandSkill / 5) + 1;
    }

    /// <summary>
    /// Calculates whether an attack on a target is successful or not.
    /// </summary>
    public static bool CalculateSuccessfulHit(Actor attacker, Actor target, int chanceToHitMod, int struckBodyPart)
    {
        if (attacker == null || target == null)
            return false;

        int chanceToHit = chanceToHitMod;

        // 护甲对命中的影响
        // Get armor value for struck body part
        chanceToHit += CalculateArmorToHit(target, struckBodyPart);

        // 肾上腺素
        // Apply adrenaline rush modifiers.
        chanceToHit += CalculateAdrenalineRushToHit(attacker, target);

        // 魅力对命中的影响
        // Apply enchantment modifier
        chanceToHit += attacker.ChanceToHitModifier;

        // Apply stat differential modifiers. (default: luck and agility)
        chanceToHit += CalculateStatsToHit(attacker, target);

        // Apply skill modifiers. (default: dodge and crit strike)
        chanceToHit += CalculateSkillsToHit(attacker, target);

        // Apply monster modifier and biography adjustments.
        chanceToHit += CalculateAdjustmentsToHit(attacker, target);

        chanceToHit = Mathf.Clamp(chanceToHit, 3, 97);

        return Dice100.SuccessRoll(chanceToHit);
    }

    public static int CalculateAdjustmentsToHit(Actor attacker, Actor target)
    {
        var player = Main.Inst.hero;
        var AITarget = target as Monster;

        int chanceToHitMod = 0;

        // Apply hit mod from character biography
        if (target == player)
        {
            chanceToHitMod -= player.BiographyAvoidHitMod;
        }

        // Apply monster modifier.
        if ((target != player) && (AITarget.EntityType == EntityTypes.EnemyMonster))
        {
            chanceToHitMod += 40;
        }

        // DF Chronicles says -60 is applied at the end, but it actually seems to be -50.
        chanceToHitMod -= 50;

        return chanceToHitMod;
    }

    public static int CalculateSkillsToHit(Actor attacker, Actor target)
    {
        int chanceToHitMod = 0;

        // Apply dodging modifier.
        // This modifier is bugged in classic and the attacker's dodging skill is used rather than the target's.
        // DF Chronicles says the dodging calculation is (dodging / 10), but it actually seems to be (dodging / 4).
        chanceToHitMod -= target.tSkills.GetLiveSkillValue(Skills.Dodging) / 4;

        // Apply critical strike modifier.
        if (Dice100.SuccessRoll(attacker.tSkills.GetLiveSkillValue(Skills.CriticalStrike)))
        {
            chanceToHitMod += attacker.tSkills.GetLiveSkillValue(Skills.CriticalStrike) / 10;
        }

        return chanceToHitMod;
    }

    public static int CalculateStatsToHit(Actor attacker, Actor target)
    {
        int chanceToHitMod = 0;

        // Apply luck modifier.
        chanceToHitMod += (attacker.Stats.LiveLuck - target.Stats.LiveLuck) / 10;

        // Apply agility modifier.
        chanceToHitMod += (attacker.Stats.LiveAgility - target.Stats.LiveAgility) / 10;

        return chanceToHitMod;
    }

    public static int CalculateAdrenalineRushToHit(Actor attacker, Actor target)
    {
        const int adrenalineRushModifier = 5;
        const int improvedAdrenalineRushModifier = 8;

        int chanceToHitMod = 0;
        if (attacker.Career.AdrenalineRush && attacker.CurrentHealth < (attacker.MaxHealth / 8))
        {
            chanceToHitMod += (attacker.ImprovedAdrenalineRush) ? improvedAdrenalineRushModifier : adrenalineRushModifier;
        }

        if (target.Career.AdrenalineRush && target.CurrentHealth < (target.MaxHealth / 8))
        {
            chanceToHitMod -= (target.ImprovedAdrenalineRush) ? improvedAdrenalineRushModifier : adrenalineRushModifier;
        }
        return chanceToHitMod;
    }

    public static int MaxStatValue()
    {
        return 100;
    }

    public static int CalculateArmorToHit(Actor target, int struckBodyPart)
    {
        int armorValue = 0;
        if (struckBodyPart <= target.ArmorValues.Length)
        {
            armorValue = target.ArmorValues[struckBodyPart] + target.IncreasedArmorValueModifier + target.DecreasedArmorValueModifier;
        }
        return armorValue;
    }

    public static int CalculateWeaponToHit(Item weapon)
    {
        return weapon.GetWeaponMaterialModifier() * 10;
    }

    public static int CalculateStruckBodyPart()
    {
        int[] bodyParts = { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 6 };
        return bodyParts[UnityEngine.Random.Range(0, bodyParts.Length)];
    }

    // public static ToHitAndDamageMods CalculateSwingModifiers(FPSWeapon onscreenWeapon)
    // {
    //     ToHitAndDamageMods mods = new ToHitAndDamageMods();
    //     if (onscreenWeapon != null)
    //     {
    //         // The Daggerfall manual groups diagonal slashes to the left and right as if they are the same, but they are different.
    //         // Classic does not apply swing modifiers to unarmed attacks.
    //         if (onscreenWeapon.WeaponState == WeaponStates.StrikeUp)
    //         {
    //             mods.damageMod = -4;
    //             mods.toHitMod = 10;
    //         }
    //         if (onscreenWeapon.WeaponState == WeaponStates.StrikeDownRight)
    //         {
    //             mods.damageMod = -2;
    //             mods.toHitMod = 5;
    //         }
    //         if (onscreenWeapon.WeaponState == WeaponStates.StrikeDownLeft)
    //         {
    //             mods.damageMod = 2;
    //             mods.toHitMod = -5;
    //         }
    //         if (onscreenWeapon.WeaponState == WeaponStates.StrikeDown)
    //         {
    //             mods.damageMod = 4;
    //             mods.toHitMod = -10;
    //         }
    //     }
    //     return mods;
    // }

    public static ToHitAndDamageMods CalculateProficiencyModifiers(Actor attacker, Item weapon)
    {
        ToHitAndDamageMods mods = new ToHitAndDamageMods();
        if (weapon != null)
        {
            // Apply weapon proficiency
            if (((int)attacker.Career.ExpertProficiencies & weapon.GetWeaponSkillUsed()) != 0)
            {
                mods.damageMod = (attacker.Level / 3) + 1;
                mods.toHitMod = attacker.Level;
            }
        }
        // Apply hand-to-hand proficiency. Hand-to-hand proficiency is not applied in classic.
        else if (((int)attacker.Career.ExpertProficiencies & (int)ProficiencyFlags.HandToHand) != 0)
        {
            mods.damageMod = (attacker.Level / 3) + 1;
            mods.toHitMod = attacker.Level;
        }
        return mods;
    }

    public static ToHitAndDamageMods CalculateRacialModifiers(Actor attacker, Item weapon, Hero player)
    {
        ToHitAndDamageMods mods = new ToHitAndDamageMods();
        if (weapon != null)
        {
            if (player.RaceTemplate.ID == (int)Races.DarkElf)
            {
                mods.damageMod = attacker.Level / 4;
                mods.toHitMod = attacker.Level / 4;
            }
            else if (weapon.GetWeaponSkillIDAsShort() == (short)Skills.Archery)
            {
                if (player.RaceTemplate.ID == (int)Races.WoodElf)
                {
                    mods.damageMod = attacker.Level / 3;
                    mods.toHitMod = attacker.Level / 3;
                }
            }
            else if (player.RaceTemplate.ID == (int)Races.Redguard)
            {
                mods.damageMod = attacker.Level / 3;
                mods.toHitMod = attacker.Level / 3;
            }
        }
        return mods;
    }

    public static int CalculateBackstabChance(Hero player, Actor target, bool isEnemyFacingAwayFromPlayer)
    {
        if (isEnemyFacingAwayFromPlayer)
        {
            // player.TallySkill(DFCareer.Skills.Backstabbing, 1);
            return player.tSkills.GetLiveSkillValue(Skills.Backstabbing);
        }
        return 0;
    }

    public static int CalculateBackstabDamage(int damage, int backstabbingLevel)
    {
        if (backstabbingLevel > 1 && Dice100.SuccessRoll(backstabbingLevel))
        {
            damage *= 3;
            // string backstabMessage = TextManager.Instance.GetLocalizedText("successfulBackstab");
            // DaggerfallUI.Instance.PopupMessage(backstabMessage);
        }
        return damage;
    }

    public static int CalculateWeaponMinDamage(Weapons weapon)
    {
        switch (weapon)
        {
            case Weapons.Dagger:
            case Weapons.Tanto:
            case Weapons.Wakazashi:
            case Weapons.Shortsword:
            case Weapons.Broadsword:
            case Weapons.Staff:
            case Weapons.Mace:
                return 1;
            case Weapons.Longsword:
            case Weapons.Claymore:
            case Weapons.Battle_Axe:
            case Weapons.War_Axe:
            case Weapons.Flail:
                return 2;
            case Weapons.Saber:
            case Weapons.Katana:
            case Weapons.Dai_Katana:
            case Weapons.Warhammer:
                return 3;
            case Weapons.Short_Bow:
            case Weapons.Long_Bow:
                return 4;
            default:
                return 0;
        }
    }

    public static int CalculateWeaponMaxDamage(Weapons weapon)
    {
        switch (weapon)
        {
            case Weapons.Dagger:
                return 6;
            case Weapons.Tanto:
            case Weapons.Shortsword:
            case Weapons.Staff:
                return 8;
            case Weapons.Wakazashi:
                return 10;
            case Weapons.Broadsword:
            case Weapons.Saber:
            case Weapons.Battle_Axe:
            case Weapons.Mace:
                return 12;
            case Weapons.Flail:
                return 14;
            case Weapons.Longsword:
            case Weapons.Katana:
            case Weapons.War_Axe:
            case Weapons.Short_Bow:
                return 16;
            case Weapons.Claymore:
            case Weapons.Warhammer:
            case Weapons.Long_Bow:
                return 18;
            case Weapons.Dai_Katana:
                return 21;
            default:
                return 0;
        }
    }

    public static DFCareer.EnemyGroups GetEnemyEntityEnemyGroup(Monster e)
    {
        switch (e.CareerIndex)
        {
            case (int)MonsterCareers.Rat:
            case (int)MonsterCareers.GiantBat:
            case (int)MonsterCareers.GrizzlyBear:
            case (int)MonsterCareers.SabertoothTiger:
            case (int)MonsterCareers.Spider:
            case (int)MonsterCareers.Slaughterfish:
            case (int)MonsterCareers.GiantScorpion:
            case (int)MonsterCareers.Dragonling:
            case (int)MonsterCareers.Horse_Invalid:             // (grouped as undead in classic)
            case (int)MonsterCareers.Dragonling_Alternate:      // (grouped as undead in classic)
                return DFCareer.EnemyGroups.Animals;
            case (int)MonsterCareers.Imp:
            case (int)MonsterCareers.Spriggan:
            case (int)MonsterCareers.Orc:
            case (int)MonsterCareers.Centaur:
            case (int)MonsterCareers.Werewolf:
            case (int)MonsterCareers.Nymph:
            case (int)MonsterCareers.OrcSergeant:
            case (int)MonsterCareers.Harpy:
            case (int)MonsterCareers.Wereboar:
            case (int)MonsterCareers.Giant:
            case (int)MonsterCareers.OrcShaman:
            case (int)MonsterCareers.Gargoyle:
            case (int)MonsterCareers.OrcWarlord:
            case (int)MonsterCareers.Dreugh:                    // (grouped as undead in classic)
            case (int)MonsterCareers.Lamia:                     // (grouped as undead in classic)
                return DFCareer.EnemyGroups.Humanoid;
            case (int)MonsterCareers.SkeletalWarrior:
            case (int)MonsterCareers.Zombie:                    // (grouped as animal in classic)
            case (int)MonsterCareers.Ghost:
            case (int)MonsterCareers.Mummy:
            case (int)MonsterCareers.Wraith:
            case (int)MonsterCareers.Vampire:
            case (int)MonsterCareers.VampireAncient:
            case (int)MonsterCareers.Lich:
            case (int)MonsterCareers.AncientLich:
                return DFCareer.EnemyGroups.Undead;
            case (int)MonsterCareers.FrostDaedra:
            case (int)MonsterCareers.FireDaedra:
            case (int)MonsterCareers.Daedroth:
            case (int)MonsterCareers.DaedraSeducer:
            case (int)MonsterCareers.DaedraLord:
                return DFCareer.EnemyGroups.Daedra;
            case (int)MonsterCareers.FireAtronach:
            case (int)MonsterCareers.IronAtronach:
            case (int)MonsterCareers.FleshAtronach:
            case (int)MonsterCareers.IceAtronach:
                return DFCareer.EnemyGroups.None;

            default:
                return DFCareer.EnemyGroups.None;
        }
    }

      // Generates player health based on level and career hit points per level
    public static int RollMaxHealth(Hero player)
    {
        const int baseHealth = 25;
        int maxHealth = baseHealth + player.Career.HitPointsPerLevel;

        for (int i = 1; i < player.Level; i++)
        {
            maxHealth += CalculateHitPointsPerLevelUp(player);
        }

        return maxHealth;
    }

    // Calculate hit points player gains per level.
    public static int CalculateHitPointsPerLevelUp(Hero player)
    {
        int minRoll = player.Career.HitPointsPerLevel / 2;
        int maxRoll = player.Career.HitPointsPerLevel;
        DFRandom.Seed = (uint)Time.renderedFrameCount;
        int addHitPoints = DFRandom.random_range_inclusive(minRoll, maxRoll);
        addHitPoints += HitPointsModifier(player.Stats.PermanentEndurance);
        if (addHitPoints < 1)
            addHitPoints = 1;
        return addHitPoints;
    }

    public static int HitPointsModifier(int endurance)
    {
        return (int)Mathf.Floor((float)endurance / 10f) - 5;
    }

    // Calculate how much health the player should recover per hour of rest
    public static int CalculateHealthRecoveryRate(Hero player)
    {
        short medical = player.tSkills.GetLiveSkillValue(Skills.Medical);
        int endurance = player.Stats.LiveEndurance;
        int maxHealth = player.MaxHealth;
        // PlayerEnterExit playerEnterExit;
        // playerEnterExit = GameManager.Instance.PlayerGPS.GetComponent<PlayerEnterExit>();
        DFCareer.RapidHealingFlags rapidHealingFlags = player.Career.RapidHealing;

        short addToMedical = 60;

        if (rapidHealingFlags == DFCareer.RapidHealingFlags.Always)
            addToMedical = 100;
        // else if (DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.IsDay && !playerEnterExit.IsPlayerInside)
        // {
        //     if (rapidHealingFlags == DFCareer.RapidHealingFlags.InLight)
        //         addToMedical = 100;
        // }
        else if (rapidHealingFlags == DFCareer.RapidHealingFlags.InDarkness)
            addToMedical = 100;

        medical += addToMedical;

        return Mathf.Max((int)Mathf.Floor(HealingRateModifier(endurance) + medical * maxHealth / 1000), 1);
    }

    public static int HealingRateModifier(int endurance)
    {
        // Original Daggerfall seems to have a bug where negative endurance modifiers on healing rate
        // are applied as modifier + 1. Not recreating that here.
        return (int)Mathf.Floor((float)endurance / 10f) - 5;
    }


    public static int SpellPoints(int intelligence, float multiplier)
    {
        return (int)Mathf.Floor((float)intelligence * multiplier);
    }


    // Generates health for enemy classes based on level and class
    public static int RollEnemyClassMaxHealth(int level, int hitPointsPerLevel)
    {
        const int baseHealth = 10;
        int maxHealth = baseHealth;

        for (int i = 0; i < level; i++)
        {
            maxHealth += UnityEngine.Random.Range(1, hitPointsPerLevel + 1);
        }
        return maxHealth;
    }


     /// <summary>
    /// Gets a random material based on player level.
    /// Note, this is called by default RandomArmorMaterial function.
    /// </summary>
    /// <param name="playerLevel">Player level, possibly adjusted.</param>
    /// <returns>WeaponMaterialTypes value of material selected.</returns>
    public static WeaponMaterialTypes RandomMaterial(int playerLevel)
    {
        int levelModifier = (playerLevel - 10);

        if (levelModifier >= 0)
            levelModifier *= 2;
        else
            levelModifier *= 4;

        int randomModifier = UnityEngine.Random.Range(0, 256);

        int combinedModifiers = levelModifier + randomModifier;
        combinedModifiers = Mathf.Clamp(combinedModifiers, 0, 256);

        int material = 0; // initialize to iron

        // The higher combinedModifiers is, the higher the material
        while (Item.materialsByModifier[material] < combinedModifiers)
        {
            combinedModifiers -= Item.materialsByModifier[material++];
        }

        return (WeaponMaterialTypes)(material);
    }

    public static int CalculateCasterLevel(Actor caster, IEntityEffect effect)
    {
        return caster != null ? caster.Level : 1;
    }

    public static int ModifyEffectAmount(IEntityEffect sourceEffect, Actor target, int amount)
    {
        if (sourceEffect == null || sourceEffect.ParentBundle == null)
            return amount;

        int percentDamageOrDuration = SavingThrow(sourceEffect, target);
        float percent = percentDamageOrDuration / 100f;

        return (int)(amount * percent);
    }
    
    public static DFCareer.ToleranceFlags GetToleranceFlag(DFCareer.Tolerance tolerance)
    {
        DFCareer.ToleranceFlags flag = DFCareer.ToleranceFlags.Normal;
        switch (tolerance)
        {
            case DFCareer.Tolerance.Immune:
                flag = DFCareer.ToleranceFlags.Immune;
                break;
            case DFCareer.Tolerance.Resistant:
                flag = DFCareer.ToleranceFlags.Resistant;
                break;
            case DFCareer.Tolerance.LowTolerance:
                flag = DFCareer.ToleranceFlags.LowTolerance;
                break;
            case DFCareer.Tolerance.CriticalWeakness:
                flag = DFCareer.ToleranceFlags.CriticalWeakness;
                break;
        }

        return flag;
    }

    public static int SavingThrow(DFCareer.Elements elementType, DFCareer.EffectFlags effectFlags, Actor target, int modifier)
    {
        // Handle resistances granted by magical effects
        if (target.HasResistanceFlag(elementType))
        {
            int chance = target.GetResistanceChance(elementType);
            if (Dice100.SuccessRoll(chance))
                return 0;
        }

        // Magic effect resistances did not stop the effect. Try with career flags and biography modifiers
        int savingThrow = 50;
        DFCareer.ToleranceFlags toleranceFlags = DFCareer.ToleranceFlags.Normal;
        int biographyMod = 0;

        var playerEntity = Main.Inst.hero;
        if ((effectFlags & DFCareer.EffectFlags.Paralysis) != 0)
        {
            toleranceFlags |= GetToleranceFlag(target.Career.Paralysis);
            // Innate immunity if high elf. Start with 100 saving throw, but can be modified by
            // tolerance flags. Note this differs from classic, where high elves have 100% immunity
            // regardless of tolerance flags.
            if (target == playerEntity && playerEntity.Race == Races.HighElf)
                savingThrow = 100;
        }
        if ((effectFlags & DFCareer.EffectFlags.Magic) != 0)
        {
            toleranceFlags |= GetToleranceFlag(target.Career.Magic);
            if (target == playerEntity)
                biographyMod += playerEntity.BiographyResistMagicMod;
        }
        if ((effectFlags & DFCareer.EffectFlags.Poison) != 0)
        {
            toleranceFlags |= GetToleranceFlag(target.Career.Poison);
            if (target == playerEntity)
                biographyMod += playerEntity.BiographyResistPoisonMod;
        }
        if ((effectFlags & DFCareer.EffectFlags.Fire) != 0)
            toleranceFlags |= GetToleranceFlag(target.Career.Fire);
        if ((effectFlags & DFCareer.EffectFlags.Frost) != 0)
            toleranceFlags |= GetToleranceFlag(target.Career.Frost);
        if ((effectFlags & DFCareer.EffectFlags.Shock) != 0)
            toleranceFlags |= GetToleranceFlag(target.Career.Shock);
        if ((effectFlags & DFCareer.EffectFlags.Disease) != 0)
        {
            toleranceFlags |= GetToleranceFlag(target.Career.Disease);
            if (target == playerEntity)
                biographyMod += playerEntity.BiographyResistDiseaseMod;
        }

        // Note: Differing from classic implementation here. In classic
        // immune grants always 100% resistance and critical weakness is
        // always 0% resistance if there is no immunity. Here we are using
        // a method that allows mixing different tolerance flags, getting
        // rid of related exploits when creating a character class.
        if ((toleranceFlags & DFCareer.ToleranceFlags.Immune) != 0)
            savingThrow += 50;
        if ((toleranceFlags & DFCareer.ToleranceFlags.CriticalWeakness) != 0)
            savingThrow -= 50;
        if ((toleranceFlags & DFCareer.ToleranceFlags.LowTolerance) != 0)
            savingThrow -= 25;
        if ((toleranceFlags & DFCareer.ToleranceFlags.Resistant) != 0)
            savingThrow += 25;

        savingThrow += biographyMod + modifier;
        if (elementType == DFCareer.Elements.Frost && target == playerEntity && playerEntity.Race == Races.Nord)
            savingThrow += 30;
        else if (elementType == DFCareer.Elements.Magic && target == playerEntity && playerEntity.Race == Races.Breton)
            savingThrow += 30;

        // Handle perfect immunity of 100% or greater
        // Otherwise clamping to 5-95 allows a perfectly immune character to sometimes receive incoming payload
        // This doesn't seem to match immunity intent or player expectations from classic
        if (savingThrow >= 100)
            return 0;

        // Increase saving throw by MagicResist, equal to LiveWillpower / 10 (rounded down)
        savingThrow += target.MagicResist;

        savingThrow = Mathf.Clamp(savingThrow, 5, 95);

        int percentDamageOrDuration = 100;
        int roll = Dice100.Roll();

        if (roll <= savingThrow)
        {
            // Percent damage/duration is prorated at within 20 of failed roll, as described in DF Chronicles
            if (savingThrow - 20 <= roll)
                percentDamageOrDuration = 100 - 5 * (savingThrow - roll);
            else
                percentDamageOrDuration = 0;
        }

        return Mathf.Clamp(percentDamageOrDuration, 0, 100);
    }

    /// <summary>
    /// Gets DFCareer.EffectFlags from an effect.
    /// Note: If effect is not instanced by a bundle then it will not have an element type.
    /// </summary>
    /// <param name="effect">Source effect.</param>
    /// <returns>DFCareer.EffectFlags.</returns>
    public static DFCareer.EffectFlags GetEffectFlags(IEntityEffect effect)
    {
        DFCareer.EffectFlags result = DFCareer.EffectFlags.None;

        // Paralysis/Disease
        // if (effect is Paralyze)
        //     result |= DFCareer.EffectFlags.Paralysis;
        // if (effect is DiseaseEffect)
        //     result |= DFCareer.EffectFlags.Disease;

        // Elemental
        switch (effect.ParentBundle.elementType)
        {
            case ElementTypes.Fire:
                result |= DFCareer.EffectFlags.Fire;
                break;
            case ElementTypes.Cold:
                result |= DFCareer.EffectFlags.Frost;
                break;
            case ElementTypes.Poison:
                result |= DFCareer.EffectFlags.Poison;
                break;
            case ElementTypes.Shock:
                result |= DFCareer.EffectFlags.Shock;
                break;
            case ElementTypes.Magic:
                result |= DFCareer.EffectFlags.Magic;
                break;
        }

        return result;
    }

    /// <summary>
    /// Gets a resistance element based on effect element.
    /// </summary>
    /// <param name="effect">Source effect.</param>
    /// <returns>DFCareer.Elements</returns>
    public static DFCareer.Elements GetElementType(IEntityEffect effect)
    {
        // Always return magic for non-elemental (i.e. magic-only) effects
        if (effect.Properties.AllowedElements == ElementTypes.Magic)
            return DFCareer.Elements.Magic;

        // Otherwise return element selected by parent spell bundle
        switch (effect.ParentBundle.elementType)
        {
            case ElementTypes.Fire:
                return DFCareer.Elements.Fire;
            case ElementTypes.Cold:
                return DFCareer.Elements.Frost;
            case ElementTypes.Poison:
                return DFCareer.Elements.DiseaseOrPoison;
            case ElementTypes.Shock:
                return DFCareer.Elements.Shock;
            case ElementTypes.Magic:
                return DFCareer.Elements.Magic;
            default:
                return DFCareer.Elements.None;
        }
    }

    public static int GetResistanceModifier(DFCareer.EffectFlags effectFlags, Actor target)
    {
        int result = 0;

        // Will only read best matching resistance modifier from flags - priority is given to disease/poison over elemental
        // Note disease/poison resistance are both the same here for purposes of saving throw
        if ((effectFlags & DFCareer.EffectFlags.Disease) == DFCareer.EffectFlags.Disease || (effectFlags & DFCareer.EffectFlags.Poison) == DFCareer.EffectFlags.Poison)
            result = target.Resistances.LiveDiseaseOrPoison;
        else if ((effectFlags & DFCareer.EffectFlags.Fire) == DFCareer.EffectFlags.Fire)
            result = target.Resistances.LiveFire;
        else if ((effectFlags & DFCareer.EffectFlags.Frost) == DFCareer.EffectFlags.Frost)
            result = target.Resistances.LiveFrost;
        else if ((effectFlags & DFCareer.EffectFlags.Shock) == DFCareer.EffectFlags.Shock)
            result = target.Resistances.LiveShock;
        else if ((effectFlags & DFCareer.EffectFlags.Magic) == DFCareer.EffectFlags.Magic)
            result = target.Resistances.LiveMagic;

        return result;
    }

    public static int SavingThrow(IEntityEffect sourceEffect, Actor target)
    {
        if (sourceEffect == null || sourceEffect.ParentBundle == null)
            return 100;

        DFCareer.EffectFlags effectFlags = GetEffectFlags(sourceEffect);
        DFCareer.Elements elementType = GetElementType(sourceEffect);
        int modifier = GetResistanceModifier(effectFlags, target);

        return SavingThrow(elementType, effectFlags, target, modifier);
    }

    /// <summary>
    /// Performs complete gold and spellpoint costs for an array of effects.
    /// Also calculates multipliers for target type.
    /// </summary>
    /// <param name="effectEntries">EffectEntry array for spell.</param>
    /// <param name="targetType">Target type of spell.</param>
    /// <param name="totalGoldCostOut">Total gold cost out.</param>
    /// <param name="totalSpellPointCostOut">Total spellpoint cost out.</param>
    /// <param name="casterEntity">Caster entity. Assumed to be player if null.</param>
    /// <param name="minimumCastingCost">Spell point always costs minimum (e.g. from vampirism). Do not set true for reflection/absorption cost calculations.</param>
    public static SpellCost CalculateTotalEffectCosts(EffectEntry[] effectEntries, TargetTypes targetType, Actor casterEntity = null, bool minimumCastingCost = false)
    {
        const int castCostFloor = 5;

        SpellCost totalCost;
        totalCost.goldCost = 0;
        totalCost.spellPointCost = 0;

        // Must have effect entries
        if (effectEntries == null || effectEntries.Length == 0)
            return totalCost;

        // Add costs for each active effect slot
        for (int i = 0; i < effectEntries.Length; i++)
        {
            if (string.IsNullOrEmpty(effectEntries[i].Key))
                continue;

            (int goldCost, int spellPointCost) = CalculateEffectCosts(effectEntries[i], casterEntity);
            totalCost.goldCost += goldCost;
            totalCost.spellPointCost += spellPointCost;
        }

        // Multipliers for target type
        totalCost.goldCost = ApplyTargetCostMultiplier(totalCost.goldCost, targetType);
        totalCost.spellPointCost = ApplyTargetCostMultiplier(totalCost.spellPointCost, targetType);

        // Set vampire spell cost
        if (minimumCastingCost)
            totalCost.spellPointCost = castCostFloor;

        // Enforce minimum
        if (totalCost.spellPointCost < castCostFloor)
            totalCost.spellPointCost = castCostFloor;

        return totalCost;
    }

    public static int ApplyTargetCostMultiplier(int cost, TargetTypes targetType)
    {
        switch (targetType)
        {
            default:
            case TargetTypes.CasterOnly:                // x1.0
            case TargetTypes.ByTouch:
                // These do not change costs, just including here for completeness
                break;
            case TargetTypes.SingleTargetAtRange:       // x1.5
                cost = (int)(cost * 1.5f);
                break;
            case TargetTypes.AreaAroundCaster:          // x2.0
                cost = (int)(cost * 2.0f);
                break;
            case TargetTypes.AreaAtRange:               // x2.5
                cost = (int)(cost * 2.5f);
                break;
        }

        return cost;
    }

    /// <summary>
    /// Calculate effect costs from an EffectEntry.
    /// </summary>
    public static SpellCost CalculateEffectCosts(EffectEntry effectEntry, Actor casterEntity = null)
    {
        // Get effect template
        // IEntityEffect effectTemplate = Effects.GetEffectTemplate(effectEntry.Key);
        // if (effectTemplate == null)
            return new SpellCost { goldCost = 0, spellPointCost = 0 };

        // return CalculateEffectCosts(effectTemplate, effectEntry.Settings, casterEntity);
    }

    public static int MagicResist(int willpower)
    {
        return (int)Mathf.Floor((float)willpower / 10f);
    }

    public static bool IsItemStackable(Item item)
    {
        if (item.IsIngredient || item.IsPotion || (item.ItemGroup == ItemGroups.Books) ||
            item.IsOfTemplate(ItemGroups.Currency, (int)Currency.Gold_pieces) ||
            item.IsOfTemplate(ItemGroups.Weapons, (int)Weapons.Arrow) ||
            item.IsOfTemplate(ItemGroups.UselessItems2, (int)UselessItems2.Oil))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Gets a random armor material based on player level.
    /// </summary>
    /// <param name="playerLevel">Player level, possibly adjusted.</param>
    /// <returns>ArmorMaterialTypes value of material selected.</returns>
    public static ArmorMaterialTypes RandomArmorMaterial(int playerLevel)
    {
        // Random armor material
        int roll = Dice100.Roll();
        if (roll >= 70)
        {
            if (roll >= 90)
            {
                WeaponMaterialTypes plateMaterial = FormulaUtils.RandomMaterial(playerLevel);
                return (ArmorMaterialTypes)(0x0200 + plateMaterial);
            }
            else
                return ArmorMaterialTypes.Chain;
        }
        else
            return ArmorMaterialTypes.Leather;
    }

    // Gets vampire clan based on region
    public static VampireClans GetVampireClan(int regionIndex)
    {
        FactionFile.FactionData factionData;
        GameManager.Instance.PlayerEntity.FactionData.GetRegionFaction(regionIndex, out factionData);
        switch ((FactionFile.FactionIDs) factionData.vam)
        {
            case FactionFile.FactionIDs.The_Vraseth:
                return VampireClans.Vraseth;
            case FactionFile.FactionIDs.The_Haarvenu:
                return VampireClans.Haarvenu;
            case FactionFile.FactionIDs.The_Thrafey:
                return VampireClans.Thrafey;
            case FactionFile.FactionIDs.The_Lyrezi:
                return VampireClans.Lyrezi;
            case FactionFile.FactionIDs.The_Montalion:
                return VampireClans.Montalion;
            case FactionFile.FactionIDs.The_Khulari:
                return VampireClans.Khulari;
            case FactionFile.FactionIDs.The_Garlythi:
                return VampireClans.Garlythi;
            case FactionFile.FactionIDs.The_Anthotis:
                return VampireClans.Anthotis;
            case FactionFile.FactionIDs.The_Selenu:
                return VampireClans.Selenu;
        }

        // The Lyrezi are the default like in classic
        return VampireClans.Lyrezi;
    }

}

/// <summary>
/// A structure containing both the gold and spell point cost of either a single effect, or an entire spell
/// </summary>
public struct SpellCost
{
    public int goldCost;
    public int spellPointCost;

    public void Deconstruct(out int gcost, out int spcost)
    {
        gcost = goldCost;
        spcost = spellPointCost;
    }
}

public class Dice100
{
    private Dice100()
    {
    }

    public static int Roll()
    {
        return Random.Range(1, 101);
    }

    public static bool SuccessRoll(int chanceSuccess)
    {
        return Random.Range(0, 100) < chanceSuccess; // Same as Random.Range(1, 101) <= chanceSuccess
    }

    public static bool FailedRoll(int chanceSuccess)
    {
        return Random.Range(0, 100) >= chanceSuccess; // Same as Random.Range(1, 101) > chanceSuccess
    }
}

/// <summary>
/// Reimplementing key parts of Daggerfall's random library.
/// This ensures critical random number sequences (e.g. building names)
/// will match Daggerfall's output across all platforms.
/// </summary>
public static class DFRandom
{
    static ulong next = 1;
    static ulong savedNext;

    public static uint Seed
    {
        get { return (uint)next; }
        set { next = value; }
    }

    /// <summary>
    /// Seed random generator.
    /// </summary>
    /// <param name="seed">Seed int.</param>
    public static void srand(int seed)
    {
        next = (uint)seed;
    }

    /// <summary>
    /// Seed random generator.
    /// </summary>
    /// <param name="seed">Seed uint.</param>
    public static void srand(uint seed)
    {
        next = seed;
    }

    /// <summary>
    /// Generate a random number.
    /// </summary>
    /// <returns></returns>
    public static uint rand()
    {
        next = next * 1103515245 + 12345;
        return ((uint)((next >> 16) & 0x7FFF));
    }

    /// <summary>
    /// Generates a random number between min and max (exclusive).
    /// </summary>
    /// <param name="min">Minimum number.</param>
    /// <param name="max">Maximum number (exclusive).</param>
    /// <returns>Random number between min and max - 1.</returns>
    public static int random_range(int min, int max)
    {
        return (int)rand() % (max - min) + min;
    }

    /// <summary>
    /// Generates a random number between min and max (inclusive).
    /// </summary>
    /// <param name="min">Minimum number.</param>
    /// <param name="max">Maximum number (inclusive).</param>
    /// <returns>Random number between min and max - 1.</returns>
    public static int random_range_inclusive(int min, int max)
    {
        return (int)rand() % (max - min + 1) + min;
    }

    /// <summary>
    /// Generates a random number between 0 and max (exclusive).
    /// </summary>
    /// <param name="max">Maximum number (exclusive).</param>
    /// <returns>Random number between 0 and max - 1.</returns>
    public static int random_range(int max)
    {
        return (int)rand() % max;
    }

    public static void SaveSeed()
    {
        savedNext = next;
    }

    public static void RestoreSeed()
    {
        next = savedNext;
    }
}