using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一切技能最终都要转化为 Effect
// Effect 不是技能的特效表现，二是数值和状态上的影响
public class Effect
{
  // private EffectTarget target;
  // private int durationMSec;
  // icon
  // title
  // description

  // 持续多少回合
  public int round { get; set; }

  // public int bonusActionPoint { get; set; }
  // public Bonus bonus = new Bonus();
  public bool isActionForbid = false;

  public void TickRound()
  {
    round -= 1;
  }
}
