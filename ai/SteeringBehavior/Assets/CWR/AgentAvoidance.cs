using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class AgentAvoidance : MonoBehaviour
    {
        public Transform target;
        public float aheadLength;
        public float maxAvoidForce;

        [Header("RUNTIME")]
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;
        public Agent[] collisionAgents;

        [Header("DEBUG")]
        public Color aheadColor;

        void Start()
        {
            agent = GetComponent<Agent>();
            collisionAgents = GameObject.FindObjectsOfType<Agent>();
        }

        private Vector3 Seek(Vector3 targetPos)
        {
            Vector3 dir = (targetPos - agent.pos).ZeroY();
            var desiredVelocity = dir.normalized * agent.maxSpeed;
            var steering = desiredVelocity - agent.velocity;
            return steering;
        }

        private Vector3 Avoidance()
        {
            var ahead = agent.pos + agent.velocity.normalized * aheadLength;
            var ahead2 = agent.pos + agent.velocity.normalized * aheadLength * 0.5f;

            // find most threatening obstacle
            Agent obstacle = null;
            float obstacleDist = float.MaxValue;
            foreach (var a in collisionAgents)
            {
                if (a == agent)
                    continue;
                if (a == target)
                    continue;
                
                float dist = (ahead - a.pos).ZeroYLength();
                if (dist > a.collisionRadius + agent.collisionRadius)
                {
                    dist = (ahead2 - a.pos).ZeroYLength();
                    if (dist > a.collisionRadius + agent.collisionRadius)
                        continue;
                }

                dist = (agent.pos - a.pos).ZeroYLength();
                if (obstacle == null || dist < obstacleDist)
                {
                    obstacle = a;
                    obstacleDist = dist;
                }
            }

            if (obstacle == null)
                return Vector3.zero;

            var avoidance = (ahead - obstacle.pos).ZeroY();
            avoidance = avoidance.normalized * maxAvoidForce;
            Debug.DrawLine(obstacle.pos, obstacle.pos + avoidance, Color.red, 0.1f);
            return avoidance;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            var steering = Seek(target.position);
            steering += Avoidance();

            // steering = steering.Truncate(agent.maxForce);
            
            var accel = steering / agent.mass;
            agent.velocity = agent.velocity + accel * dt;
            // agent.velocity = agent.velocity.Truncate(agent.maxSpeed);

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
                Gizmos.color = aheadColor;
                Gizmos.DrawLine(agent.pos, agent.pos + agent.velocity.normalized * aheadLength);
            }
        }
    }
}

