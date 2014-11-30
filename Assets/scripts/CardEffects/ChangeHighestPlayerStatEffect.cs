using UnityEngine;
using System.Collections;

public class ChangeHighestPlayerStatEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var action = GetComponent<CardAction>();
		var player = (Player) target;


		if (player.corruption > player.sexisme) {
			player.corruption = Mathf.Clamp(player.corruption + action.attack, 0, 9999);
		} else if (player.corruption < player.sexisme) {
			player.sexisme = Mathf.Clamp(player.sexisme + action.attack, 0, 9999);
		}
		else {
			player.corruption = Mathf.Clamp(player.corruption + action.attack/2, 0, 9999);
			player.sexisme = Mathf.Clamp(player.sexisme + action.attack/2, 0, 9999);
		}

	}
}
