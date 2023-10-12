using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "ADrink", menuName = "AI/A/ADrink")]
public class ADrink : Action
{
    public override System.Type ActionObjType() => typeof(ADrinkObj);

    public float moneyCostSpd;
    public float moodEarnSpd;
    public float socialEarnSpd;
    public float minutes;
}
