using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo
{
	// StateMachineBehaviour 可以挂在 Animation State 或者 Layer 上
	// TODO
	// 这部分是否整合到 ActorAnimation ?
	public class ActorBehaviour : StateMachineBehaviour
	{
		public ActorAnimation actorAni = null;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Assert(actorAni != null, "CHECK");
			actorAni.OnActorBehaviourEnter(stateInfo.shortNameHash);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Assert(actorAni != null, "CHECK");
			actorAni.OnActorBehaviourExit(stateInfo.shortNameHash);
		}
	}
}
