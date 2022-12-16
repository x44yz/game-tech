using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Monster : Unit
    {
        public MonsterMode mode;
        public MonsterData data;
        public int armorClass;
        public int maxHitPoints;
        public int hitPoints;

        protected override void OnHit(Unit attacker)
        {
            base.OnHit(attacker);
            Debug.Log("xx-- d2Monster.OnHit");
        }

        public bool isPossibleToHit
        {
            // TODO
            get { return true; }
        }

        public bool tryLiftGargoyle()
        {
            // TODO
            return false;
        }
    }
}
