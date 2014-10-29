using UnityEngine;
using System.Collections;

public class CardActor : Card {

    public bool canAttack = false;

	// Use this for initialization
	void Start () {
        cardType = CardType.Actor;
	}

    public override bool useOn(Target c) {
        Debug.Log(fullName + " used on " + c.fullName);
        return true;
    }

    public void StartTurn() {
        canAttack = true;
    }



}
