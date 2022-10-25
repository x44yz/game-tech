using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWR
{
    // https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-queue--gamedev-14365
    public class Queuing : MonoBehaviour
    {
        public Transform door;
        public float avoidAhead;
        public float maxAvoidForce;
        public float queueAhead;
        public float queueRadius;
        
        [Header("RUNTIME")]
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;
        public Agent[] collisionAgents;

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
            var ahead = agent.pos + agent.velocity.normalized * avoidAhead;
            var ahead2 = agent.pos + agent.velocity.normalized * avoidAhead * 0.5f;

            // find most threatening obstacle
            Agent obstacle = null;
            float obstacleDist = float.MaxValue;
            foreach (var a in collisionAgents)
            {
                if (a == agent)
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

        private Agent GetNeighborAhead()
        {
            var ahead = agent.pos + agent.velocity.normalized * queueAhead;

            Agent neighbor = null;
            foreach (var a in collisionAgents)
            {
                if (a == null || a == agent)
                    continue;
                
                float dist = (ahead - a.pos).ZeroYLength();
                if (dist <= queueRadius)
                {
                    neighbor = a;
                    break;
                }
            }

            return neighbor;
        }

        private Vector3 Queue()
        {
            Vector3 bake = Vector3.zero;

            var neighbor = GetNeighborAhead();
            if (neighbor != null)
            {
                // 当离得太近的，硬停止
                var dist = (agent.pos - neighbor.pos).ZeroYLength();
                if (dist <= queueRadius)
                    agent.velocity *= 0.3f;
            }

            return bake;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            var steering = Seek(door.position);
            steering += Avoidance();
            steering += Queue();
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
    }
}

