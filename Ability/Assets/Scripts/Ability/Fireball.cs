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
            targeter.radius = 5;
            targeter.Activate();
        }
    }

    public override void OnDeactivate()
    {
        
    }

    public override void OnTargetSelect()
    {
        // targeter.

    }

    private void PerformFireball()
    {
    }
}
