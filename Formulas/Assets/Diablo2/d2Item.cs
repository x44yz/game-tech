using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Item : MonoBehaviour
    {
        public ItemType _itype;

        public static d2Item Create(Transform parent)
        {
            var obj = new GameObject();
            obj.transform.parent = parent;
            var item = obj.AddComponent<d2Item>();
            return item;
        }

        public void clear()
        {
            _itype = ItemType.None;
        }
    }
}
