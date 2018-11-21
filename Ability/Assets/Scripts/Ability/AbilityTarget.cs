using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTarget
{
    bool CanAttack();

    void Selected(Ability ability);
    void ApplyEffect(Effect effect);
}
