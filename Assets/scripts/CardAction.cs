using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardAction : Card {

    public bool canTargetPlayers = false;
    public bool canTargetAllies = false;


    protected override void init(){
        base.init();
        cardType = CardType.Action;

    }

    public override bool isValidTarget(Target t) {
        if (t.TargetType == TargetType.Player) {
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
        owner.IncreaseCorruption(corruptionCost);
        owner.IncreaseSexisme(sexismeCost);

        if (t.TargetType == TargetType.Card)
        {
            Card c = (Card)t;
            c.ReduceReputation(attack);
			effect.OnActionPerformed(t);
        }
        else
        {
            Player p = (Player)t;
            p.ReduceReputation(attack);
			effect.OnActionPerformed(t);
        }

        destroy();

        Debug.Log(fullName + " used on " + t.fullName);
    }
}