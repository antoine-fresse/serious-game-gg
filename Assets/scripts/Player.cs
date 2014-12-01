using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Player : Target {

    public RectTransform handPos;
    public RectTransform boardPos;
	public RectTransform deckPos;

	private const int MaxCardsInHand = 8;
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
		c.owner.corruption += c.corruptionCost;
		c.owner.sexisme += c.sexismeCost;
		c.place = Place.Board;
		c.effect.OnPlacedOnBoard();
	}

	public void MoveToHand(CardActor actor) {
		if (!board.Contains(actor))
			return;

		if (hand.Count >= MaxCardsInHand) {
			actor.destroy();
			return;
		}
		board.Remove(actor);
		hand.Add(actor);
		actor.place = Place.Hand;
		actor.attack = actor.baseAttack;
		actor.reputation = actor.baseReputation;
		actor.canAttack = false;
		actor.preventAttack = false;


	}

	public void DrawRPC(int times = 1) {
		photonView.RPC("Draw", PhotonTargets.AllBuffered, times);
	}

	[RPC]
    public void Draw(int times = 1) {

		
        if (times < 1) {
            return;
        }
        if (deck.Count == 0)
			ChangeReputation(-times);
        else{
            if (hand.Count < MaxCardsInHand) {

				if (this != GameManager.instance.localPlayer && !GameManager.instance.offlineMode)
					return;

                Card card = deck[Random.Range(0, deck.Count-1)];
				deck.Remove(card);
				hand.Add(card);
				card.place = Place.Hand;
				card.effect.OnDraw();
	            card.show();
				photonView.RPC("HasDrawn", PhotonTargets.OthersBuffered, card.photonView.viewID);

                Draw(times - 1);
            }
        }
    }

	[RPC]
	void HasDrawn(int viewId) {
		var card = PhotonView.Find(viewId).GetComponent<Card>();
		deck.Remove(card);
		hand.Add(card);
		card.place = Place.Hand;
		card.effect.OnDraw();
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


		outline = GameObject.Find("Context").GetComponent<Outline>();
		col = outline.effectColor;
		col.a = c.cardType == CardType.Context ? 255 : 0;
		outline.effectColor = col;
		
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

		outline = GameObject.Find("Context").GetComponent<Outline>();
		col = outline.effectColor;
		col.a = 0;
		outline.effectColor = col;
		

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
					card.show();
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
            pos += 3*offset;
			card.show();
        }
    }

	public void ChangeReputation(int value)
    {
        reputation = Mathf.Clamp(reputation + value,0,30);

		if(value < 0)
			Shake();

        if (reputation == 0)
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

	public void Discard(int nb) {

		if (this != GameManager.instance.localPlayer && !GameManager.instance.offlineMode)
			return;

		if (nb > hand.Count)
			nb = hand.Count;

		for (var i = 0; i < nb; i++) {
			var n = Random.Range(0, hand.Count - 1);
			photonView.RPC("DiscardRPC", PhotonTargets.AllBuffered, hand[n].photonView.viewID);
		}


	}
	[RPC]
	void DiscardRPC(int viewId) {
		PhotonView.Find(viewId).GetComponent<Card>().destroy();
	}
}
