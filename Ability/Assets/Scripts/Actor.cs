using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IAbilityTarget, IAbilityCaster
{
//    public AbilitySlot[] abilitySlots = new AbilitySlot[4];
//    public bool isPlayerControl;

  // private int curSelectAbilitySlotIndex = -1;
    // private AbilitySlot targetAbilitySlot = null;

  public Action<Actor> onTurnStart;
  public Action<Actor> onTurnFinish;

  public int actionPoint { get; set; }
  // public int bonusActionPoint { get; set; }
  public bool isActionTurn { get; set; }
  public List<Effect> effects = new List<Effect>();

  // public Bonus bonus = new Bonus();
  // public List<Bonus> bonuses = new List<Bonus>();

  public int atk;
  public int def;
  public float crit; // %
  public float critDMG; // %
  public int hp;
  public int mp;

  public bool forbidAction
  {
    get
    {
      for (int i = 0; i < effects.Count; ++i)
      {
        Effect eft = effects[i];
        if (eft.forbidAction)
          return true;
      }
      return false;
    }
  }

  void Awake()
  {
    actionPoint = 1;
    isActionTurn = false;
  }

  public virtual void Tick(float dt)
  {
  }

  public virtual void TickRound()
  {
    foreach(Effect eft in effects)
    {
      eft.TickRound();
    }

    for (int i = effects.Count - 1; i >= 0; --i)
    {
      Effect eft = effects[i];
      if (eft.round <= 0)
      {
        eft.End();
        RemoveEffect(eft);
      }
    }
  }

  public virtual bool CanAttack(){ return false; }

  public virtual void Selected(IAbilityCaster caster, Ability ability){}

  public virtual void ApplyEffect(Effect effect)
  {
    Debug.Assert(effects.Contains(effect) == false, "CHECK");
    effects.Add(effect);

    // update bonus
    // bonusActionPoint += effect.bonusActionPoint;
    // bonus.isActionForbid |= effect.bonus.isActionForbid;
    // bonuses.Add(effect.bonus);
  }

  public virtual void RemoveEffect(Effect effect)
  {
    Debug.Assert(effects.Contains(effect) == true, "CHECK");
    effects.Remove(effect);
  }

  public virtual void TakeDamage(int damage)
  {
    Debug.Assert(damage > 0, "CHECK");
    hp -= damage;
  }

  public virtual void StartTurn()
  {
    isActionTurn = true;

    TickRound();

    if (onTurnStart != null)
      onTurnStart(this);
  }

  protected virtual void FinishTurn()
  {
    isActionTurn = false;

    for (int i = effects.Count - 1; i >= 0; --i)
    {
      Effect effect = effects[i];

      // effect.round -= 1;
      if (effect.round <= 0)
      {
        // remove effect
        RemoveEffect(effect);
      }
    }

    if (onTurnFinish != null)
      onTurnFinish(this);
  }
}
