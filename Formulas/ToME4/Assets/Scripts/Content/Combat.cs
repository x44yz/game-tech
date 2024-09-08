using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat
{
}

public static class ICombatUtil
{
    public static void AttackTarget(this ICombat combat, GameActor target)
    {
        var self = combat as GameActor;
        if (self.combat_dam != null)
        {
            var dam = self.combat_dam + self.GetStr() - target.combat_armor;
            DamageTypes.Get(DamageTypes.PHYSICAL).projector(self, target, Mathf.Max(0, (int)dam));
        }
    }
}
