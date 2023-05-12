using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero : Actor
{
    public bool WeaponDamage(Item strikingWeapon, bool arrowHit, bool arrowSummoned, Transform hitTransform, Vector3 impactPosition, Vector3 direction)
    {
        DaggerfallEntityBehaviour entityBehaviour = hitTransform.GetComponent<DaggerfallEntityBehaviour>();
        var entityMobileUnit = hitTransform.GetComponentInChildren<MobileUnit>();

        // Check if hit an entity and remove health

        EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;

        // Calculate damage
        int animTime = (int)(ScreenWeapon.GetAnimTime() * 1000);    // Get animation time, converted to ms.
        bool isEnemyFacingAwayFromPlayer = entityMobileUnit.IsBackFacing &&
            entityMobileUnit.EnemyState != MobileStates.SeducerTransform1 &&
            entityMobileUnit.EnemyState != MobileStates.SeducerTransform2;
        int damage = FormulaHelper.CalculateAttackDamage(playerEntity, enemyEntity, isEnemyFacingAwayFromPlayer, animTime, strikingWeapon);

        // Break any "normal power" concealment effects on player
        if (playerEntity.IsMagicallyConcealedNormalPower && damage > 0)
            EntityEffectManager.BreakNormalPowerConcealmentEffects(GameManager.Instance.PlayerEntityBehaviour);

        // Add arrow to target's inventory
        if (arrowHit && !arrowSummoned)
        {
            DaggerfallUnityItem arrow = ItemBuilder.CreateWeapon(Weapons.Arrow, WeaponMaterialTypes.None);
            arrow.stackCount = 1;
            enemyEntity.Items.AddItem(arrow);
        }

        // Play hit sound and trigger blood splash at hit point
        if (damage > 0)
        {
            if (usingRightHand)
                enemySounds.PlayHitSound(currentRightHandWeapon);
            else
                enemySounds.PlayHitSound(currentLeftHandWeapon);

            EnemyBlood blood = hitTransform.GetComponent<EnemyBlood>();
            if (blood)
            {
                blood.ShowBloodSplash(enemyEntity.MobileEnemy.BloodIndex, impactPosition);
            }

            // Knock back enemy based on damage and enemy weight
            if (enemyMotor)
            {
                if (enemyMotor.KnockbackSpeed <= (5 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10)) &&
                    entityBehaviour.EntityType == EntityTypes.EnemyClass ||
                    enemyEntity.MobileEnemy.Weight > 0)
                {
                    float enemyWeight = enemyEntity.GetWeightInClassicUnits();
                    float tenTimesDamage = damage * 10;
                    float twoTimesDamage = damage * 2;

                    float knockBackAmount = ((tenTimesDamage - enemyWeight) * 256) / (enemyWeight + tenTimesDamage) * twoTimesDamage;
                    float KnockbackSpeed = (tenTimesDamage / enemyWeight) * (twoTimesDamage - (knockBackAmount / 256));
                    KnockbackSpeed /= (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10);

                    if (KnockbackSpeed < (15 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10)))
                        KnockbackSpeed = (15 / (PlayerSpeedChanger.classicToUnitySpeedUnitRatio / 10));
                    enemyMotor.KnockbackSpeed = KnockbackSpeed;
                    enemyMotor.KnockbackDirection = direction;
                }
            }

            if (DaggerfallUnity.Settings.CombatVoices && entityBehaviour.EntityType == EntityTypes.EnemyClass && Dice100.SuccessRoll(40))
            {
                Genders gender;
                if (entityMobileUnit.Enemy.Gender == MobileGender.Male || enemyEntity.MobileEnemy.ID == (int)MobileTypes.Knight_CityWatch)
                    gender = Genders.Male;
                else
                    gender = Genders.Female;

                bool heavyDamage = damage >= enemyEntity.MaxHealth / 4;
                enemySounds.PlayCombatVoice(gender, false, heavyDamage);
            }
        }
        else
        {
            if ((!arrowHit && !enemyEntity.MobileEnemy.ParrySounds) || strikingWeapon == null)
                ScreenWeapon.PlaySwingSound();
            else if (enemyEntity.MobileEnemy.ParrySounds)
                enemySounds.PlayParrySound();
        }

        // Handle weapon striking enchantments - this could change damage amount
        if (strikingWeapon != null && strikingWeapon.IsEnchanted)
        {
            EntityEffectManager effectManager = GetComponent<EntityEffectManager>();
            if (effectManager)
                damage = effectManager.DoItemEnchantmentPayloads(EnchantmentPayloadFlags.Strikes, strikingWeapon, GameManager.Instance.PlayerEntity.Items, enemyEntity.EntityBehaviour, damage);
            strikingWeapon.RaiseOnWeaponStrikeEvent(entityBehaviour, damage);
        }

        // Remove health
        enemyEntity.DecreaseHealth(damage);

        // Handle attack from player
        enemyEntity.EntityBehaviour.HandleAttackFromSource(GameManager.Instance.PlayerEntityBehaviour);

        // Allow custom race handling of weapon hit against enemies, e.g. vampire feeding or lycanthrope killing
        RacialOverrideEffect racialOverride = GameManager.Instance.PlayerEffectManager.GetRacialOverrideEffect();
        if (racialOverride != null)
            racialOverride.OnWeaponHitEntity(GameManager.Instance.PlayerEntity, entityBehaviour.Entity);

        return true;
    }
}
