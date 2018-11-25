using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    // private AbilityCircleTargeter targeter = null;


    public override void OnActivate(Actor parent)
    {
//        if (targeter == null)
//        {
//            targeter = new AbilityCircleTargeter();
//            targeter.radius = 5;
//            targeter.Activate();
//        }
    }

    public override void OnDeactivate()
    {
        
    }

    public override void OnTargetSelect()
    {
        // TODO:
        // 播放技能释放动画，粒子特效
        // delay: 创造火球，火球飞往目标，计算飞行的时间
        // delay: 真正释放到对方身上，创建 Effect
    }

    private void PerformFireball()
    {
    }
}
