using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormulaUtils
{
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
        PlayerEntity player = GameManager.Instance.PlayerEntity;
        short skillID = 0;

        // Choose whether weapon-wielding enemies use their weapons or weaponless attacks.
        // In classic, weapon-wielding enemies use the damage values of their weapons
        // instead of their weaponless values.
        // For some enemies this gives lower damage than similar-tier monsters
        // and the weaponless values seems more appropriate, so here
        // enemies will choose to use their weaponless attack if it is more damaging.
        // 如果空手的伤害比持武器伤害大，默认空手
        EnemyEntity AIAttacker = attacker as EnemyEntity;
        if (AIAttacker != null && weapon != null)
        {
            int weaponAverage = (weapon.GetBaseDamageMin() + weapon.GetBaseDamageMax()) / 2;
            int noWeaponAverage = (AIAttacker.MobileEnemy.MinDamage + AIAttacker.MobileEnemy.MaxDamage) / 2;

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
                if (attacker == player)
                {
                    DaggerfallUI.Instance.PopupMessage(TextManager.Instance.GetLocalizedText("materialIneffective"));
                }
                return 0;
            }
            // Get weapon skill used
            skillID = weapon.GetWeaponSkillIDAsShort();
        }
        else
        {
            skillID = (short)DFCareer.Skills.HandToHand;
        }

        chanceToHitMod = attacker.Skills.GetLiveSkillValue(skillID);

        // 玩家
        if (attacker == player)
        {
            // Apply swing modifiers
            ToHitAndDamageMods swingMods = CalculateSwingModifiers(GameManager.Instance.WeaponManager.ScreenWeapon);
            damageModifiers += swingMods.damageMod;
            chanceToHitMod += swingMods.toHitMod;

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
        if (skillID == (short)DFCareer.Skills.HandToHand)
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
                        minBaseDamage = AIAttacker.MobileEnemy.MinDamage;
                        maxBaseDamage = AIAttacker.MobileEnemy.MaxDamage;
                    }
                    else if (attackNumber == 1)
                    {
                        minBaseDamage = AIAttacker.MobileEnemy.MinDamage2;
                        maxBaseDamage = AIAttacker.MobileEnemy.MaxDamage2;
                    }
                    else if (attackNumber == 2)
                    {
                        minBaseDamage = AIAttacker.MobileEnemy.MinDamage3;
                        maxBaseDamage = AIAttacker.MobileEnemy.MaxDamage3;
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
            chanceToHitMod = AdjustWeaponHitChanceMod(attacker, target, chanceToHitMod, weaponAnimTime, weapon);

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
        if (target == player)
        {
            DaggerfallUnityItem[] equippedItems = target.ItemEquipTable.EquipTable;
            DaggerfallUnityItem item = null;
            if (equippedItems.Length != 0)
            {
                if (IsRingOfNamira(equippedItems[(int)EquipSlots.Ring0]) || IsRingOfNamira(equippedItems[(int)EquipSlots.Ring1]))
                {
                    IEntityEffect effectTemplate = GameManager.Instance.EntityEffectBroker.GetEffectTemplate(RingOfNamiraEffect.EffectKey);
                    effectTemplate.EnchantmentPayloadCallback(EnchantmentPayloadFlags.None,
                        targetEntity: AIAttacker.EntityBehaviour,
                        sourceItem: item,
                        sourceDamage: damage);
                }
            }
        }
        //Debug.LogFormat("Damage {0} applied, animTime={1}  ({2})", damage, weaponAnimTime, GameManager.Instance.WeaponManager.ScreenWeapon.WeaponState);

        return damage;
    }
}
