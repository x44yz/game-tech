using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class Transition
    {
        public State from;
        public State to;
        public Func<bool> condition;

        public Transition(State from, State to, Func<bool> condition)
        {
            this.from = from;
            this.to = to;
            this.condition = condition;
        }
        
        public virtual bool IsValid()
        { 
            return condition == null || condition.Invoke();
        }

        // public virtual State GetNextState() { return null; }
        public virtual void OnTransition() {}
    }
}
