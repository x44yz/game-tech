using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// target selector
// NOTE:
// 释放一个技能，会创建一个技能目标选择器
public class AbilityTargeter
{
  public List<Actor> targets = new List<Actor>();

  public Actor target
  {
    get
    {
      if (targets.Count == 1)
        return targets[0];
      else
      {
        Debug.LogError("target only called when targets.Count == 1");
        return null;
      }
    }
  }

    public virtual void Activate()
    {
    }

    public virtual void Draw()
    {
    }
}
