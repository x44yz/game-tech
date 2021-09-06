using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class Transition
    {
        public State from;
        public State to;

        public Transition(State from, State to)
        {
            this.from = from;
            this.to = to;
        }
        
        public virtual bool IsValid() { return true; }
        // public virtual State GetNextState() { return null; }
        public virtual void OnTransition() {}
    }
}
