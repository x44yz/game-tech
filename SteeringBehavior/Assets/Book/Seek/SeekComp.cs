using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Book
{
    public class SeekComp : MonoBehaviour
    {
        public Transform target;

        [Header("RUNTIME")]
        public AIAgent agent;

        // [Header("DEBUG")]

        private void Awake() 
        {
            agent = GetComponent<AIAgent>();
        }

        void Update()
        {
            Vector3 dist = (target.position - agent.pos).ZeroY();
            Vector3 accel = dist.normalized * agent.maxAccel;

            agent.accel = accel;
        }

        void OnDrawGizmos()
        {

        }
    }
}