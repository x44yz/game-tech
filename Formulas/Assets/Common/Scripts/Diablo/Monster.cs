using System;
using UnityEngine;

namespace Diablo
{
	public class Monster : MonoBehaviour
	{
		public enum Status
		{
			STAND,
			WALK,
			ATTACK,
			GOTHIT,
			DEATH,
		}

		public int hp;

		public Status status;

		public Player atkTarget;
		public int armorClass;

		public int aniFrame;
		public int aniFrameNum;

		private void Update()
		{
			if (status == Status.STAND)
				DoStand();
			else if (status == Status.ATTACK)
				DoAttack(atkTarget);
		}

		private void DoStand()
		{

		}

		public void DoAttack(Player plr)
		{
			// 确定攻击动画的第几帧造成伤害
			if (aniFrame == aniFrameNum)
			{
				// TryHit(plr, );
			}
		}

		public void StartKill()
		{

		}

		public void StartHit(int damage)
		{
			status = Status.GOTHIT;

			// TODO:
			// play got hit animation
		}

		private void TryHit(Player plr, int hit, int minDamage, int maxDamage)
		{
			if (plr.hp >> 6 <= 0 || plr.invincible)
				return;

			// NOTE:
			// 判断距离

			
		}
	}
}
