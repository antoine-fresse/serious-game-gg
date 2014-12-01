using UnityEngine;
using System.Collections;

public abstract class AbstractCardEffect : MonoBehaviour {

	void Update() {
		if(GameManager.instance)
			OnUpdate();
	}

	public virtual void OnUpdate() { }
	public virtual void OnInit() { }
	public virtual void OnDraw() { }

	public virtual void OnTurnStart() { } // CardActor Only
	public virtual void OnTurnEnd() { } // CardActor Only

	public virtual void OnActionPerformed(Target target) { } // CardAction Only

	public virtual void OnAttackPerformed(Target target, int dmg) { // CardActor Only

		var attacker = GetComponent<CardActor>();
		if (target.TargetType == TargetType.Player) {
			Player p = (Player)target;
			p.ChangeReputation(-dmg);
		} else if (target.TargetType == TargetType.Card) {
			Card ca = (Card)target;
			ca.ChangeReputation(-dmg);
			//attacker.ChangeReputation(-ca.attack);

			// Riposte
			ca.effect.OnAttackReceived(attacker);
			
		}
	}

	public virtual void OnPlacedOnBoard() { } // CardActor & CardContext Only
	public virtual void OnDeath() { } // CardActor & CardContext Only

	public virtual void OnAttackReceived(Target attacker) {
		// Riposte
		var atkr = attacker.GetComponent<CardActor>();
		Card ca = GetComponent<CardActor>();
		atkr.ChangeReputation(-ca.attack);
	} // CardActor Only

	public virtual void OnSelected() { }
	public virtual void OnDeselected() { }

	public virtual bool IsValidTarget(Target t) {

		var self = GetComponent<Card>();

		if (self.cardType == CardType.Action) {

			var action = (CardAction) self;
			if (t.TargetType == TargetType.Player) {
				Player p = (Player) t;
				return p == self.owner ? action.canTargetSelf : action.canTargetEnemyPlayer;
			}
			else {
				Card c = (Card) t;
				if (c.place != Place.Board)
					return false;

				return self.owner == c.owner ? action.canTargetAllies : action.canTargetActors;
			}
		}
		else if (self.cardType == CardType.Actor) {
			var actor = (CardActor) self;
			if (!actor.canAttack || actor.place != Place.Board || t == actor)
				return false;

			if (t.TargetType == TargetType.Player) {
				return actor.owner != (Player) t;
			}

			Card ca = (Card) t;

			if (ca.place == Place.Board && ca.owner != actor.owner) {
				return true;
			}

			return false;

		} else if (self.cardType == CardType.Context) {
			return t.TargetType == TargetType.Context;
		}
		return false;
	}

}
