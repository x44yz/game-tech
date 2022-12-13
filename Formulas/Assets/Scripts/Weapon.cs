using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Unit owner;

    void Start()
    {
        owner = this.transform.root.transform.GetComponent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner.isAttacking == false)
            return;

        var target = other.GetComponent<Unit>();
        if (target == null) 
            return;

        if (owner.CanAttack(target) == false)
            return;

        target.TakeDamage(owner, owner.atk);
        Debug.Log("hit::" + other.name);
    }
}
