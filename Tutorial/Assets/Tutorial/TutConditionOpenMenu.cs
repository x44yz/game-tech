using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    // [Serializable]
    public class TutConditionOpenMenu : TutorialCondition
    {
        public readonly static List<string> ShopTypes = new List<string>{"purchase", "sell"};

        public string menuName;
        public string[] parameters;

        public override bool IsSatisfied(TutorialTriggerType triggerType,  params object[] datas)
        {
            if (triggerType != TutorialTriggerType.OpenMenu)
                return false;

            string menu = datas[0] as string;
            if (menu.Equals(menuName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            // Debugger.Log("xx-- TutConditionOpenMenu.IsSatisfied > " + menu + " - " + menuName);
            return false;
        }
    }
}