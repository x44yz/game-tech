using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Book
{
    public class LeaveComp : MonoBehaviour
    {
        public Transform target;
        public float escapeRadius;
        public float dangerRadius;
        public float timeToTargetSpeed; // 变化到目标速度的快慢

        [Header("RUNTIME")]
        public AIAgent agent;

        [Header("DEBUG")]
        public Color escapeRadiusColor = Color.blue;
        public Color dangerRadiusColor = Color.red;

        private void Awake() 
        {
            agent = GetComponent<AIAgent>();
        }

        void Update()
        {
            Vector3 distVec = Utils.Vector3ZeroY(agent.pos - target.position);
            float dist = distVec.magnitude;

            if (dist > dangerRadius)
            {
                agent.accel = Vector3.zero;
                return;
            }
            
            float reduce;
            if (dist < escapeRadius)
            {
                reduce = 0f;
            }
            else
            {
                reduce = agent.maxMoveSpeed * (dist / dangerRadius);
            }
            float targetSpeed = agent.maxMoveSpeed - reduce;

            Vector3 targetVelocity = distVec.normalized * targetSpeed;
            // 因为这边目标速度与当前速度计算的加速度缺少时间参数，所以自己设定时间来确定变化快慢
            Vector3 accel = targetVelocity - agent.velocity;
            accel /= timeToTargetSpeed;

            agent.accel = accel;
        }

        void OnDrawGizmos()
        {
            Handles.color = escapeRadiusColor;
            Handles.DrawWireDisc(target.position, Vector3.up, escapeRadius);
        
            Handles.color = dangerRadiusColor;
            Handles.DrawWireDisc(target.position, Vector3.up, dangerRadius);
        }
    }
}
