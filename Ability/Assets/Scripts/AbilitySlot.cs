using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ability 是原始信息
// AbilitySlot 相当于一个管理 Ability 的容器
public class AbilitySlot
{
  public Actor caster;
  private Ability ability = null;
  public int cooldownLeftMSec = 0;

  private AbilityTargeter targeter = null;

  public bool isActive
  {
    get {
      return ability != null && ability.isActive;
    }
  }

  public void SetAbility(Ability ability)
  {
    this.ability = ability;

    // create targeter based on ability
    // TODO:
    // 测试统一采用指向型技能
    targeter = new AbilityTargeter();
  }

  public void Activate()
  {
    if (ability == null)
      return;

    if (cooldownLeftMSec > 0)
      return;

    ability.Activate(caster);
  }

  public void Deactivate()
  {
  }
}
