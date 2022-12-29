using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public static f2Object obj_dude; // 当前选中的 unit
        public static f2Object inven_dude = null; // 当前查看背包的 unit

        // TODO: Rather complex, but understandable, needs testing.
        // static int make_straight_path_func(f2Object a1, int from, int to, StraightPathNode pathNodes, Object** a5, int a6, PathBuilderCallback* callback)
        // {
        // }

        static int tile_num_beyond(int from, int to, int distance)
        {
            // TODO
            return 0;
        }
    }
}
