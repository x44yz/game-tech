using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWR
{
    public class Agent : MonoBehaviour
    {
        public float maxSpeed = 4;
        public float maxForce = 1;
        public float mass = 1;

        [Header("RUNTIME")]
        public Vector3 velocity;

        public Vector3 pos
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }
}
