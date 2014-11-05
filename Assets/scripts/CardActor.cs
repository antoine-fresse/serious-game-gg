using UnityEngine;
using System.Collections;

public class CardActor : Card {

    public bool canAttack = false;

	// Use this for initialization
	void Start () {
        cardType = CardType.Actor;
	}

    public override void useOn(Target c) {
        Debug.Log(fullName + " used on " + c.fullName);

        canAttack = false;
    }

    public override bool isValidTarget(Target t) {
        return true;
    }

    public override void startTurn() {
        if(place == Place.Board)
            canAttack = true;
    }



}
