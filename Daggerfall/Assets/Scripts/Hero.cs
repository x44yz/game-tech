using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    public RaceTemplate RaceTemplate { get { return GetLiveRaceTemplate(); } }

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
