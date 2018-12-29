using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
	

	public SpriteRenderer spr;
	private Animator ani;

	protected virtual void Start() 
	{
		ani = spr.GetComponent<Animator>();
	}

	public void TakeDamage(int damage)
	{
	}

	public bool CheckDeath()
	{
		return false;
	}

	protected void PlayAnimation(string id, bool value)
	{
		ani.SetBool(id, value);
	}

	protected void PlayAnimation(string id)
	{
		ani.SetTrigger(id);
	}

	public void OnAnimationEnd(string id)
	{

	}
}
