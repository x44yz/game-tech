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

        void Awake()
        {
            EatState eat = new EatState();
            SleepState sleep = new SleepState();
            WorkState work = new WorkState();
        
            fsm = new StateMachine();
            fsm.AddTransition()
        }

        void Update()
        {
            fsm.Update(Time.deltaTime);
        }
    }
}

