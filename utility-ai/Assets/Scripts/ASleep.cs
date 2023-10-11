using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "ASleep", menuName = "AI/A/ASleep")]
public class ASleep : Action
{
    public override System.Type ActionObjType() => typeof(ASleepObj);

    public float energyRecoverSpd;
}
