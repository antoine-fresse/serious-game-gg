using GoogleFu;
using UnityEngine;
using System.Collections;

public class CardContext : Card {

    public float corruptionMultiplier = 1.0f;
    public float sexismeMultiplier = 1.0f;

	

    // Use this for initialization
    protected override void init(){
        base.init();
        cardType = CardType.Context;
    }

	[RPC]
    protected override void useOnRPC(int viewID) {
        // Do Nothing
    }

    public override bool isValidTarget(Target t) {
	    return effect.IsValidTarget(t);
    }


}
