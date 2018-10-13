using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability 
{
    public AbilityType type;
    public bool isActive;
    public int cooldownMSec;

    public void Tick(float dt)
    {
    }

    public void Activate(Actor parent)
    {
        OnActivate(parent);
    }

    public void Deactivate()
    {
    }
     
    public virtual void OnActivate(Actor parent)
    {
    }

    public virtual void OnDeactivate()
    {
    }

    public virtual void OnTargetSelect()
    {
    }
}
