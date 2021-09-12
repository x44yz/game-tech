using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSMTool
{
    public class BTDef
    {
        public const string B3_CATEGORY_COMPOSITE = "composite";
        public const string B3_CATEGORY_ACTION = "action";
        public const string B3_CATEGORY_CONDITION = "condition";
        public const string B3_CATEGORY_DECORATOR = "decorator";

        public const string B3_COMPOSITE_SEQUENCE = "Sequence";
        public const string B3_COMPOSITE_PRIORITY = "Priority";
        public const string B3_COMPOSITE_MEMSEQUENCE = "MemSequence";
        public const string B3_COMPOSITE_MEMPRIORITY = "MemPriority";

        // public static readonly Dictionary<Type, string> TsiUToB3 = new Dictionary<Type, string>()
        // {
        //     { typeof(TsiU.TBTSequence), B3_COMPOSITE_SEQUENCE },
        //     { typeof(TsiU.TBTPrioritizedSelector), B3_COMPOSITE_PRIORITY },
        //     { typeof(TsiU.TBTNonPrioritizedSelector), B3_COMPOSITE_MEMPRIORITY }
        // };

        // public static readonly Dictionary<Type, string> TsiUNodeDisplayName = new Dictionary<Type, string>()
        // {
        //     { typeof(TsiU.TBTSequence), "Sequence" },
        //     { typeof(TsiU.TBTPrioritizedSelector),  "PrioritySelector" },
        //     { typeof(TsiU.TBTNonPrioritizedSelector), "NonPrioritySelector" },
        //     { typeof(TsiU.TBTParallel), "Parallel" },
        // };
    }
}