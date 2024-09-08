using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spell Resistance
/// </summary>
public class SpellResistance : IncumbentEffect
{
    public static readonly string EffectKey = "SpellResistance";

    public override void SetProperties()
    {
        properties.Key = EffectKey;
        properties.ClassicKey = MakeClassicKey(22, 255);
        properties.SupportDuration = true;
        properties.SupportChance = true;
        properties.ChanceFunction = ChanceFunction.Custom;
        properties.AllowedTargets = Effects.TargetFlags_All;
        properties.AllowedElements = Effects.ElementFlags_MagicOnly;
        properties.AllowedCraftingStations = MagicCraftingStations.SpellMaker;
        properties.MagicSkill = DFCareer.MagicSkills.Thaumaturgy;
        properties.DurationCosts = MakeEffectCosts(20, 100);
        properties.ChanceCosts = MakeEffectCosts(20, 100);
    }

    // public override string GroupName => TextManager.Instance.GetLocalizedText("spellResistance");
    // public override TextFile.Token[] SpellMakerDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1570);
    // public override TextFile.Token[] SpellBookDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1270);

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other.Key == Key) ? true : false;
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        incumbent.RoundsRemaining += RoundsRemaining;
    }
}
