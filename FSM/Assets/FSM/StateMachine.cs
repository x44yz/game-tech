using System.Collections;
using System.Collections.Generic;

namespace AI.FSM
{
    public class StateMachine
    {
        public State curState;
        public Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();

        public void Update(float dt)
        {
            if (curState == null)
                return;

            if (transitions.TryGetValue(curState, out List<Transition> tsList))
            {
                foreach (var ts in tsList)
                {
                    if (ts.IsValid())
                    {
                        SetState(ts.to);
                        break;
                    }
                }
            }

            curState.OnUpdate(dt);
        }

        public void SetState(State st)
        {
            if (curState != null)
                curState.OnExit();

            curState = st;
            curState.OnEnter();
        }

        public void AddTransition(Transition ts)
        {
            List<Transition> tsList;
            if (!transitions.TryGetValue(ts.from, out tsList))
            {
                tsList = new List<Transition>();
                transitions.Add(ts.from, tsList);
            }
            tsList.Add(ts);
        }
    }
}