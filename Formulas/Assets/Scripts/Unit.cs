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

    private void Start()
    {
        m_Animator = transform.GetComponentInChildren<Animator>();
        m_SpriteGroup = model.GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        UpdateSpriteOrder(dt);
    }

    public virtual bool CanAttack(Unit target)
    {
        return true;
    }

    public void Attack()
    {
        m_Animator.SetTrigger("Attack");
    }

    public void HeavyAttack()
    {
        m_Animator.SetTrigger("Attack2");
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
