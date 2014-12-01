using UnityEngine;
using System.Collections;

public class DefaultEffect : AbstractCardEffect {

	

	public override void OnActionPerformed(Target target) {
		var attacker = GetComponent<CardAction>();
		if (target.TargetType == TargetType.Card) {
			Card c = (Card)target;
			c.ChangeReputation(-attacker.attack);
		} else {
			Player p = (Player)target;
			p.ChangeReputation(-attacker.attack);
		}
	}
}
