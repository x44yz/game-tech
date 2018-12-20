using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	public bool isActive = false;

	public override void Tick(float dt)
	{
	}

	public override bool CanAttack()
	{
		return false;
	}

	public override void Selected(IAbilityCaster caster, Ability ability)
	{
		Debug.Log("Enemy selected");
		Debug.Assert(ability != null, "CHECK");
		gameObject.GetComponent<SpriteRenderer>().color = Color.red;

		ability.Apply(caster, this);
	}

	public override void ApplyEffect(Effect effect)
	{
	}

	public override void TakeDamage(int damage)
	{
	}
}
