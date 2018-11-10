using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. choose ability
// 2. choose target
public class Player : MonoBehaviour 
{
    public Ability activeAbility = null;

    private void Update()
    {
//        if (Input.GetKeyDown(KeyCode.W))
//        {  
//        }
//
//        if (Input.GetMouseButtonDown(0))
//        {       
//        }
    }

    public void Selected()
    {
        Debug.Log("Player selected");
    }
}
