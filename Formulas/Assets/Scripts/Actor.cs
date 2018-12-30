using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
	public SpriteRenderer spr;
	// private Animator ani;
	public ActorAnimation ani;

	protected virtual void Start() 
	{
		// ani = spr.GetComponent<Animator>();
	}

	private void Update() 
	{
		// var aniStateInfo = ani.GetCurrentAnimatorStateInfo(0);
		// if (aniStateInfo.IsName(""))
	}

	public void TakeDamage(int damage)
	{
	}

	public bool CheckDeath()
	{
		return false;
	}

	// protected void PlayAnimation(string id, bool value)
	// {
	// 	ani.SetBool(id, value);
	// }

	// protected void PlayAnimation(string id)
	// {
	// 	ani.SetTrigger(id);
	// }
}
