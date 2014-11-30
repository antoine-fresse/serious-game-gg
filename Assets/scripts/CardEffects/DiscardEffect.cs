using UnityEngine;
using System.Collections;

public class DiscardEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var player = (Player) target;
		var action = GetComponent<CardAction>();

		player.Discard(action.attack);


	}
}
