using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : Target {

    public RectTransform handPos;
    public RectTransform boardPos;

    public int maxCardsInHand = 6;
    public int reputation = 30;
    public int corruption = 0;
    public int sexisme = 0;

    public List<Card> deck;
    public List<Card> hand;
    public List<Card> board;

    public int cardWidth = 200;

	// Use this for initialization

    protected override void init(){
		base.init();
        type = Type.Player;
    }


    public void OnTurnStart()
    {
        draw();
        foreach(Card c in board){
            c.selectable.interactable = true;
			c.OnTurnStart();
        }
        foreach (Card c in hand){
            c.selectable.interactable = true;
        }
    }

    public void OnTurnEnd()
    {
        foreach (Card c in board){
            c.selectable.interactable = false;
			c.OnTurnEnd();
        }
        foreach (Card c in hand){
            c.selectable.interactable = false;
        }
    }

	public void moveToBoard(Card c) {
		if (!hand.Contains(c))
			return;

		hand.Remove(c);
		board.Add(c);
		c.place = Place.Board;
		c.effect.OnPlacedOnBoard();
	}

    public void draw(int times = 1) {
        if (times < 1) {
            return;
        }
        if (deck.Count == 0)
            reduceReputation(times);
        else{
            if (hand.Count < maxCardsInHand) {
                Card d = deck[0];
                deck.RemoveAt(0);
                d.show();
                hand.Add(d);
                d.place = Place.Hand;
				d.effect.OnDraw();
                draw(times - 1);
            }
        }
    }
    void Update() {
        displayDeck();
        displayHand();
        displayBoard();
    }

    public void displayDeck()
    {
        foreach (var card in deck)
        {
            card.transform.position = new Vector3(10000, 0, 0);
            card.hide();
        }
    }

    public void displayHand() {
        Vector3 pos = handPos.position + new Vector3(-cardWidth * hand.Count / 2, 0, 0);
        var offset = new Vector3(cardWidth, 0, 0);
        foreach (var card in hand) {
            card.transform.position = pos;
            pos += offset;

            if (this == GameManager.instance.player2)
            {
                card.hide();
            }
        }
    }

    public void displayBoard() {
        Vector3 pos = boardPos.position + new Vector3(-cardWidth * board.Count / 2, 0, 0);
        var offset = new Vector3(cardWidth, 0, 0);
        foreach (var card in board) {
            card.transform.position = pos;
            pos += offset;
        }
    }

    public void reduceReputation(int value)
    {
        reputation -= value;

        if (reputation <= 0)
        {
            GameManager.instance.playerDied(this);
        }
    }


    public bool isInHand(Card c) {
        return hand.Contains(c);   
    }

    public bool isInDeck(Card c) {
        return deck.Contains(c);
    }

    public void shuffleDeck() {
        // TODO SHUFFLE DECK
    }


    public void increaseCorruption(int cost) {
        if (GameManager.instance.contextCard) {
            cost *= (int)GameManager.instance.contextCard.corruptionMultiplier;
        }
        corruption += cost;
    }
    public void increaseSexisme(int cost) {
        if (GameManager.instance.contextCard) {
            cost *= (int)GameManager.instance.contextCard.sexismeMultiplier;
        }
        sexisme += cost;
    }

    public void removeCard(Card c)
    {
        if (hand.Contains(c))
            hand.Remove(c);
        if (board.Contains(c))
            board.Remove(c);
        if (deck.Contains(c))
            deck.Remove(c);
    }
}
