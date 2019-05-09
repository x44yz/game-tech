using System;

public class Monster : Actor
{
	public enum Status
	{
		STAND,
		WALK,
		ATTACK,
		GOTHIT,
		DEATH,
	}

	public Status status;

	public Actor target;
	public int armorClass;

	public int aniFrame;
	public int aniFrameNum;

	private void Update()
	{
		if (status == Status.STAND)
			DoStand();
		else if (status == Status.ATTACK)
			DoAttack(target);
	}

	private void DoStand()
	{

	}

	public void DoAttack(Actor atr)
	{
		// 确定攻击动画的第几帧造成伤害
		if (aniFrame == aniFrameNum)
		{
			TryHit();
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

	private void TryHit(Actor atr, int hit, int minDamage, int maxDamage)
	{
		
	}
}