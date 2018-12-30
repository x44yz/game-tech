using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBehaviour : StateMachineBehaviour
{
	private Actor actor = null;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		string aniName = "unknown";
		if (stateInfo.IsName("Actor_Attack"))
			aniName = "Attack";
		else if (stateInfo.IsName("Actor_Idle"))
			aniName = "Idle";

		Debug.Log("xx-- ActorBehaviour.OnStateEnter > " + aniName);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		string aniName = "unknown";
		if (stateInfo.IsName("Actor_Attack"))
			aniName = "Attack";
		else if (stateInfo.IsName("Actor_Idle"))
			aniName = "Idle";

		Debug.Log("xx-- ActorBehaviour.OnStateExit > " + aniName);

		CheckActor(animator);

		// if (stateInfo.IsName("Actor_Attack"))
		// actor.
	}

	private void CheckActor(Animator animator)
	{
		if (actor != null)
			return;

		actor = animator.GetComponentInParent<Actor>();
		Debug.Assert(actor != null, "CHECK");
	}
}
