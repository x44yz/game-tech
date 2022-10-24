using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class Arrive : MonoBehaviour
    {
        public Transform target;
        public float slowRadius;
        public float stopRadius;

        [Header("RUNTIME")]
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        [Header("DEBUG")]
        public Color slowRadiusColor;
        public Color stopRadiusColor;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Vector3 dir = Utils.Vector3ZeroY(target.position - agent.pos);
            var desiredVelocity = Vector3.zero;

            float dist = dir.magnitude;
            if (dist <= stopRadius)
            {
                agent.velocity = Vector3.zero;
                steeringVal = 0f;
                accelVal = 0f;
                velocityVal = 0f;
                return;
            }

            if (dist < slowRadius)
                desiredVelocity = dir.normalized * agent.maxSpeed * (dist / slowRadius);
            else
                desiredVelocity = dir.normalized * agent.maxSpeed;

            var steering = desiredVelocity - agent.velocity;
            steering = Utils.Vector3Truncate(steering, agent.maxForce);
            
            var accel = steering / agent.mass;
            agent.velocity = agent.velocity + accel * dt;
            agent.velocity = Utils.Vector3Truncate(agent.velocity, agent.maxSpeed);

            agent.pos = agent.pos + agent.velocity * dt;

            // debug track
            steeringVal = steering.magnitude;
            accelVal = accel.magnitude;
            velocityVal = agent.velocity.magnitude;
        }

        private void OnDrawGizmos()
        {
            if (target != null)
            {
                Handles.color = slowRadiusColor;
                Handles.DrawWireDisc(target.position, Vector3.up, slowRadius);
            
                Handles.color = stopRadiusColor;
                Handles.DrawSolidDisc(target.position, Vector3.up, stopRadius);
            }
        }
    }
}

