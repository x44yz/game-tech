using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo
{
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
		// public enum State
		// {
		// 	Normal,
		// 	Attack,
		// 	Hurt,
		// 	Dead,
		// } 

		public SpriteRenderer spr;
		public Vector3 healthBarOffset;

		//public int hp;
		//protected int maxHP;

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

		// protected State state;
		private FaceDir m_faceDir;
		protected GameObject healthBar;

		public Actor target = null;
		public float baseAttackSpeed = 0f;

		protected float attackSpeedTick = 0f;

		protected virtual void Awake() 
		{
			healthBar = GameObject.Instantiate(GameManager.Instance.actorHealthBar);
			healthBar.transform.SetParent(spr.transform, false);
			healthBar.transform.localPosition = healthBarOffset;

			//maxHP = hp;
		}

		protected virtual void Start() 
		{
			// state = State.Normal;
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
			//hp = hp - damage;
			
			UpdateHealthBar();
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

		protected void UpdateHealthBar()
		{
			// float sx = hp * 1.0f / maxHP;
			// sx = Mathf.Clamp(sx, 0, 1);
			// healthBar.transform.localScale = new Vector3(sx, 1, 1);
		}
	}
}

