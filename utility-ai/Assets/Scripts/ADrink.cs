using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "AEat", menuName = "AI/A/AEat")]
public class ADrink : Action
{
    public override System.Type ActionObjType() => typeof(AEatObj);

    public float moneyCost;
    public float minutes;
}
