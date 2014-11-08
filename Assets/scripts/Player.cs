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
	void Start () {
	}

    public void draw()
    {
        draw(1);
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
}
