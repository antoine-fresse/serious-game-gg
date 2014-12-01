using UnityEngine;
using System.Collections;

public class BonusDamageFromCorruptionEffect : AbstractCardEffect {
	public override void OnUpdate() {

		var actor = GetComponent<CardActor>();

		actor.cardEffect.text = actor.effectDesc + " (<color=green>" + GameManager.instance.getOtherPlayer(actor.owner).corruption + "</color>)";


	}

	public override void OnAttackPerformed(Target target, int dmg) {

		var bonusDmg = 0;

		if (target.TargetType == TargetType.Player)
			bonusDmg = ((Player) target).corruption;
		if(target.TargetType == TargetType.Card)
			bonusDmg = ((Card)target).owner.corruption;

		base.OnAttackPerformed(target, dmg + bonusDmg);


	}

	public override void OnAttackReceived(Target attacker) {

		var actor = GetComponent<CardActor>();
		var bonusDmg = ((Card)attacker).owner.corruption;

		// Riposte
		var atkr = attacker.GetComponent<CardActor>();
		atkr.ChangeReputation(-(actor.attack + bonusDmg));


	}
}
