using UnityEngine;
using System.Collections;

public class SendToHandEffect : AbstractCardEffect {

	public override void OnActionPerformed(Target target) {
		var actor = (CardActor)target;
		actor.owner.MoveToHand(actor);
	}
}
