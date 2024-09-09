using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTeleportComp : MonoBehaviour
{
    public LayerMask collideLayer;

    [Header("DEBUG")]
    public bool showRayLine;
    public Color raylineColor = Color.blue;
    public float raylineDuration = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, collideLayer.value))
            {
                transform.position = hit.point;
                if (showRayLine)
                    Debug.DrawLine(Camera.main.gameObject.transform.position, hit.point, raylineColor, raylineDuration);
            }
        }
    }
}
