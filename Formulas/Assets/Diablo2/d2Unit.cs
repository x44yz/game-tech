using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Unit : Unit
    {
        [Header("---- D2UNIT ----")]
        public int charId;
        public int weaponId;
        public int armorId;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }
    }
}

