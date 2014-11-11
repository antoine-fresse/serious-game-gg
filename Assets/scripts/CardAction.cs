using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardAction : Card {

    public bool canTargetPlayers = false;
    public bool canTargetAllies = false;


	public Text cardName;
	public Text cardDesc;
	public Text cardCost;

    protected override void init(){
        base.init();
        cardType = CardType.Action;

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

    }

    public override bool isValidTarget(Target t) {
        if (t.type == Type.Player) {
            if (!canTargetPlayers)
                return false;

            Player p = (Player)t;
            return p == owner ? canTargetAllies : true;
        }
        else {
            Card c = (Card)t;
            if (c.place != Place.Board)
                return false;

            return owner == c.owner ? canTargetAllies : true;
        }
    }

    public override void useOn(Target t) {
        owner.increaseCorruption(corruptionCost);
        owner.increaseSexisme(sexismeCost);

        if (t.type == Type.Card)
        {
            Card c = (Card)t;
            c.reduceReputation(attack);
			effect.OnActionPerformed(t);
        }
        else
        {
            Player p = (Player)t;
            p.reduceReputation(attack);
			effect.OnActionPerformed(t);
        }

        destroy();

        Debug.Log(fullName + " used on " + t.fullName);
    }
}