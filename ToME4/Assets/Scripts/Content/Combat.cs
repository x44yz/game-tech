using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat
{
}

public static class ICombatUtil
{
    public static void AttackTarget(this ICombat combat, Actor target)
    {
        var self = combat as Actor;
        if (self.combat_dam != null)
        {
            var dam = self.combat_dam + self.getStr() - target.combat_armor;
            
        }
    }
}
