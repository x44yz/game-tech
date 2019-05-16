using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo
{

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
		private enum AniConditionType
		{
			None,
			Float,
			Int,
			Bool,
			Trigger,
		}

		private class AniInfo
		{
			public string name;
			public int shortNameHash;
			public string condition;
			public AniConditionType ctype;
		}

		private Animator ani;
		private AniInfo[] aniInfos = new AniInfo[(int)ActorAniState.Count];
		// private ActorAniState curAniState = ActorAniState.None;
		public ActorAniState curAniState { get; set; }

		public void Init(Actor actor)
		{
			ani = actor.gameObject.GetComponentInChildren<Animator>();
			Debug.Assert(ani != null, "CHECK: ActorAnimation init ani is null > " + actor.gameObject.name);
			ActorBehaviour[] abs = ani.GetBehaviours<ActorBehaviour>();
			for (int i = 0; i < abs.Length; ++i)
			{
				abs[i].actorAni = this;
			}

			aniInfos[(int)ActorAniState.None] = null;

			var aniInfo = new AniInfo();
			aniInfo.name = "Actor_Idle";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Idle");
			aniInfo.condition = string.Empty;
			aniInfo.ctype = AniConditionType.None;
			aniInfos[(int)ActorAniState.Idle] = aniInfo;

			aniInfo = new AniInfo();
			aniInfo.name = "Actor_Attack_L";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Attack_L");
			aniInfo.condition = "Attack";
			aniInfo.ctype = AniConditionType.Trigger;
			aniInfos[(int)ActorAniState.Attack] = aniInfo;

			aniInfo = new AniInfo();
			aniInfo.name = "Actor_Attack_R";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Attack_R");
			aniInfo.condition = "Attack";
			aniInfo.ctype = AniConditionType.Trigger;
			aniInfos[(int)ActorAniState.Attack] = aniInfo;

			aniInfo = new AniInfo();
			aniInfo.name = "Actor_Hurt_L";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Hurt_L");
			aniInfo.condition = "Hurt";
			aniInfo.ctype = AniConditionType.Trigger;
			aniInfos[(int)ActorAniState.Hurt] = aniInfo;

			aniInfo = new AniInfo();
			aniInfo.name = "Actor_Hurt_R";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Hurt_R");
			aniInfo.condition = "Hurt";
			aniInfo.ctype = AniConditionType.Trigger;
			aniInfos[(int)ActorAniState.Hurt] = aniInfo;

			aniInfo = new AniInfo();
			aniInfo.name = "Actor_Dead";
			aniInfo.shortNameHash = Animator.StringToHash("Actor_Dead");
			aniInfo.condition = "Dead";
			aniInfo.ctype = AniConditionType.Bool;
			aniInfos[(int)ActorAniState.Dead] = aniInfo;

			// make sure
			SetFaceDir(actor.faceDir);
		}

		public void Update(float dt)
		{

		}

		public void PlayAnimation(ActorAniState aniState)
		{
			this.curAniState = aniState;

			var aniInfo = aniInfos[(int)aniState];
			Debug.Assert(aniInfo != null, "CHECK");

			if (aniInfo.ctype == AniConditionType.Trigger)
			{
				ani.SetTrigger(aniInfo.condition);
			}
			else
				throw new System.NotImplementedException();
		}

		public void SetFaceDir(FaceDir faceDir)
		{
			if (ani != null)
				ani.SetInteger("FaceDir", (int)faceDir);
		}

		public void OnActorBehaviourEnter(int shortNameHash)
		{
			// TODO
			// 优化: 不用 for 每次查找
			for (int i = (int)ActorAniState.None + 1; i < aniInfos.Length; ++i)
			{
				var aniInfo = aniInfos[i];
				if (aniInfo.shortNameHash == shortNameHash)
				{
					curAniState = (ActorAniState)i;
					break;
				}
			}
		}

		public void OnActorBehaviourExit(int shortNameHash)
		{
			// TODO
			// 优化: 不用 for 每次查找
			// for (int i = 0; i < aniInfos.Length; ++i)
			// {
				// var aniInfo = aniInfos[i];
				// if (aniInfo.shortNameHash == shortNameHash)
			// }
		}
	}

}