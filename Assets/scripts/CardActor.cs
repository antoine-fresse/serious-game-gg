using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardActor : Card {

    public bool canAttack = false;

	// Use this for initialization

	public Text cardName;
	public Text cardDesc;
	public Text cardCost;
	public Text cardStats;

    protected override void init(){
        base.init();
        cardType = CardType.Actor;

		cardName.text = fullName;
		cardDesc.text = description;
		string cost = "";
		if (corruptionCost > 0 || sexismeCost > 0) {
			cost += "Cout : ";
			if (corruptionCost > 0)
				cost += corruptionCost + " corruption";
			if (sexismeCost > 0)
				cost += sexismeCost + " sexisme";
		}

		cardCost.text = cost;
		cardStats.text = attack + "/" + reputation;
    }

	void Update() {
		cardStats.text = attack + "/" + reputation;
	}

    public override void useOn(Target c) {
        Debug.Log(fullName + " used on " + c.fullName);

		if (c.type == Type.Player) {
			Player p = (Player)c;
			p.reduceReputation(attack);
			effect.OnAttackPerformed(c);
		} else {
			Card ca = (Card)c;
			ca.reduceReputation(attack);
			effect.OnAttackPerformed(c);
			ca.effect.OnAttackReceived(this);
		}
        canAttack = false;
		selectable.interactable = false;
    }

    public override bool isValidTarget(Target t) {

		if (!canAttack || place != Place.Board)
			return false;

		if (t.type == Type.Player) {
			return owner != (Player)t;
		}

		Card ca = (Card)t;

		if (ca.place == Place.Board && ca.owner != owner) {
			return true;
		}

        return true;
    }

    public override void OnTurnStart() {
		if (place == Place.Board) {
			canAttack = true;
			effect.OnTurnStart();
		}
    }



}
