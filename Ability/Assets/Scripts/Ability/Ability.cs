using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability 
{
    public AbilityType type;
    public bool isActive;
    public int cooldownMSec;

    public IAbilityCaster caster;
    public IAbilityTarget target;

    public void Tick(float dt)
    {
    }

    public void Activate(Actor caster)
    {
        OnActivate(caster);
    }

    public void Deactivate()
    {
    }

    public virtual void Apply(IAbilityCaster caster, IAbilityTarget target)
    {
        this.caster = caster;
        this.target = target;
    }

    public void ApplyDamage(IAbilityCaster caster, IAbilityTarget target, int damage)
    {
        // determine damage mutiplier from caster
        int damageMult = 1;
        damage = (damage * (100 + damageMult)) / 100;

        // determine spell resistance factor
        // TODO

        target.TakeDamage(damage);
    }

    public void ApplyHealing(Actor caster, Actor target, int healing)
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
