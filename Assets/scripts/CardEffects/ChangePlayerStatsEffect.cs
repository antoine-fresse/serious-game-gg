using UnityEngine;
using System.Collections;

public class ChangePlayerStatsEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var action = GetComponent<CardAction>();
		var player = (Player) target;

		if (action.attack > 0) {
			player.IncreaseCorruption(action.attack);
		}
		else 
			player.corruption = Mathf.Clamp(player.corruption + action.attack,0,9999);

		if (action.reputation > 0) {
			player.IncreaseSexisme(action.reputation);
		} else 
			player.sexisme = Mathf.Clamp(player.sexisme + action.reputation, 0, 9999);

	}
}
