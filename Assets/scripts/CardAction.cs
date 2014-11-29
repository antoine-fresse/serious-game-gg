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

	[RPC]
	protected override void useOnRPC(int viewID) {

		Target t = PhotonView.Find(viewID).GetComponent<Target>();

		show();

		owner.IncreaseCorruption(corruptionCost);
		owner.IncreaseSexisme(sexismeCost);


		effect.OnActionPerformed(t);

		destroy();
	}

}