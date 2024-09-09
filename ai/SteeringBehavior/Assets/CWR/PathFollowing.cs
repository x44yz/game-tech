using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class PathFollowing : MonoBehaviour
    {
        public Path path;
        public float nodeValidRadius = 1f;

        [Header("RUNTIME")]
        public Agent agent;
        public float steeringVal;
        public float accelVal;
        public float velocityVal;
        public int curPathNodeIdx;

        [Header("DEBUG")]
        public Color nodeValidRadiusColor = Color.green;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        private Vector3 Seek(Vector3 targetPos)
        {
            Vector3 dir = (targetPos - agent.pos).ZeroY();
            var desiredVelocity = dir.normalized * agent.maxSpeed;
            var steering = desiredVelocity - agent.velocity;
            return steering;
        }

        private Transform PathNode()
        {
            if (path == null || path.nodes == null || path.nodes.Length == 0)
                return null;
            if (curPathNodeIdx < 0)
                curPathNodeIdx = 0;
            if (curPathNodeIdx >= path.nodes.Length)
                curPathNodeIdx = path.nodes.Length - 1;

            var node = path.nodes[curPathNodeIdx];
            var dist = (node.position - agent.pos).ZeroYLength();
            if (dist <= nodeValidRadius)
            {
                curPathNodeIdx += 1;
            }
            return node;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Transform pathNode = PathNode();
            if (pathNode == null)
                return;

            var steering = Seek(pathNode.position);
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
            if (path != null && path.nodes != null)
            {
                Handles.color = nodeValidRadiusColor;
                for (int i = 0; i < path.nodes.Length; ++i)
                {
                    Handles.DrawWireDisc(path.nodes[i].position, Vector3.up, nodeValidRadius);
                }
            }
        }
    }
}

