using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Freeze 时间/回合制
// Freeze 禁止 Action，并不能阻止其他 Effect 起作用
public class Freeze : Ability
{
    public override void Apply(IAbilityCaster caster, IAbilityTarget target)
    {
        base.Apply(caster, target);

        Actor aCaster = caster as Actor;
        Debug.Assert(aCaster != null, "CHECK");

		Actor aTarget = target as Actor;
		Debug.Assert(aTarget != null, "CHECK");

		// create animation
		

		// damage may be delay after animation
		// compute damange by caster level or ability level
		int damage = 1;
		ApplyDamage(caster, target, damage);

		// create effect
		Effect effect = new Effect();
		effect.round = 3;
		effect.isActionForbid = true;
		target.ApplyEffect(effect);
	}
}
