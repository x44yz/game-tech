using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor, IAbilityTarget
{
    public bool isActive = false;

    private void Update()
    {
        
    }

    public bool CanAttack()
    {
        return false;
    }

    public void Selected(Ability ability)
    {
        // Debug.Log("Enemy selected");
        Debug.Assert(ability != null, "CHECK");

 
    }

    public void ApplyEffect(Effect effect)
    {
    }
}
