using UnityEngine;
using System.Collections;

public class PreventAttackEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var actor = (CardActor)target;
		actor.preventAttack = true;
	}
}
