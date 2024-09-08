using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Unit owner;
    public List<Unit> attackTargets = new List<Unit>();

    void Start()
    {
        owner = this.transform.root.transform.GetComponent<Unit>();
    }

    public void StartAttack()
    {
        attackTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (owner.isAttacking == false)
        //     return;

        // var target = other.GetComponent<Unit>();
        // if (target == null) 
        //     return;

        // // 防止对象多个 collider 引起多次 enter
        // if (attackTargets.Contains(target))
        //     return;
        // attackTargets.Add(target);

        // if (owner.CanAttack(target) == false)
        //     return;

        // owner.HitTarget(target);
        // Debug.Log("hit::" + other.name + " - " + other.GetInstanceID());
    }
}
