using UnityEngine;
using System.Collections;

public class AoeOnPlacedEffect : AbstractCardEffect {

	public override void OnPlacedOnBoard() {

		var actor = GetComponent<CardActor>();

		var other = GameManager.instance.getOtherPlayer(actor.owner);


		foreach(var card in other.board.ToArray())
		{
			card.ChangeReputation(-actor.custom_param);
		}

	}
}
