using UnityEngine;
using System.Collections;

public class BuffFromCorruptionEffect : AbstractCardEffect {

	public override void OnTurnStart() {
		var actor = GetComponent<CardActor>();

		var mine = actor.owner.corruption;
		var his = GameManager.instance.getOtherPlayer(actor.owner).corruption;
		if(his > mine)
			actor.attack += 2;
	}
}
