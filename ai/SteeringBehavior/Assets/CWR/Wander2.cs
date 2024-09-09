using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    // https://www.jianshu.com/p/f5979985b5dd
    public class Wander2 : MonoBehaviour
    {
        public float circleDist;
        public float circleRadius;
        public float wanderJitter;

        [Header("RUNTIME")]
        public Agent agent;
        public Vector3 wanderTarget;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        [Header("DEBUG")]
        public Color circleColor;

        void Start()
        {
            agent = GetComponent<Agent>();
            float theta = Random.value * 2 * Mathf.PI;
            wanderTarget = new Vector3(circleRadius * Mathf.Cos(theta), 0f, circleRadius * Mathf.Sin(theta));
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            float jitter = wanderJitter * dt;
            wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, 0f, Random.Range(-1f, 1f) * jitter);
            wanderTarget = wanderTarget.normalized * circleRadius;

            var targetPos = agent.pos + agent.velocity.normalized * circleDist + wanderTarget;
            Vector3 dir = (targetPos - agent.pos).ZeroY();
            var desiredVelocity = dir.normalized * agent.maxSpeed;
            var steering = desiredVelocity - agent.velocity;
            steering = steering.Truncate(agent.maxForce);
            
            // var steering = circleCenter + displacement;
            // Debug.DrawLine(circleCenter, circleCenter + steering, Color.blue, 0.1f);
            
            var accel = steering / agent.mass;
            agent.velocity = agent.velocity + accel * dt;
            agent.velocity = agent.velocity.Truncate(agent.maxSpeed);

            agent.pos = agent.pos + agent.velocity * dt;

            // debug track
            steeringVal = steering.magnitude;
            accelVal = accel.magnitude;
            velocityVal = agent.velocity.magnitude;
        }

        private void OnDrawGizmos()
        {
            if (agent != null)
            {
                Vector3 circleCenter = agent.pos + agent.velocity.normalized * circleDist;
                Handles.color = circleColor;
                Handles.DrawLine(agent.pos, circleCenter);
                Handles.DrawWireDisc(circleCenter, Vector3.up, circleRadius);
            }
        }
    }
}

