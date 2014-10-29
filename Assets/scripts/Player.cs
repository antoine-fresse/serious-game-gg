using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : Target {

    public Text reputationText;

    public int maxCardsInHand = 6;
    public int reputation = 30;
    public int corruption = 0;
    public int sexisme = 0;

    public List<Card> deck;
    public List<Card> hand;
    public List<Card> board;

	// Use this for initialization
	void Start () {
        reputationText.text = reputation.ToString();
	}

    void OnMouseUp() {
        GameManager.instance.elementClicked(this);
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
                hand.Add(d);
                draw(times - 1);
            }
        }
    }
    void Update() {
        displayHand();
        displayBoard();
    }
    public void displayHand() {
        Vector3 pos = transform.position + new Vector3(-hand.Count/2,0,0);
        var offset = new Vector3(1,0,0);
        foreach (var card in hand) {
            card.transform.position = pos;
            pos += offset;
        }
    }

    public void displayBoard() {
        Vector3 pos = gameObject.transform.FindChild("Board").transform.position + new Vector3(-board.Count / 2, 0, 0);
        var offset = new Vector3(1, 0, 0);
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

        reputationText.text = reputation.ToString();
    }

}
