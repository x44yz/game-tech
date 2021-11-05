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
        protected Func<bool> condition;
        protected Action onTransition;

        public Transition(State from, State to, Func<bool> condition, Action onTransition)
        {
            this.from = from;
            this.to = to;
            this.condition = condition;
            this.onTransition = onTransition;
        }
        
        public virtual bool IsValid()
        {
            if (condition == null)
                return false;
            return condition.Invoke();
        }

        public virtual void OnTransition()
        {
            if (onTransition != null)
                onTransition.Invoke();
        }
    }
}
