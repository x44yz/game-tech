using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Book
{
    public class WallAvoidanceComp : MonoBehaviour
    {
        public Vector3 checkOffset = new Vector3(0f, 1f, 0f);
        public float headLength = 4f;
        public float sideLength = 2f;
        public float sideAngle = 45f;
        public LayerMask avoidanceLayer;
        public float wallAvoidDistance = 0.5f;
        public float maxAccel = 1f;

        [Header("RUNTIME")]
        public AIAgent agent;

        [Header("DEBUG")]
        public Color headColor = Color.blue;
        public Color sideColor = Color.green;

        private void Awake() 
        {
            agent = GetComponent<AIAgent>();
            agent.velocity = agent.forward * agent.maxMoveSpeed;
        }

        private void Update()
        {
            var checkStartPos = agent.pos + checkOffset;

            Vector3 accel = Vector3.zero;
            // check forward
            RaycastHit hit;
            if (Physics.Raycast(checkStartPos, agent.forward, out hit, headLength, avoidanceLayer.value) == false)
            {
                if (Physics.Raycast(checkStartPos, Quaternion.Euler(0f, sideAngle, 0f) * agent.forward, out hit, sideLength, avoidanceLayer.value) == false)
                {
                    Physics.Raycast(checkStartPos, Quaternion.Euler(0f, -sideAngle, 0f) * agent.forward, out hit, sideLength, avoidanceLayer.value);
                }
            }

            if (hit.collider != null)
            {
                Vector3 targetPos = hit.point + hit.normal * wallAvoidDistance;
                // 当速度方向与墙碰撞法线平行的时候，在法线左右偏移一定距离
                float angle = Vector3.Angle(agent.velocity, hit.normal);
                Debug.Log("xx-- hit angle > " + angle);
                // 165f 就看作平行，该值可以根据测试调整
                if (angle > 165f)
                {
                    // Vector3 perp = new Vector3(-hit.normal.z, hit.normal.y, hit.normal.x);
                    // Debug.DrawLine(hit.point, hit.point + perp * Mathf.Sin((angle - 165f) * Mathf.Deg2Rad) * 2f, Color.red, 10f);
                    // targetPos = targetPos + (perp * Mathf.Sin((angle - 165f) * Mathf.Deg2Rad) * 2f * wallAvoidDistance);
                    targetPos = hit.point + Quaternion.Euler(0f, 90f - angle, 0f) * hit.normal * wallAvoidDistance;
                }

                accel = (targetPos - agent.pos).ZeroY().normalized * maxAccel;
            }
            else
            {
                accel = Vector3.zero;
            }

            // Vector3 targetVelocity = distVec.normalized * targetSpeed;
            // // 因为这边目标速度与当前速度计算的加速度缺少时间参数，所以自己设定时间来确定变化快慢
            // Vector3 accel = targetVelocity - agent.velocity;
            // accel *= 1f / timeToTargetSpeed;

            agent.accel = accel;
            agent.velocity += accel * Time.deltaTime;
            if (agent.velocity.magnitude > agent.maxMoveSpeed)
            {
                agent.velocity = agent.velocity.normalized * agent.maxMoveSpeed;
            }
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = headColor;
            Vector3 startPos = transform.position + checkOffset;
            Gizmos.DrawLine(startPos, startPos + transform.forward * headLength);

            Gizmos.color = sideColor;
            Gizmos.DrawLine(startPos, startPos + Quaternion.Euler(0f, sideAngle, 0f) * transform.forward * sideLength);
            Gizmos.DrawLine(startPos, startPos + Quaternion.Euler(0f, -sideAngle, 0f) * transform.forward * sideLength);
        }
    }
}
