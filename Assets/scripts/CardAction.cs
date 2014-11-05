using UnityEngine;
using System.Collections;

public class CardAction : Card {



    public bool canTargetPlayers = false;
    public bool canTargetAllies = false;

    void Start() {
        cardType = CardType.Action;
    }
    public void useCard() {
        Player target = GameManager.instance.getOtherPlayer(owner);
        target.reduceReputation(attack);
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
            return owner == c.owner ? canTargetAllies : true;
        }
    }

    public override void useOn(Target t) {
        owner.increaseCorruption(corruptionCost);
        owner.increaseSexisme(sexismeCost);
        Debug.Log(fullName + " used on " + t.fullName);
    }
}