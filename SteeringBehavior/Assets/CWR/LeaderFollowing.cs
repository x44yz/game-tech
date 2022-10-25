using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    // 如果使用 stop radius 就是急停的效果
    public class LeaderFollowing : MonoBehaviour
    {
        public Transform target;
        public float slowRadius;
        public bool useStopRadius;
        public float stopRadius;
        public float leaderBehindDist;

        [Header("RUNTIME")]
        public Agent agent;
        public Agent targetAgent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;

        [Header("DEBUG")]
        public Color slowRadiusColor;
        public Color stopRadiusColor;

        void Start()
        {
            agent = GetComponent<Agent>();
            targetAgent = target.GetComponent<Agent>();
        }

        private Vector3 Arrival(Vector3 targetPos)
        {
            Vector3 dir = (targetPos - agent.pos).ZeroY();
            var desiredVelocity = Vector3.zero;

            float dist = dir.magnitude;
            if (useStopRadius && dist <= stopRadius)
            {
                agent.velocity = Vector3.zero;
                steeringVal = 0f;
                accelVal = 0f;
                velocityVal = 0f;
                return Vector3.zero;
            }

            if (dist < slowRadius)
                desiredVelocity = dir.normalized * agent.maxSpeed * (dist / slowRadius);
            else
                desiredVelocity = dir.normalized * agent.maxSpeed;

            var steering = desiredVelocity - agent.velocity;
            return steering;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            var behindPos = targetAgent.pos + (targetAgent.velocity * -1).normalized * leaderBehindDist;

            var steering = Arrival(behindPos);
            steering = steering.Truncate(agent.maxForce);
            
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
            if (targetAgent != null)
            {
                var behindPos = targetAgent.pos + (targetAgent.velocity * -1).normalized * leaderBehindDist;

                Handles.color = slowRadiusColor;
                Handles.DrawWireDisc(behindPos, Vector3.up, slowRadius);
            
                Handles.color = stopRadiusColor;
                Handles.DrawSolidDisc(behindPos, Vector3.up, stopRadius);
            }
        }
    }
}

