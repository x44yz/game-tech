using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace angband
{
    public class angPlayer : Unit
    {   
        [Range(0f, 100f)]
        public float m_fHealth = 100f;
        [Range(0f, 100f)]
        public float m_fArmour = 0f;
        public angWeapon handWeapon;

        [Header("RUNTIME")]
        public bool m_bAdrenalineActive = false; // 是否注射肾上腺素
        public float m_attackStrength; // 攻击加成
        public int m_numNearPeds { get { return m_nearPeds.Count; } }
        // 筛选 30米（平面） 范围内的角色
        public List<angPlayer> m_nearPeds = new List<angPlayer>();

        protected override void OnStart()
        {
            base.OnStart();

            handWeapon = GetComponent<angWeapon>();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        public angWeapon GetWeapon()
        {
            return handWeapon;
        }

        // 格斗伤害
        public void FightStrike()
        {
            // 找寻最近的对象
            var nearPed = m_nearPeds[0];
            var closestPedPiece = ePedPieceTypes.PEDPIECE_TORSO;

            int fightMoveDamage = 1; // TODO:根据动作确定伤害
            int damageMult = fightMoveDamage * (UnityEngine.Random.Range(0, 1) + 2) + 1; // 动作伤害缩放2-3倍，最小值1

            // 如果是攻击者是玩家并且注射肾上腺素，默认伤害20
            if (IsPlayer())
            {
                if (m_bAdrenalineActive)
                    damageMult = 20;
            }
            else
            {
                damageMult = (int)(damageMult * m_attackStrength);
            }

	        // 0-forward, 1-left, 2-backward, 3-right.
	        int direction = 0;

            nearPed.InflictDamage(this, eWeaponType.WEAPONTYPE_UNARMED, damageMult * 3.0f, closestPedPiece, direction);
        }

        public bool IsPlayer()
        {
            return true;
        }

        // direction: 0-forward, 1-left, 2-backward, 3-right.
        public bool InflictDamage(angPlayer damagedBy, eWeaponType method, float damage, ePedPieceTypes pedPiece, int direction)
        {
            float healthImpact = 0f;

            // TODO: 0.33f ?
            if (IsPlayer())
                healthImpact = damage * 0.33f;
            // else
            //     healthImpact = damage * m_pedStats->m_defendWeakness;

            angTest.Inst.ShowDamageText(this, (int)healthImpact);

            // 护甲
            if (m_fArmour != 0f)
            {
                if (healthImpact < m_fArmour) 
                {
                    m_fArmour = m_fArmour - healthImpact;
                    healthImpact = 0f;
                }
                else
                {
                    healthImpact = healthImpact - m_fArmour;
                    m_fArmour = 0f;
                }
            }

            m_fHealth -= healthImpact;

            if (m_fHealth >= 1f)
                return false;

            // dead
            m_fHealth = 0f;
            return true;
        }

        // ---------------------------------------------------------------------
#region EditorTest
        [Button("LevelUp", EButtonEnableMode.Playmode)]
        private void TestLevelUp()
        {
            // NextPlrLevel();
        }

#endregion // EditorTest
    }
}

