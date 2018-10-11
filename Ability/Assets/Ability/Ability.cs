using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability 
{
    public AbilityType type;
    public bool bActive;
    public int cooldownMSec;

    public void Tick(float dt)
    {
    }

    public void ApplyEffectToOwner(IAbilityTarget target)
    {
    }
}
