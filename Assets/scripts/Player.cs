using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : Target {

    public RectTransform handPos;
    public RectTransform boardPos;
	public RectTransform deckPos;

    public int maxCardsInHand = 6;
    public int reputation = 30;
    public int corruption = 0;
    public int sexisme = 0;

    public List<Card> deck;
    public List<Card> hand;
    public List<Card> board;

	public Board boardUI;

	public static int cardWidth = 110;
	// Use this for initialization

    protected override void init(){
		base.init();
        TargetType = TargetType.Player;
    }


    public void OnTurnStart()
    {
        Draw();
		
        foreach(Card c in board){
            //c.selectable.interactable = true;
			c.OnTurnStart();
        }
		/*foreach (Card c in hand){
           // c.selectable.interactable = true;
        }*/
		ResetOutlines(GameManager.instance.offlineMode || GameManager.instance.localPlayerTurn);
	    //boardUI.selectable.interactable = true;
    }

    public void OnTurnEnd()
    {
		ResetOutlines(false);
        foreach (Card c in board){
            //c.selectable.interactable = false;
			c.OnTurnEnd();
        }
        /*foreach (Card c in hand){
            //c.selectable.interactable = false;
        }*/
		//boardUI.selectable.interactable = false;
    }

	public void MoveToBoard(Card c) {
		photonView.RPC("MoveToBoardRPC", PhotonTargets.AllBuffered, c.photonView.viewID);
	}

	[RPC]
	void MoveToBoardRPC(int viewID) {
		var c = PhotonView.Find(viewID).GetComponent<Card>();

		if (!hand.Contains(c))
			return;

		hand.Remove(c);
		board.Add(c);
		c.place = Place.Board;
		c.effect.OnPlacedOnBoard();
	}

    public void Draw(int times = 1) {
        if (times < 1) {
            return;
        }
        if (deck.Count == 0)
            ReduceReputation(times);
        else{
            if (hand.Count < maxCardsInHand) {
                Card d = deck[0];
                deck.RemoveAt(0);
                d.show();
                hand.Add(d);
                d.place = Place.Hand;
				d.effect.OnDraw();
                Draw(times - 1);
            }
        }
    }
    void Update() {
        DisplayDeck();
        DisplayHand();
        DisplayBoard();
    }

	public void OutlinePossibleTargets(Card c) {

		Player p2 = GameManager.instance.getOtherPlayer(this);

		Outline outline;
		Color col;
		foreach (var card in board) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = c.isValidTarget(card) ? 255 : 0;
			outline.effectColor = col;
		}
		foreach (var card in p2.board) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = c.isValidTarget(card) ? 255 : 0;
			outline.effectColor = col;
		}
		foreach (var card in hand) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = 0;
			outline.effectColor = col;
		}
		

		
		outline = GetComponent<Outline>();
		col = outline.effectColor;
		col.a = c.isValidTarget(this) ? 255 : 0;
		outline.effectColor = col;
		
		outline = p2.GetComponent<Outline>();
		col = outline.effectColor;
		col.a = c.isValidTarget(p2) ? 255 : 0;
		outline.effectColor = col;

	}

	public void ResetOutlines(bool outlinePossibilities) {
		Player p2 = GameManager.instance.getOtherPlayer(this);

		Outline outline;
		Color col;
		foreach (var card in board) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = ((CardActor) card).canAttack && outlinePossibilities ? 255 : 0;
			outline.effectColor = col;
		}
		foreach (var card in p2.board) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = 0;
			outline.effectColor = col;
		}
		foreach (var card in hand) {
			outline = card.GetComponent<Outline>();
			col = outline.effectColor;
			col.a = outlinePossibilities ? 255:0;
			outline.effectColor = col;
		}
		

		outline = GetComponent<Outline>();
		col = outline.effectColor;
		col.a = 0;
		outline.effectColor = col;

		outline = p2.GetComponent<Outline>();
		col = outline.effectColor;
		col.a = 0;
		outline.effectColor = col;
	}

    public void DisplayDeck()
    {
        foreach (var card in deck)
        {
			card.transform.SetParent(deckPos);
            card.transform.localPosition = new Vector3(0, 0, 0);
            card.hide();
        }
    }

    public void DisplayHand() {
        Vector3 pos = /*handPos.position +*/ new Vector3(-cardWidth * hand.Count / 2, 0, 0);
        var offset = new Vector3(cardWidth, 0, 0);
        foreach (var card in hand) {
			card.transform.localPosition = pos;
	        card.transform.SetParent(handPos);
            pos += offset;

	        if (GameManager.instance.offlineMode) {
		        if (GameManager.instance.activePlayer() == this)
					card.show();
				else
			        card.hide();
	        }else{
		        if (GameManager.instance.localPlayer == this) 
					card.show();
		        else {
					card.hide();
		        }
		        
	        }
        }
    }

    public void DisplayBoard() {
        Vector3 pos = boardPos.position + new Vector3(-cardWidth * board.Count / 2, 0, 0);
        var offset = new Vector3(cardWidth, 0, 0);
        foreach (var card in board) {
			card.transform.SetParent(GameObject.Find("Cards").transform);
            card.transform.position = pos;
            pos += 2*offset;
			card.show();
        }
    }

    public void ReduceReputation(int value)
    {
        reputation -= value;
		Shake();
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

    public void ShuffleDeck() {
        // TODO SHUFFLE DECK
    }


    public void IncreaseCorruption(int cost) {
        if (GameManager.instance.contextCard) {
            cost *= (int)GameManager.instance.contextCard.corruptionMultiplier;
        }
        corruption += cost;
    }
    public void IncreaseSexisme(int cost) {
        if (GameManager.instance.contextCard) {
            cost *= (int)GameManager.instance.contextCard.sexismeMultiplier;
        }
        sexisme += cost;
    }

    public void RemoveCard(Card c)
    {
        if (hand.Contains(c))
            hand.Remove(c);
        if (board.Contains(c))
            board.Remove(c);
        if (deck.Contains(c))
            deck.Remove(c);
    }
}
