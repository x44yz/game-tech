using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public AbilitySlot[] abilitySlots = new AbilitySlot[4];
    public bool isPlayerControl;

	// private int curSelectAbilitySlotIndex = -1;

    private AbilitySlot targetAbilitySlot = null;

	public Action<Actor> onTurnStart;
	public Action<Actor> onTurnFinish;

    void Awake()
    {
        for (int i = 0; i < abilitySlots.Length; ++i)
        {
            abilitySlots[i] = new AbilitySlot();
            abilitySlots[i].caster = this;
        }

        //
        Fireball abilityFireball = new Fireball();
        abilitySlots[0].SetAbility(abilityFireball);

        targetAbilitySlot = abilitySlots[0];
    }

    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
    }

	public void StartTurn()
	{
		if (onTurnStart != null)
			onTurnStart(this);
	}

	private void FinishTurn()
	{
		if (onTurnFinish != null)
			onTurnFinish(this);
	}
}
