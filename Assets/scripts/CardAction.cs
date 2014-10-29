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

    public override bool useOn(Target t) {
        Debug.Log(fullName + " used on " + t.fullName);

        if (t.type == Type.Player) {
            if (!canTargetPlayers)
                return false;

            Player p = (Player)t;
            if (p == owner){
                if (canTargetAllies) {
                    // DEFAULT BEHAVIOUR
                    p.reduceReputation(attack);
                    owner.hand.Remove((Card)this);
                    GameObject.Destroy(this);
                    return true;
                }
                
            }
            else {
                // DEFAULT BEHAVIOUR
                p.reduceReputation(attack);
                owner.hand.Remove((Card)this);
                GameObject.Destroy(this);
                return true;
            }
                
        }
        else {
            Card c = (Card)t;
            if (owner == c.owner) {
                if (canTargetAllies) {
                    // DEFAULT BEHAVIOUR
                    c.reduceReputation(attack);
                    owner.hand.Remove((Card)this);
                    GameObject.Destroy(this);
                    return true;
                }
            }
            else {
                // DEFAULT BEHAVIOUR
                c.reduceReputation(attack);
                owner.hand.Remove((Card)this);
                GameObject.Destroy(this);
                return true;
            }
        }
        return false;
    }
}