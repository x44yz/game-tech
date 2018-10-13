using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    private AbilityCircleTargeter targeter = null;

    public override void OnActivate(Actor parent)
    {
        if (targeter == null)
        {
            targeter = new AbilityCircleTargeter();
        }
    }

    public override void OnDeactivate()
    {
        
    }
}
