using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Book
{
    public class ArriveComp : MonoBehaviour
    {
        public Transform target;
        public float slowDownRadius;
        public float timeToTargetSpeed; // 变化到目标速度的快慢
        public float targetStopRadius;

        [Header("RUNTIME")]
        public AIAgent agent;

        [Header("DEBUG")]
        public Color slowDownColor = Color.blue;
        public Color targetStopColor = Color.red;

        private void Awake() 
        {
            agent = GetComponent<AIAgent>();
        }

        void Update()
        {
            Vector3 distVec = (target.position - agent.pos).ZeroY();
            float dist = distVec.magnitude;

            if (dist < targetStopRadius)
            {
                agent.accel = Vector3.zero;
                return;
            }
            
            float targetSpeed;
            if (dist > slowDownRadius)
            {
                targetSpeed = agent.maxMoveSpeed;
            }
            else
            {
                targetSpeed = agent.maxMoveSpeed * (dist / slowDownRadius);
            }

            Vector3 targetVelocity = distVec.normalized * targetSpeed;
            // 因为这边目标速度与当前速度计算的加速度缺少时间参数，所以自己设定时间来确定变化快慢
            Vector3 accel = targetVelocity - agent.velocity;
            accel /= timeToTargetSpeed;

            agent.accel = accel;
        }

        void OnDrawGizmos()
        {
            Handles.color = slowDownColor;
            Handles.DrawWireDisc(target.position, Vector3.up, slowDownRadius);
        
            Handles.color = targetStopColor;
            Handles.DrawWireDisc(target.position, Vector3.up, targetStopRadius);
        }
    }
}