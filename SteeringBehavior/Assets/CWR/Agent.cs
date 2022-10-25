using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWR
{
    public class Agent : MonoBehaviour
    {
        public float maxSpeed = 4;
        public float maxForce = 1;
        public float mass = 1;
        public float collisionRadius = 1f;

        [Header("RUNTIME")]
        public Vector3 velocity;

        [Header("DEBUG")]
        public Color collisionColor = Color.red;

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

        private void OnDrawGizmos()
        {
            Handles.color = collisionColor;
            Handles.DrawWireDisc(transform.position, Vector3.up, collisionRadius);
        }
    }
}
