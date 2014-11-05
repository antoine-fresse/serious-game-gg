using UnityEngine;
using System.Collections;

public enum CardType
{
    Action,
    Actor,
    Context
}

public enum Place {
    Deck,
    Hand,
    Board
}

public abstract class Card : Target {


    public int attack = 1;
    public int reputation = 1;

    public int corruptionCost = 0;
    public int sexismeCost = 0;



    public bool hidden = false;
    public Player owner;

    public Place place = Place.Deck;
    public CardType cardType = CardType.Action;

    public abstract void useOn(Target c);
    public abstract bool isValidTarget(Target c);
    public void reduceReputation(int value) {
        reputation -= value;
        if (reputation <= 0) {
            Debug.Log("Card is ded yo ");
        }
    }

    public virtual void startTurn() { }

    void OnMouseUp(){
        GameManager.instance.elementClicked(this);
    }

}

