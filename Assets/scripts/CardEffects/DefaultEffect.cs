using UnityEngine;
using System.Collections;

public class DefaultEffect : AbstractCardEffect {

	public override void OnAttackPerformed(Target target) {

		var attacker = GetComponent<CardActor>();
		if (target.TargetType == TargetType.Player) {
			Player p = (Player)target;
			p.ChangeReputation(-attacker.attack);
		} else if (target.TargetType == TargetType.Card) {
			Card ca = (Card)target;
			ca.ChangeReputation(-attacker.attack);
			attacker.ChangeReputation(-ca.attack);

			ca.effect.OnAttackReceived(attacker);
			attacker.effect.OnAttackReceived(ca);
		}
	}

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
