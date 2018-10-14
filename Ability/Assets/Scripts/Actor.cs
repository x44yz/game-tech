using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public AbilitySlot[] abilitySlots = new AbilitySlot[4];
    public bool isPlayerControl;

    void Awake()
    {
        for (int i = 0; i < abilitySlots.Length; ++i)
        {
            abilitySlots[i] = new AbilitySlot();
            abilitySlots[i].caster = this;
        }

        //
        Fireball abilityFireball = new Fireball();
        abilitySlots[0].ability = abilityFireball;
    }

    void Update()
    {
        if (isPlayerControl)
        {
            UpdatePlayerControl();
        }
    }

    public void TakeDamage(int damage)
    {
        
    }

    public void UpdatePlayerControl()
    {
        int selectSlotIndex = -1;
        if (Input.GetKeyDown(KeyCode.Q))
            selectSlotIndex = 0;

        if (selectSlotIndex != -1)
        {
            abilitySlots[selectSlotIndex].Activate();
        }

        // check select actor
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("xx-- click Actor");
            }
        }
    }
}
