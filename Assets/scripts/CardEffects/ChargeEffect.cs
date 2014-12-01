using UnityEngine;
using System.Collections;

public class ChargeEffect : AbstractCardEffect {

	public override void OnPlacedOnBoard() {

		var actor = GetComponent<CardActor>();

		actor.canAttack = true;

	}
}
