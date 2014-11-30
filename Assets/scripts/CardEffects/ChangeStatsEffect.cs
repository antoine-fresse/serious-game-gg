using UnityEngine;
using System.Collections;

public class ChangeStatsEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var action = GetComponent<CardAction>();
		if (target.TargetType == TargetType.Player) {
			var player = (Player)target;
			player.ChangeReputation(action.reputation);
			return;
		}

		var actor = (CardActor) target;
		actor.attack = actor.attack + action.attack;
		actor.ChangeReputation(action.reputation);
	}
}
