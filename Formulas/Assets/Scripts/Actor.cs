using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorType
{
	Player,
	Enemy,
}

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
	public ActorAnimation ani = new ActorAnimation();
	public FaceDir faceDir { 
		get { return m_faceDir; }
		set { 
			m_faceDir = value;
			if (ani != null)
				ani.SetFaceDir(m_faceDir);
		}
	}

	public Vector2 pos {
		get;
		set;
	}

	protected State state;
	private FaceDir m_faceDir;

	public Actor target = null;

	protected virtual void Start() 
	{
		state = State.Normal;
		// ani = spr.GetComponent<Animator>();
		ani.Init(this);
	}

	protected virtual void Update() 
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

	public bool InAttackRange(Vector2 pos)
	{
		throw new System.NotImplementedException();
	}
}
