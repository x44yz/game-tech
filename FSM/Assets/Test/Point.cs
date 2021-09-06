using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public enum PointType
    {
        Eat,
        Sleep,
        Work,
    }

    public class Point : MonoBehaviour
    {
        public PointType pointType;
        public Transform modelTF;
        public TextMesh nameBoard;

        private void Awake()
        {
            nameBoard.text = pointType.ToString();
        }
    }
}