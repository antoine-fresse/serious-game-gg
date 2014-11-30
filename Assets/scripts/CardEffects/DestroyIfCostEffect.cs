using UnityEngine;
using System.Collections;

public class DestroyIfCostEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var actor = (CardActor)target;
		actor.ChangeReputation(-9999);
	}


	public override bool IsValidTarget(Target t) {

		if (!base.IsValidTarget(t))
			return false;

		var actor = (CardActor) t;

		var action = GetComponent<CardAction>();

		if (action.attack == 0) {
			if (actor.corruptionCost > 0) {
				return true;
			}
		}
		else {
			if (actor.sexismeCost > 0) {
				return true;
			}
		}

		return false;
	}
}
