using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// freeze 时间/回合制
public class Freeze : Ability
{
    public override void Apply(IAbilityCaster caster, IAbilityTarget target)
    {
        base.Apply(caster, target);

        Actor aCaster = caster as Actor;
        Debug.Assert(aCaster == null, "CHECK");

		// create effect
		// damage may be delay after effect
		// compute damange by caster level or ability level
		int damage = 1;
		ApplyDamage(caster, target, damage);
	}
}
