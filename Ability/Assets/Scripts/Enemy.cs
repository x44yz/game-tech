using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IAbilityTarget
{
    private void Update()
    {
        
    }

    public void Selected()
    {
        Debug.Log("Enemy selected");
    }
}
