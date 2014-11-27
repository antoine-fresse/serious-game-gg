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

    public override void useOn(Target c) {
        Debug.Log(fullName + " used on " + c.fullName);
    }

    public override bool isValidTarget(Target t) {
	    if (t.TargetType == TargetType.Context)
		    return true;

	    return false;
    }


}
