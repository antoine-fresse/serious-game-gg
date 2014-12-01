using UnityEngine;
using System.Collections;

public class BonusDamageFromSexismeEffect : AbstractCardEffect {


	public override void OnUpdate() {

		var actor = GetComponent<CardActor>();

		actor.cardEffect.text = actor.effectDesc + " (<color=green>" + GameManager.instance.getOtherPlayer(actor.owner).sexisme + "</color>)";


	}

	public override void OnAttackPerformed(Target target, int dmg) {

		var bonusDmg = 0;

		if (target.TargetType == TargetType.Player)
			bonusDmg = ((Player) target).sexisme;
		if(target.TargetType == TargetType.Card)
			bonusDmg = ((Card)target).owner.sexisme;

		base.OnAttackPerformed(target, dmg + bonusDmg);


	}

	public override void OnAttackReceived(Target attacker) {

		var actor = GetComponent<CardActor>();
		var bonusDmg = ((Card)attacker).owner.sexisme;

		// Riposte
		var atkr = attacker.GetComponent<CardActor>();
		atkr.ChangeReputation(-(actor.attack + bonusDmg));


	}
}
