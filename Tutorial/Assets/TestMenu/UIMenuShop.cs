using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

namespace Test
{
    public class UIMenuShop : UIBaseController
    {
        private void OnEnable() 
        {
            TutorialManager.Inst.OnTriggerOpenMenu("MenuShop");
        }
    }
}
