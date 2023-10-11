using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "AEat", menuName = "AI/A/AEat")]
public class AEat : Action
{
    public override System.Type ActionObjType() => typeof(AEatObj);

    public PointType pointType;
    public float hungerRecoverSpd;
    public float moneyCost;
}
