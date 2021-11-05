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
        public float energy;
        public float walkSpeed;
        public float defaultHungryDrain;
        public float walkEnergyDrain;
        public float eatHungrySupply;
        public float workHungryDrain;
        public float workEnergyDrain;
        public float idleEnergySupply;
        public float sleepEnergySupply;
        public float sleepHungrySupplyMulti;

        [Header("Runtime")]
        public PointType targetPT = PointType.None;
        public string curState = "";
        public float hungryRate
        {
            get { return _hungryRate; }
            set 
            {
                Debug.Log("xx-- hungryRate > " + _hungryRate + " - " + value);
                _hungryRate = value;
            }
        }
        [SerializeField]
        private float _hungryRate = 0f;

        public float energyRate 
        {
            get { return _energyRate; }
            set 
            {
                Debug.Log("xx-- energyRate > " + _energyRate + " - " + value);
                _energyRate = value;
            }
        }
        [SerializeField]
        private float _energyRate = 0f;

        void Awake()
        {
            hungryRate += defaultHungryDrain;
            energyRate = 0f;

            EatState eat = new EatState(this);
            SleepState sleep = new SleepState(this);
            WorkState work = new WorkState(this);
            IdleState idle = new IdleState(this);
            WalkState walk = new WalkState(this);
        
            fsm = new StateMachine();
            fsm.AddTransition(new Transition(idle, eat, idle.IsTranslateToEat));
            fsm.AddTransition(new Transition(idle, sleep, idle.IsTranslateToSleep));
            fsm.AddTransition(new Transition(idle, work, idle.IsTranslateToWork));
            fsm.AddTransition(new Transition(idle, walk, idle.IsTranslateToWalk));

            fsm.AddTransition(new Transition(walk, idle, OnWalkToIdleCond));

            fsm.AddTransition(new Transition(eat, idle, OnEatToIdleCond));

            fsm.AddTransition(new Transition(work, idle, OnWorkToIdleCond));
            fsm.AddTransition(new Transition(work, walk, OnXXXToWalkCond));

            fsm.AddTransition(new Transition(sleep, idle, OnSleepToIdleCond));
            fsm.AddTransition(new Transition(sleep, walk, OnXXXToWalkCond));

            // set default
            fsm.SetState(idle);
        }

        void Update()
        {
            fsm.Update(Time.deltaTime);

            float dt = Time.deltaTime;
            
            hunger += hungryRate * dt;
            hunger = Mathf.Max(hunger, 0);

            energy += energyRate * dt;
            energy = Mathf.Max(energy, 0);

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

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnIdleToEatCond()
        // {
        //     return hunger < 0.1f;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnEatToIdleCond()
        // {
        //     return hunger >= 20;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnIdleToWorkCond()
        // {
        //     return hunger > 5f && energy > 5f;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnWorkToIdleCond()
        // {
        //     return hunger < 0.1f || energy < 0.5f;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnIdleToSleepCond()
        // {
        //     return energy < 1f;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnSleepToIdleCond()
        // {
        //     return energy > 10f;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnXXXToWalkCond()
        // {
        //     if (targetPT == PointType.None)
        //         return false;

        //     var pt = GetPoint(targetPT);
        //     var dist = transform.position - pt.transform.position;
        //     dist.y = 0f;
        //     return dist.magnitude > POINT_STOP_DIST;
        // }

        // [FSMAttrTransitionMethod("ActorFSM")]
        // bool OnWalkToIdleCond()
        // {
        //     var pt = GetPoint(targetPT);
        //     var dist = transform.position - pt.transform.position;
        //     dist.y = 0f;
        //     return dist.magnitude <= POINT_STOP_DIST;
        // }
    }
}

