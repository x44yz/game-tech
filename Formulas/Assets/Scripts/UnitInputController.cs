using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInputController : MonoBehaviour
{
    public Unit owner;
    public Rigidbody2D m_rigidbody;
    public bool B_FacingRight = true;

    void Start()
    {
        owner = GetComponent<Unit>();
        m_rigidbody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (owner.isAttacking)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            owner.Attack();
            m_rigidbody.velocity = new Vector3(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            owner.HeavyAttack();
            m_rigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_rigidbody.AddForce(Vector2.left * owner.moveSpeed);
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_rigidbody.AddForce(Vector2.right * owner.moveSpeed);
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
            m_rigidbody.AddForce(Vector2.up * owner.moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_rigidbody.AddForce(Vector2.down * owner.moveSpeed);
        }


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
       
        owner.m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h)+Mathf.Abs (v));
    }

    void Filp()
    {
        B_FacingRight = !B_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.localScale = theScale;
    }
}
