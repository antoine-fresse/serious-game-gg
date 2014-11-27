using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using GoogleFu;
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

	public PhotonView photonView;

	public Button buttonEndTurn;


	// Use this for initialization
	void Awake () {
        instance = this;
		photonView = GetComponent<PhotonView>();
		Player.cardWidth = 110;
		PhotonNetwork.offlineMode = offlineMode;
		localPlayer = PhotonNetwork.isMasterClient ? player1 : player2;
		localPlayerTurn = localPlayer == player1;

		buttonEndTurn.interactable = localPlayerTurn;

	}

	void Start() {
		if (PhotonNetwork.isMasterClient) {
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_KICKSTARTER, PlayerID.Player1);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_GAMERSAREDEAD, PlayerID.Player1);
		}
		else {
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_CONFESSION, PlayerID.Player2);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_REMISEDEPRIX, PlayerID.Player2);
		}
	}

    void Update() {
	    if (contextCard) {
			contextCard.transform.SetParent(contextRect);
		    contextCard.transform.localPosition = Vector3.zero;
	    }
    }

    public void playerDied(Player deadPlayer){
        
    }

	[RPC]
	void SetContext(int viewID) {
		contextCard = PhotonView.Find(viewID).GetComponent<CardContext>();
		contextCard.owner.RemoveCard(contextCard);
		contextCard.show();
	}

	public void EndTurn() {
		photonView.RPC("EndTurnRPC", PhotonTargets.AllBuffered);
	}
	[RPC]
    void EndTurnRPC()
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
			buttonEndTurn.interactable = true;
		}
		else 
			buttonEndTurn.interactable = false;
		
        activePlayer().OnTurnStart();
    }

    public Player activePlayer()
    {
		return localPlayerTurn ? localPlayer : getOtherPlayer(localPlayer);
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
							photonView.RPC("SetContext", PhotonTargets.AllBuffered, cardSelected.photonView.viewID);
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
