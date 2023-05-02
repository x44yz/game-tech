using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DamageType
    {
        public string type;
        public System.Func<Actor, Actor, int, int> projector;
    }
}