using UnityEngine;
using System.Collections;

public class DrawEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var player = (Player) target;
		var action = GetComponent<CardAction>();
		player.Draw(action.attack);
	}
}
