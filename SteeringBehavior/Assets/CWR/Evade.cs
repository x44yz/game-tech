using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class Evade : MonoBehaviour
    {
        public Transform target;

        [Header("RUNTIME")]
        public Agent targetAgent;
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        [Header("DEBUG")]
        public float futureLineDuration = 0.1f;

        void Start()
        {
            agent = GetComponent<Agent>();
            targetAgent = target.GetComponent<Agent>();
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Vector3 curDir = Utils.Vector3ZeroY(target.position - agent.pos);
            float t = curDir.magnitude / agent.maxSpeed;
            Vector3 futurePos = targetAgent.pos + targetAgent.velocity * t;
            Debug.DrawLine(agent.pos, futurePos, Color.red, futureLineDuration);

            // Flee
            Vector3 dir = Utils.Vector3ZeroY(futurePos - agent.pos) * -1;
            var desiredVelocity = dir.normalized * agent.maxSpeed;
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
        }
    }
}

