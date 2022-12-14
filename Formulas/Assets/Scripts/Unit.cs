using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject model;
    public float moveSpeed;

    [Header("RUNTIME")]
    public bool isAttacking = false;
    public float atk = 0f;
    public int sortingOrder = 0;
    public float spriteOrderUpdateTick = 0f;
    public SpriteRenderer[] m_SpriteGroup;
    public Animator m_Animator;
    public float attackTick = 0f;
    public Weapon weapon = null;

    private void Start()
    {
        m_Animator = transform.GetComponentInChildren<Animator>();
        m_SpriteGroup = model.GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        UpdateSpriteOrder(dt);

        if (isAttacking)
        {
            attackTick -= dt;
            if (attackTick <= 0f)
            {
                attackTick = 0f;
                isAttacking = false;
            }
        }
    }

    public virtual bool CanAttack(Unit target)
    {
        return isAttacking && target != this;
    }

    public void Attack()
    {
        if (isAttacking)
            return;

        isAttacking = true;
        attackTick = 0.367f * 0.7f;
        m_Animator.SetTrigger("Attack");
        weapon.StartAttack();
    }

    public void HeavyAttack()
    {
        if (isAttacking)
            return;

        // Debug.Log("xx-- HeavyAttack > " + name);
        isAttacking = true;
        attackTick = 0.417f * 0.7f;
        m_Animator.SetTrigger("Attack2");
        weapon.StartAttack();
    }

    public void TakeDamage(Unit attacker, float atk)
    {
        m_Animator.Play("Hit");
    }

    private void UpdateSpriteOrder(float dt)
    {
        spriteOrderUpdateTick += dt;

        if (spriteOrderUpdateTick > 0.1f)
        {
            spriteOrderUpdateTick = 0f;

            sortingOrder = Mathf.RoundToInt(this.transform.position.y * 100);
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {
                m_SpriteGroup[i].sortingOrder = 0 - sortingOrder;
            }
        }
    }
}
