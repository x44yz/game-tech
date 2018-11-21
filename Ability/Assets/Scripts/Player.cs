using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. choose ability
// 2. choose target
public class Player : Actor 
{
    public Ability activeAbility = null;

    private void Start()
    {
        activeAbility = new Freeze();
    }

    private void Update()
    {
    }

    public void Selected()
    {
        Debug.Log("Player selected");
    }
}
