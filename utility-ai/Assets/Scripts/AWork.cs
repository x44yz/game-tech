using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "AWork", menuName = "AI/A/AWork")]
public class AWork : Action
{
    public override System.Type ActionObjType() => typeof(AWorkObj);

    public float energyDrainSpd;
    public float moneyCollectSpd;
    public float hungerDrainSpd;
}
