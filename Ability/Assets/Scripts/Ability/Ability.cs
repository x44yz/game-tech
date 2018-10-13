using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability 
{
    public AbilityType type;
    public bool bActive;
    public int cooldownMSec;

    public virtual void Tick(float dt)
    {
    }

    public virtual void OnActivate()
    {
        // create target selector
    }

    public virtual void OnDeactivate()
    {
    }
       
    public virtual void OnTargetSelect()
    {
    }

}
