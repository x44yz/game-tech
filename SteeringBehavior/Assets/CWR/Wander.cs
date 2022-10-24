using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class Wander : MonoBehaviour
    {
        public float circleDist;
        public float circleRadius;
        public float angleChange;

        [Header("RUNTIME")]
        public Agent agent;
        public float wanderAngle;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        [Header("DEBUG")]
        public Color circleColor;

        void Start()
        {
            agent = GetComponent<Agent>();
            wanderAngle = transform.rotation.eulerAngles.y;
            agent.velocity = transform.forward;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Vector3 circleCenter = agent.pos + agent.velocity.normalized * circleDist;
            // Vector3 displacement = Quaternion.Euler(0f, wanderAngle, 0f) * (Vector3.forward * circleRadius);
            // wanderAngle += Random.Range(0f, 1f) * angleChange - 0.5f * angleChange;
            
            float randAngle = Random.Range(0f, 1f) * angleChange - 0.5f * angleChange;
            // Vector3 displacement = Quaternion.Euler(0f, randAngle, 0f) * agent.velocity.normalized * circleRadius;
            Vector3 displacement = Quaternion.AngleAxis(randAngle, Vector3.up) * (Vector3.right * circleRadius);
            Debug.DrawLine(circleCenter, circleCenter + displacement, Color.green, 0.1f);

            // var targetPos = circleCenter + displacement;
            // Vector3 dir = Utils.Vector3ZeroY(targetPos - agent.pos);
            // var desiredVelocity = dir.normalized * agent.maxSpeed;
            // var steering = desiredVelocity - agent.velocity;
            // steering = Utils.Vector3Truncate(steering, agent.maxForce);
            
            var steering = circleCenter + displacement;
            Debug.DrawLine(circleCenter, circleCenter + steering, Color.blue, 0.1f);
            
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

