using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public enum PointType
    {
        None = -1,
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