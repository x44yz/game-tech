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

        // 
        public bool py_attack_real(angMonster mon)
        {
            /* The weapon used */

            /* See if the player hit */
            var success = test_hit(chance_of_melee_hit(p, obj, mon), mon->race->ac);

            /* If a miss, skip this hit */
            if (!success) {
                msgt(MSG_MISS, "You miss %s.", m_name);

                /* Small chance of bloodlust side-effects */
                if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
                    msg("You feel strange...");
                    player_over_exert(p, PY_EXERT_SCRAMBLE, 20, 20);
                }

                return false;
            }
        }

        public bool test_hit()
        {

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

