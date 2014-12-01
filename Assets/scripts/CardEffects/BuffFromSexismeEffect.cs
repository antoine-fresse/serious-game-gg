using UnityEngine;
using System.Collections;

public class BuffFromSexismeEffect : AbstractCardEffect {

	public override void OnTurnStart() {
		var actor = GetComponent<CardActor>();

		var mine = actor.owner.sexisme;
		var his = GameManager.instance.getOtherPlayer(actor.owner).sexisme;
		if(his > mine)
			actor.attack += 2;
	}
}
