using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player reflex settings for enemy speed.
/// </summary>
public enum PlayerReflexes
{
    VeryHigh = 0,
    High = 1,
    Average = 2,
    Low = 3,
    VeryLow = 4,
}

// [Serializable]
public class Hero : Actor
{
    public Races Race { get { return (Races)RaceTemplate.ID; } }
    public RaceTemplate RaceTemplate { get { return GetLiveRaceTemplate(); } }

    protected int biographyResistDiseaseMod = 0;
    protected int biographyResistMagicMod = 0;
    protected int biographyAvoidHitMod = 0;
    protected int biographyResistPoisonMod = 0;
    protected int biographyFatigueMod = 0;
    protected int biographyReactionMod = 0;

    public int BiographyResistDiseaseMod { get { return biographyResistDiseaseMod; } set { biographyResistDiseaseMod = value; } }
    public int BiographyResistMagicMod { get { return biographyResistMagicMod; } set { biographyResistMagicMod = value; } }
    public int BiographyAvoidHitMod { get { return biographyAvoidHitMod; } set { biographyAvoidHitMod = value; } }
    public int BiographyResistPoisonMod { get { return biographyResistPoisonMod; } set { biographyResistPoisonMod = value; } }
    public int BiographyFatigueMod { get { return biographyFatigueMod; } set { biographyFatigueMod = value; } }
    public int BiographyReactionMod { get { return biographyReactionMod; } set { biographyReactionMod = value; } }

    protected PlayerReflexes reflexes;
    public PlayerReflexes Reflexes { get { return reflexes; } set { reflexes = value; } }

    public Item currentRightHandWeapon = null;
    public Item currentLeftHandWeapon = null;
    public Item lastBowUsed = null;
    public Item LastBowUsed { get { return lastBowUsed; } }

    protected RaceTemplate raceTemplate;
    public RaceTemplate GetLiveRaceTemplate()
    {
        // Look for racial override effect
        // RacialOverrideEffect racialOverrideEffect = GameManager.Instance.PlayerEffectManager.GetRacialOverrideEffect();
        // if (racialOverrideEffect != null)
        //     return racialOverrideEffect.CustomRace;
        // else
        return raceTemplate;
    }

    /// <summary>
    /// Assigns player entity settings from a character document.
    /// </summary>
    public void AssignCharacter(int level = 1 /*, int maxHealth = 0, bool fillVitals = true*/)
    {
        entityType = EntityTypes.Player;

        // TODO: Add some bonus points to stats
        career = Classes.GetClassCareerTemplate(ClassCareers.Mage);
        if (career != null)
        {
            raceTemplate = RacesTemp.GetRaceTemplate(Races.Breton);
            // faceIndex = 0;
            reflexes = PlayerReflexes.Average;
            // gender = Genders.Male;
            stats.SetPermanentFromCareer(career);
            this.level = level;
            maxHealth = FormulaUtils.RollMaxHealth(this);
            name = "Nameless";
            // stats.SetDefaults();
            skills.SetDefaults();
            FillVitalSigns();
            for (int i = 0; i < ArmorValues.Length; i++)
            {
                ArmorValues[i] = 100;
            }
        }

        Debug.Log("Assigned character " + this.name);
    }

    public bool WeaponDamage(Item strikingWeapon, bool arrowHit, bool arrowSummoned, Actor target, Vector3 impactPosition, Vector3 direction)
    {
        // Calculate damage
        int animTime = 1;    // Get animation time, converted to ms.
        bool isEnemyFacingAwayFromPlayer = false;
        int damage = FormulaUtils.CalculateAttackDamage(this, target, isEnemyFacingAwayFromPlayer, animTime, strikingWeapon);

        // Play hit sound and trigger blood splash at hit point
        if (damage > 0)
        {
            // Knock back enemy based on damage and enemy weight
            // if (enemyMotor.KnockbackSpeed <= (5 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10)) &&
            //     entityBehaviour.EntityType == EntityTypes.EnemyClass ||
            //     target.Weight > 0)
            // {
            //     float enemyWeight = enemyEntity.GetWeightInClassicUnits();
            //     float tenTimesDamage = damage * 10;
            //     float twoTimesDamage = damage * 2;

            //     float knockBackAmount = ((tenTimesDamage - enemyWeight) * 256) / (enemyWeight + tenTimesDamage) * twoTimesDamage;
            //     float KnockbackSpeed = (tenTimesDamage / enemyWeight) * (twoTimesDamage - (knockBackAmount / 256));
            //     KnockbackSpeed /= (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10);

            //     if (KnockbackSpeed < (15 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10)))
            //         KnockbackSpeed = (15 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10));
            //     enemyMotor.KnockbackSpeed = KnockbackSpeed;
            //     enemyMotor.KnockbackDirection = direction;
            // }
        }

        // Handle weapon striking enchantments - this could change damage amount
        // if (strikingWeapon != null && strikingWeapon.IsEnchanted)
        // {
        //     EntityEffectManager effectManager = GetComponent<EntityEffectManager>();
        //     if (effectManager)
        //         damage = effectManager.DoItemEnchantmentPayloads(EnchantmentPayloadFlags.Strikes, strikingWeapon, GameManager.Instance.PlayerEntity.Items, enemyEntity.EntityBehaviour, damage);
        //     strikingWeapon.RaiseOnWeaponStrikeEvent(entityBehaviour, damage);
        // }

        // Remove health
        target.DecreaseHealth(damage);

        // Handle attack from player
        // enemyEntity.EntityBehaviour.HandleAttackFromSource(GameManager.Instance.PlayerEntityBehaviour);

        // Allow custom race handling of weapon hit against enemies, e.g. vampire feeding or lycanthrope killing
        // RacialOverrideEffect racialOverride = GameManager.Instance.PlayerEffectManager.GetRacialOverrideEffect();
        // if (racialOverride != null)
        //     racialOverride.OnWeaponHitEntity(GameManager.Instance.PlayerEntity, entityBehaviour.Entity);

        return true;
    }
}
