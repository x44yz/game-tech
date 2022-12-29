using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public static bool is_hit_from_front(f2Object a1, f2Object a2)
        {
            // int diff = a1.rotation - a2.rotation;
            // if (diff < 0) {
            //     diff = -diff;
            // }

            // return diff != 0 && diff != 1 && diff != 5;
            return true;
        }
    }
}

