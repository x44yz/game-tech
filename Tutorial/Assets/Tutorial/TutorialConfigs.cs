using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(fileName = "TutorialConfigs", menuName = "Tutorial/TutorialConfigs", order = 1)]
    public class TutorialConfigs : ScriptableObject
    {
        public Tutorial[] tutorials;
    }
}
