using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.FSM;

namespace Test
{


    public class Actor : MonoBehaviour
    {
        public const float POINT_STOP_DIST = 1f;

        public List<Point> points = new List<Point>();

        private StateMachine fsm;

        public float hunger;
        public float fatigue;
        public float hungryRate;
        public float fatigueRate;
        public float walkSpeed;

        [Header("Runtime")]
        public PointType targetPT = PointType.None;
        public string curState = "";

        void Awake()
        {
            EatState eat = new EatState(this);
            SleepState sleep = new SleepState(this);
            WorkState work = new WorkState(this);
            IdleState idle = new IdleState(this);
            WalkState walk = new WalkState(this);
        
            fsm = new StateMachine();
            // fsm.AddTransition(new Transition(eat, sleep, OnEatToSleepCond));
            // fsm.AddTransition(new Transition(sleep, eat, OnSleepToEatCond));
            fsm.AddTransition(new Transition(idle, eat, OnIdleToEatCond));
            // fsm.AddTransition(new Transition(idle, sleep, OnIdleToSleepCond));
            fsm.AddTransition(new Transition(eat, walk, OnXXXToWalkCond));
            fsm.AddTransition(new Transition(walk, idle, OnWalkToIdleCond));

            // set default
            fsm.SetState(idle);
        }

        void Update()
        {
            fsm.Update(Time.deltaTime);

            float dt = Time.deltaTime;
            
            hunger += hungryRate * dt;
            hunger = Mathf.Max(hunger, 0);

            // fatigue += fatigueRate * dt;
            // fatigue = Mathf.Max(fatigue, 0);

            curState = fsm.curState.ToString();
        }

        public Point GetPoint(PointType pt)
        {
            foreach (var p in points)
            {
                if (p.pointType == pt)
                    return p;
            }
            Debug.Assert(false, "Cant find point type > " + pt);
            return null;
        }

        bool OnIdleToEatCond()
        {
            return hunger < 1f;
        }

        bool OnIdleToSleepCond()
        {
            return hunger < 1f;
        }

        bool OnXXXToWalkCond()
        {
            if (targetPT == PointType.None)
                return false;

            var pt = GetPoint(targetPT);
            var dist = transform.position - pt.transform.position;
            dist.y = 0f;
            return dist.magnitude > POINT_STOP_DIST;
        }

        bool OnWalkToIdleCond()
        {
            var pt = GetPoint(targetPT);
            var dist = transform.position - pt.transform.position;
            dist.y = 0f;
            return dist.magnitude <= POINT_STOP_DIST;
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

