using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public AbilitySlot[] abilitySlots = new AbilitySlot[4];

    void Awake()
    {
        for (int i = 0; i < abilitySlots.Length; ++i)
        {
            abilitySlots[i].caster = this;
        }

        //
        Fireball abilityFireball = new Fireball();
        abilitySlots[0].ability = abilityFireball;
    }

    void Update()
    {
        int selectSlotIndex = -1;
        if (Input.GetKeyDown(KeyCode.Q))
            selectSlotIndex = 0;

        if (selectSlotIndex != -1)
        {
            abilitySlots[selectSlotIndex].Activate();
        }
    }

    public void TakeDamage(int damage)
    {
        
    }
}
