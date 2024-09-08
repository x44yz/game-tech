using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Paralyze
/// </summary>
public class Paralyze : IncumbentEffect
{
    public static readonly string EffectKey = "Paralyze";

    bool awakeAlert = true;

    public override void SetProperties()
    {
        properties.Key = EffectKey;
        properties.ClassicKey = MakeClassicKey(0, 255);
        properties.SupportDuration = true;
        properties.SupportChance = true;
        properties.AllowedTargets = Effects.TargetFlags_Other;
        properties.AllowedElements = Effects.ElementFlags_All;
        properties.AllowedCraftingStations = MagicCraftingStations.SpellMaker;
        properties.MagicSkill = DFCareer.MagicSkills.Alteration;
        properties.DurationCosts = MakeEffectCosts(28, 100);
        properties.ChanceCosts = MakeEffectCosts(28, 100);
    }

    // public override string GroupName => TextManager.Instance.GetLocalizedText("paralyze");
    // public override TextFile.Token[] SpellMakerDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1502);
    // public override TextFile.Token[] SpellBookDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1202);

    public override void Start(ActorEffect manager, Actor caster = null)
    {
        base.Start(manager, caster);
        PlayerAggro();
    }

    public override void ConstantEffect()
    {
        base.ConstantEffect();
        StartParalyzation();
    }

    public override void Resume(ActorEffect.EffectSaveData_v1 effectData, ActorEffect manager, Actor caster = null)
    {
        base.Resume(effectData, manager, caster);
        StartParalyzation();
    }

    public override void End()
    {
        base.End();
        StopParalyzation();
    }

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other is Paralyze);
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        // Stack my rounds onto incumbent
        incumbent.RoundsRemaining += RoundsRemaining;
    }

    void StartParalyzation()
    {
        // Get peered entity gameobject
        var entityBehaviour = GetPeeredEntityBehaviour(manager);
        if (!entityBehaviour)
            return;

        entityBehaviour.IsParalyzed = true;
        PlayerAggro();

        // Output "You are paralyzed." if the host manager is player
        // if (awakeAlert && manager.EntityBehaviour == GameManager.Instance.PlayerEntityBehaviour)
        // {
        //     DaggerfallUI.AddHUDText(TextManager.Instance.GetLocalizedText("youAreParalyzed"), 1.5f);
        //     awakeAlert = false;
        // }
    }

    void StopParalyzation()
    {
        // Get peered entity gameobject
        var entityBehaviour = GetPeeredEntityBehaviour(manager);
        if (!entityBehaviour)
            return;

        entityBehaviour.IsParalyzed = false;
    }
}