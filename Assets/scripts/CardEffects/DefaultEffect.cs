using UnityEngine;
using System.Collections;

public class DefaultEffect : CardEffectInterface {

	public override void OnAttackPerformed(Target target) {

		var attacker = GetComponent<CardActor>();
		if (target.TargetType == TargetType.Player) {
			Player p = (Player)target;
			p.ReduceReputation(attacker.attack);
		} else if (target.TargetType == TargetType.Card) {
			Card ca = (Card)target;
			ca.ReduceReputation(attacker.attack);
			attacker.ReduceReputation(ca.attack);

			ca.effect.OnAttackReceived(attacker);
			attacker.effect.OnAttackReceived(ca);
		}
	}

	public override void OnActionPerformed(Target target) {
		var attacker = GetComponent<CardAction>();
		if (target.TargetType == TargetType.Card) {
			Card c = (Card)target;
			c.ReduceReputation(attacker.attack);
		} else {
			Player p = (Player)target;
			p.ReduceReputation(attacker.attack);
		}
	}
}
