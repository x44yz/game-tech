using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.FSM;

namespace Test
{


    public class Actor : MonoBehaviour
    {
        public List<Point> points = new List<Point>();

        private StateMachine fsm;

        public int Hungry;
        public int Fatigue;

        void Awake()
        {
            EatState eat = new EatState();
            SleepState sleep = new SleepState();
            WorkState work = new WorkState();
        
            fsm = new StateMachine();
            fsm.AddTransition(new EatToSleepTransition(eat, sleep));
            fsm.AddTransition(new SleepToEatTransition(sleep, eat));

            // set default
            fsm.SetState(sleep);
        }

        void Update()
        {
            fsm.Update(Time.deltaTime);
        }
    }
}

