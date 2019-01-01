using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBehaviour : StateMachineBehaviour
{
	public ActorAnimation actorAni = null;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		string aniName = "unknown";
		if (stateInfo.IsName("Actor_Attack"))
			aniName = "Attack";
		else if (stateInfo.IsName("Actor_Idle"))
			aniName = "Idle";
		// Debug.Log("xx-- ActorBehaviour.OnStateEnter > " + aniName);

		Debug.Assert(actorAni != null, "CHECK");
		actorAni.OnActorBehaviourEnter(stateInfo.shortNameHash);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		string aniName = "unknown";
		if (stateInfo.IsName("Actor_Attack"))
			aniName = "Attack";
		else if (stateInfo.IsName("Actor_Idle"))
			aniName = "Idle";
		// Debug.Log("xx-- ActorBehaviour.OnStateExit > " + aniName);

		// CheckActor(animator);
		// actorAni.OnAniBehaviourEnd();
		Debug.Assert(actorAni != null, "CHECK");
		actorAni.OnActorBehaviourExit(stateInfo.shortNameHash);
	}

	// private void CheckActor(Animator animator)
	// {
	// 	if (actorAni != null)
	// 		return;

	// 	var actor = animator.GetComponentInParent<Actor>();
	// 	Debug.Assert(actor != null, "CHECK");
	// 	// actorAni = actor.ani;
	// }
}
