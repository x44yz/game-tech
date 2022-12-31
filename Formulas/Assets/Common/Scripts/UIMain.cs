using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Image imgLeftMouse;
    public Image imgRightMouse;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            imgLeftMouse.color = myColor;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            imgRightMouse.color = myColor;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            imgLeftMouse.color = myColor;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            imgRightMouse.color = myColor;
        }
    }
}
