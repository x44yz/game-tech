using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorAniState
{
	None,
	Idle,
	Attack,
	Hurt,
	Dead,
	Count,
};

// Actor Animation Manager
public class ActorAnimation
{
	private class AniInfo
	{
		public string name;
		public string condition;
	}

	private Animator ani;
	private AniInfo[] aniInfos = new AniInfo[(int)ActorAniState.Count];
	private ActorAniState aniState = ActorAniState.None;

	public void Init(Actor actor)
	{
		aniInfos[(int)ActorAniState.None] = null;

		var aniInfo = new AniInfo();
		aniInfo.name = "Attack_Idle";
		aniInfo.condition = string.Empty;
		aniInfos[(int)ActorAniState.Idle] = aniInfo;

		aniInfo = new AniInfo();
		aniInfo.name = "Attack_Attack";
		aniInfo.condition = "Attack";
		aniInfos[(int)ActorAniState.Attack] = aniInfo;

		aniInfo = new AniInfo();
		aniInfo.name = "Attack_Hurt";
		aniInfo.condition = "Hurt";
		aniInfos[(int)ActorAniState.Hurt] = aniInfo;

		aniInfo = new AniInfo();
		aniInfo.name = "Attack_Dead";
		aniInfo.condition = "Dead";
		aniInfos[(int)ActorAniState.Dead] = aniInfo;
	}

	public void Update(float dt)
	{

	}

	public void PlayAnimation(ActorAniState aniState)
	{

	}

	public ActorAniState GetCurrentAniState()
	{
		return aniState;
	}
}
