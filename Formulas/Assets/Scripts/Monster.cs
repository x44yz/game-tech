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

	private void Update()
	{

	}

	public void DoAttack(Actor atr)
	{
		// 确定攻击动画的第几帧造成伤害
		
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
}