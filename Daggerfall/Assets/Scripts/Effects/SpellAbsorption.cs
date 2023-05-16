using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spell Absorption
/// </summary>
public class SpellAbsorption : IncumbentEffect
{
    public static readonly string EffectKey = "SpellAbsorption";

    public override void SetProperties()
    {
        properties.Key = EffectKey;
        properties.ClassicKey = MakeClassicKey(20, 255);
        properties.SupportDuration = true;
        properties.SupportChance = true;
        properties.ChanceFunction = ChanceFunction.Custom;
        properties.AllowedTargets = Effects.TargetFlags_All;
        properties.AllowedElements = Effects.ElementFlags_MagicOnly;
        properties.AllowedCraftingStations = MagicCraftingStations.SpellMaker;
        properties.MagicSkill = DFCareer.MagicSkills.Restoration;
        properties.DurationCosts = MakeEffectCosts(28, 140);
        properties.ChanceCosts = MakeEffectCosts(28, 140);
    }

    // public override string GroupName => TextManager.Instance.GetLocalizedText("spellAbsorption");
    // public override TextFile.Token[] SpellMakerDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1568);
    // public override TextFile.Token[] SpellBookDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1268);

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other.Key == Key) ? true : false;
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        incumbent.RoundsRemaining += RoundsRemaining;
    }
}