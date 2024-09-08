using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class State
    {
        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void OnUpdate(float dt) {}
    }
}