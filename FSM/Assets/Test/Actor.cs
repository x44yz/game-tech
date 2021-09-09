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

        public float hunger;
        public float fatigue;
        public float hungryRate;
        public float fatigueRate;

        void Awake()
        {
            EatState eat = new EatState(this);
            SleepState sleep = new SleepState(this);
            WorkState work = new WorkState(this);
            IdleState idle = new IdleState(this);
            WalkState walk = new WalkState(this);
        
            fsm = new StateMachine();
            fsm.AddTransition(new Transition(eat, sleep, OnEatToSleepCond));
            fsm.AddTransition(new Transition(sleep, eat, OnSleepToEatCond));

            // set default
            fsm.SetState(idle);
        }

        void Update()
        {
            fsm.Update(Time.deltaTime);

            float dt = Time.deltaTime;
            
            hunger += hungryRate * dt;
            hunger = Mathf.Max(hunger, 0);

            fatigue += fatigueRate * dt;
            fatigue = Mathf.Max(fatigue, 0);
        }

        bool OnEatToSleepCond()
        {
            return hunger > 0 && fatigue > 1;
        }

        bool OnEatToWorkCond()
        {
            return false;
        }

        bool OnSleepToEatCond()
        {
            return hunger > 0 && fatigue == 0;
        }

        bool OnWorkToSleepCond()
        {
            return false;
        }

        bool OnWorkToEatCond()
        {
            return false;
        }
    }
}

