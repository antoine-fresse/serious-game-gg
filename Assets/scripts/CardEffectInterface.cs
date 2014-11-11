using UnityEngine;
using System.Collections;

public abstract class CardEffectInterface : MonoBehaviour {

	public virtual void OnInit() { }
	public virtual void OnDraw() { }

	public virtual void OnTurnStart() { } // CardActor Only
	public virtual void OnTurnEnd() { } // CardActor Only

	public virtual void OnActionPerformed(Target target) { } // CardAction Only
	public virtual void OnAttackPerformed(Target target) { } // CardActor Only

	public virtual void OnPlacedOnBoard() { } // CardActor & CardContext Only
	public virtual void OnDeath() { } // CardActor & CardContext Only

	public virtual void OnAttackReceived(Target attacker) { } // CardActor Only

	public virtual void OnSelected() { }
	public virtual void OnDeselected() { }

}
