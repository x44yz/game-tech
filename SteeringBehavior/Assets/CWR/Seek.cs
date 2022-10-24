using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWR
{
    public class Seek : MonoBehaviour
    {
        public Transform target;
        [Header("RUNTIME")]
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Vector3 dir = Utils.Vector3ZeroY(target.position - agent.pos);
            var desiredVelocity = dir.normalized * agent.maxSpeed;
            var steering = desiredVelocity - agent.velocity;
            if (steering.magnitude > agent.maxForce)
                steering = steering.normalized * agent.maxForce;
            
            var accel = steering / agent.mass;
            agent.velocity = agent.velocity + accel * dt;
            if (agent.velocity.magnitude > agent.maxSpeed)
                agent.velocity = agent.velocity.normalized * agent.maxSpeed;

            agent.pos = agent.pos + agent.velocity * dt;

            // debug track
            steeringVal = steering.magnitude;
            accelVal = accel.magnitude;
            velocityVal = agent.velocity.magnitude;
        }
    }
}

