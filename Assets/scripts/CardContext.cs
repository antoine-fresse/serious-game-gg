using UnityEngine;
using System.Collections;

public class CardContext : Card {

    public float corruptionMultiplier = 1.0f;
    public float sexismeMultiplier = 1.0f;

    // Use this for initialization
    void Start() {
        cardType = CardType.Context;
    }

    public override void useOn(Target c) {
        Debug.Log(fullName + " used on " + c.fullName);
    }

    public override bool isValidTarget(Target t) {
        return false;
    }


    



}
