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

        private Agent FindMostThreateningObstacle()
        {
            var ahead = agent.pos + agent.velocity.normalized * aheadLength;
            var ahead2 = agent.pos + agent.velocity.normalized * aheadLength * 0.5f;

            Agent obstacle = null;
            float obstacleDist = float.MaxValue;
            foreach (var a in collisionAgents)
            {
                if (a == agent)
                    continue;
                if (a == target)
                    continue;
                
                float radiusSqr = a.collisionRadius * a.collisionRadius;
                float dist = (ahead - a.pos).ZeroYSqrLength();
                if (dist >= radiusSqr)
                {
                    // 如果和 head 的中段发生碰撞
                    dist = (ahead2 - a.pos).ZeroYSqrLength();
                    if (dist >= radiusSqr)
                    {
                        // 如果两个 agent 靠得太近
                        dist = (agent.pos - a.pos).ZeroYSqrLength();
                        if (dist >= radiusSqr)
                            continue;
                    }
                }

                dist = (agent.pos - a.pos).ZeroYSqrLength();
                if (obstacle == null || dist < obstacleDist)
                {
                    obstacle = a;
                    obstacleDist = dist;
                }
            }

            return obstacle;
        }

        private Vector3 CollisionAvoidance()
        {
            var obstacle = FindMostThreateningObstacle();
            if (obstacle == null)
                return Vector3.zero;

            var ahead = agent.pos + agent.velocity.normalized * aheadLength;
            var avoidance = (ahead - obstacle.pos).ZeroY();
            avoidance = avoidance.normalized * maxAvoidForce;
            Debug.DrawLine(obstacle.pos, obstacle.pos + avoidance, Color.red, 0.1f);
            return avoidance;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            var steering = Seek(target.position);
            steering += CollisionAvoidance();

            // 可以添加最大转向力
            // steering = steering.Truncate(agent.maxForce);
            
            var accel = steering / agent.mass;
            agent.velocity = agent.velocity + accel;
            // 限制最大速度
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
                Gizmos.color = aheadColor;
                Gizmos.DrawLine(agent.pos, agent.pos + agent.velocity.normalized * aheadLength);
            }
        }
    }
}

