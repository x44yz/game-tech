using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public static class DamageTypes
{
    public const string PHYSICAL = "PHYSICAL";

    public static List<DamageType> damageTypes;

    public static void Init()
    {
        damageTypes = new List<DamageType>();

        damageTypes.Add(new DamageType(){type = PHYSICAL, projector = DamageTypes.DefaultProjector});

    }

    public static int DefaultProjector(Engine.Actor src, Engine.Actor target, int dam)
    {
        // throw new System.NotImplementedException();
        var gactor = src as GameActor;
        if (gactor.isHero)
            dam = dam * 6;
        target.TakeHit(dam, src);
        return dam;
    }

    public static DamageType Get(string type)
    {
        return damageTypes.Find(x => x.type == type);
    }
}
