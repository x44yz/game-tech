using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ability 是原始信息
// AbilitySlot 相当于一个管理 Ability 的容器
public class AbilitySlot
{
    public Actor parent;
    public Ability ability = null;
    public int cooldownLeftMSec = 0;

    public bool isActive
    {
        get {
            return ability != null && ability.isActive;
        }
    }

    public void Activate()
    {
        if (ability == null)
            return;

        if (cooldownLeftMSec > 0)
            return;

        ability.Activate(parent);
    }

    public void Deactivate()
    {
        
    }
}
