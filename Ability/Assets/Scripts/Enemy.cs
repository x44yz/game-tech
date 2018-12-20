using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
  // public bool isActive = false;
  // public bool isAbilityEffect = false;
  private Ability activeAbility = null;

  public override void Tick(float dt)
  {
    // NOTE:
    // Ability 是技能的描述，model
    // Effect 才是技能的影响
  }

  public override bool CanAttack()
  {
    return false;
  }

  public override void Selected(IAbilityCaster caster, Ability ability)
  {
    Debug.Log("Enemy selected by ability > " + ability.name);
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
