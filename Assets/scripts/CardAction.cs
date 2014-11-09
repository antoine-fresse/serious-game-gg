using UnityEngine;
using System.Collections;

public class CardAction : Card {



    public bool canTargetPlayers = false;
    public bool canTargetAllies = false;

    void Start() {
        cardType = CardType.Action;
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
        }
        else
        {
            Player p = (Player)t;
            p.reduceReputation(attack);
        }

        destroy();

        Debug.Log(fullName + " used on " + t.fullName);
    }
}