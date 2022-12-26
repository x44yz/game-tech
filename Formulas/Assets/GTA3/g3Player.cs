using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace gta3
{
    public class g3Player : Unit
    {   
        [Range(0f, 100f)]
        public float m_fHealth = 100f;
        [Range(0f, 100f)]
        public float m_fArmour = 0f;

        protected override void OnStart()
        {
            base.OnStart();

            TestRefreshEquip();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        g3Weapon GetWeapon()
        {

        }

        void FightStrike()
        {
            // find near

            nearPed->InflictDamage(this, eWeaponType.WEAPONTYPE_UNARMED, damageMult * 3.0f, closestPedPiece, direction);
        }

        bool IsPlayer()
        {
            return true;
        }

        bool InflictDamage(g3Player damagedBy, eWeaponType method, float damage, ePedPieceTypes pedPiece, int direction)
        {
            float healthImpact = 0f;

            // TODO: 0.33f ?
            if (IsPlayer())
                healthImpact = damage * 0.33f;
            // else
            //     healthImpact = damage * m_pedStats->m_defendWeakness;

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

        [Button("RefreshEquip", EButtonEnableMode.Playmode)]
        private void TestRefreshEquip()
        {
            // RemoveEquipment(inv_body_loc.INVLOC_HEAD, true);
            // RemoveEquipment(inv_body_loc.INVLOC_RING_LEFT, true);
            // RemoveEquipment(inv_body_loc.INVLOC_RING_RIGHT, true);
            // RemoveEquipment(inv_body_loc.INVLOC_AMULET, true);
            // RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
            // RemoveEquipment(inv_body_loc.INVLOC_HAND_RIGHT, true);
            // RemoveEquipment(inv_body_loc.INVLOC_CHEST, true);

            // if (testHeadItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_HEAD, (_item_indexes)testHeadItemId);
            // if (testLeftRingItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_RING_LEFT, (_item_indexes)testLeftRingItemId);
            // if (testRightRingItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_RING_RIGHT, (_item_indexes)testRightRingItemId);
            // if (testAmuletItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_AMULET, (_item_indexes)testAmuletItemId);
            // if (testLeftHandItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_HAND_LEFT, (_item_indexes)testLeftHandItemId);
            // if (testRightHandItemId != (int)_item_indexes.IDI_NONE)
            // {
            //     var dat = dfData.AllItemsList[testRightHandItemId];
            //     if (dat.iLoc == item_equip_type.ILOC_TWOHAND)
            //     {
            //         Debug.LogWarning("right hand is two-hand weapon, remove left hand equip");
            //         testLeftHandItemId = (int)_item_indexes.IDI_NONE;
            //         RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
            //     }
            //     ChangePlayerItems(inv_body_loc.INVLOC_HAND_RIGHT, (_item_indexes)testRightHandItemId);
            // }
            // if (testChestItemId != (int)_item_indexes.IDI_NONE)
            //     ChangePlayerItems(inv_body_loc.INVLOC_CHEST, (_item_indexes)testChestItemId);
        }

        // [Dropdown("TestGetHeadItemIds")]
        // public int testHeadItemId;
        // public static DropdownList<int> testHeadItemIds = null;
        // private DropdownList<int> TestGetHeadItemIds()
        // {
        //     if (testHeadItemIds == null)
        //         testHeadItemIds = TestGetItemIds(item_equip_type.ILOC_HELM);
        //     return testHeadItemIds;
        // }

#endregion // EditorTest
    }
}

