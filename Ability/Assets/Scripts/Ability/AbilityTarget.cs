using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTarget
{
    bool CanAttack();

    void Selected(IAbilityCaster caster, Ability ability);
    void ApplyEffect(Effect effect);
	void TakeDamage(int damage);
}
