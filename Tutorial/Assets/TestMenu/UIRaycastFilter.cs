using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class UIRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool enable;
        public RectTransform target;

        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
        {
            if (!enable)
                return true;

            if (null == target)
                return true;
            return !RectTransformUtility.RectangleContainsScreenPoint(target, screenPos, eventCamera);
        }
    }
}
