using UnityEngine;
using System.Collections;

public class DrawOnDeathEffect : AbstractCardEffect {

	public override void OnDeath() {
		var actor = GetComponent<Card>();
		actor.owner.Draw(1);
	}
}
