using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Magical concealment effect base.
/// Provides functionality common to all magical concealment effects (Chameleon/Invisibility/Shadow).
/// </summary>
public abstract class ConcealmentEffect : IncumbentEffect
{
    protected MagicalConcealmentFlags concealmentFlag = MagicalConcealmentFlags.None;
    protected string startConcealmentMessageKey = string.Empty;
    bool awakeAlert = true;

    public override void ConstantEffect()
    {
        base.ConstantEffect();
        StartConcealment();
    }

    public override void Start(ActorEffect manager, Actor caster = null)
    {
        base.Start(manager, caster);
        StartConcealment();
    }

    public override void Resume(ActorEffect.EffectSaveData_v1 effectData, ActorEffect manager, Actor caster = null)
    {
        base.Resume(effectData, manager, caster);
        StartConcealment();
    }

    public override void End()
    {
        base.End();
        StopConcealment();
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        // Stack my rounds onto incumbent
        incumbent.RoundsRemaining += RoundsRemaining;
    }

    protected virtual void StartConcealment()
    {
        // Get peered entity gameobject
        Actor entityBehaviour = GetPeeredEntityBehaviour(manager);
        if (!entityBehaviour)
            return;

        entityBehaviour.MagicalConcealmentFlags |= concealmentFlag;

        if (!string.IsNullOrEmpty(startConcealmentMessageKey))
        {
            // Output start of concealment message if the host manager is player (e.g. "You are invisible.")
            if (IsIncumbent && awakeAlert && entityBehaviour == Main.Inst.hero)
            {
                // DaggerfallUI.AddHUDText(TextManager.Instance.GetLocalizedText(startConcealmentMessageKey), 1.5f);
                awakeAlert = false;
            }
        }
    }

    protected virtual void StopConcealment()
    {
        // Get peered entity gameobject
        Actor entityBehaviour = GetPeeredEntityBehaviour(manager);
        if (!entityBehaviour)
            return;

        entityBehaviour.MagicalConcealmentFlags &= ~concealmentFlag;
    }
}