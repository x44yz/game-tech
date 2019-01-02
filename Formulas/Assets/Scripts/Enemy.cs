using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	protected override void Start() 
	{
		base.Start();

		faceDir = FaceDir.LEFT;
	}

	protected override void Update() 
	{
		if (state == State.Normal)
		{
			// check nearest
			// Actor player = WorldScene.Instance.FindNearestActor(this, ActorType.Player);
			// if (player != null)
			// {
			// }
			if (target != null)
				DoAttack();
		}
		else if (state == State.Attack)
		{
			if (ani.curAniState == ActorAniState.Idle)
				DoAttack();
		}
	}

	private void DoAttack()
	{
		state = State.Attack;
		ani.PlayAnimation(ActorAniState.Attack);
	}
}
