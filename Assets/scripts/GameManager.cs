using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Player player1;
    public Player player2;

    public Card cardSelected;

    public CardContext contextCard;

	public RectTransform contextRect;

    public RectTransform yourTurnText;

	public Player localPlayer;

	public bool localPlayerTurn = true;

	public bool offlineMode = false;

	// Use this for initialization
	void Awake () {
        instance = this;
		Player.cardWidth = 110;
		PhotonNetwork.offlineMode = offlineMode;
		localPlayer = PhotonNetwork.isMasterClient ? player1 : player2;
		localPlayerTurn = localPlayer == player1;
	}

    void Update() {
	    if (contextCard) {
			contextCard.transform.SetParent(contextRect);
		    contextCard.transform.localPosition = Vector3.zero;
	    }
    }

    public void playerDied(Player deadPlayer){
        
    }

    public void EndTurn()
    {
		
		activePlayer().OnTurnEnd();

	    localPlayerTurn = !localPlayerTurn;

		if (cardSelected) {
			cardSelected.setSelected(false);
			cardSelected = null;
		}
		if (localPlayerTurn) {
		    DOTween.Sequence()
			    .Append(yourTurnText.DOMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce))
			    .AppendInterval(1.0f)
			    .Append(yourTurnText.DOMove(new Vector3(0f, (float) Screen.height*2f, 0f), 0f));
	    }
        activePlayer().OnTurnStart();
		/*if(localPlayerTurn)
			activePlayer().ResetOutlines(true);*/
    }

    public Player activePlayer()
    {
		return localPlayerTurn ? player1 : player2;
    }

    public Player getOtherPlayer(Player p){
        return p == player1 ? player2 : player1;
    }

    public void elementClicked(Target c){

		if (!localPlayerTurn && !offlineMode)
            return;

        if (cardSelected == null && c.TargetType == TargetType.Card) {
            Card ca = (Card)c;
            if (ca.owner == activePlayer() && ca != contextCard){
				if (ca.cardType == CardType.Actor && ca.place == Place.Board)
					if (!((CardActor)ca).canAttack)
						return;

                cardSelected = ca;
                cardSelected.setSelected(true);
				activePlayer().OutlinePossibleTargets(cardSelected);
            }
        }
        else if (cardSelected != null) { // A card is selected
            if (cardSelected == c) {
                cardSelected.setSelected(false);
                cardSelected = null;
				activePlayer().ResetOutlines(true);
            }
            else {
                bool result = false;
                switch (c.TargetType) {
	                case TargetType.Card: {
		                Card ca = (Card)c;
		                if (ca.place == Place.Board) {
			                result = cardSelected.isValidTarget(c);
		                } else if (ca.owner == activePlayer() && ca != contextCard) {
			                cardSelected.setSelected(false);
			                cardSelected = (Card)c;
			                cardSelected.setSelected(true);
			                activePlayer().OutlinePossibleTargets(cardSelected);
			                return;
		                }
	                }
		                break;
	                case TargetType.Player:
		                result = cardSelected.isValidTarget(c);
		                break;
	                case TargetType.Board:
		                if (cardSelected.place == Place.Hand && cardSelected.cardType == CardType.Actor) {
			                cardSelected.owner.MoveToBoard(cardSelected);
			                cardSelected.setSelected(false);
			                cardSelected = null;
			                activePlayer().ResetOutlines(true);
		                }
		                break;
					case TargetType.Context: {
						if (cardSelected.cardType == CardType.Context) {
							contextCard = (CardContext)cardSelected;
							cardSelected.owner.RemoveCard(cardSelected);
							
							cardSelected.setSelected(false);
							cardSelected = null;
							activePlayer().ResetOutlines(true);
						}
					}
		                break;
                }
				// If we have a valid target, we use the card
                if (result) {
                    cardSelected.useOn(c);
                    cardSelected.setSelected(false);
                    cardSelected = null;
					player1.ResetOutlines(true);
                }
            }
        }
        
    }

}
