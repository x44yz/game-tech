using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOTest : MonoBehaviour
{
    public Unit[] units;

    // Start is called before the first frame update
    void Start()
    {
        units = GameObject.FindObjectsOfType<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Ground")))
            {
                Debug.DrawLine(Camera.main.gameObject.transform.position, hit.point, Color.green, 4f);
            
                foreach (var ut in units)
                {
                    ut.MoveToTarget(hit.point);
                }
            }
        }
    }
}
