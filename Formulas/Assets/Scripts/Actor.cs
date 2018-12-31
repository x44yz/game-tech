﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// 动画状态和 Actor 状态不同
public class Actor : Entity
{
	// 更复杂采用 FSM
	public enum State
	{
		Normal,
		Attack,
		Hurt,
		Dead,
	} 

	public SpriteRenderer spr;
	// private Animator ani;
	public ActorAnimation ani;

	protected State state;

	protected virtual void Start() 
	{
		state = State.Normal;

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