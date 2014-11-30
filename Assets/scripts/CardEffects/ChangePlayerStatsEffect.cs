using UnityEngine;
using System.Collections;

public class ChangePlayerStatsEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var action = GetComponent<CardAction>();
		var player = (Player) target;

		player.corruption = Mathf.Clamp(player.corruption + action.attack,0,9999);
		player.sexisme = Mathf.Clamp(player.sexisme + action.reputation, 0, 9999);

	}
}
