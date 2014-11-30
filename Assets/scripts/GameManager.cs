using System;
using System.Linq;
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

	public Text startGameText;



	// Use this for initialization
	void Awake () {
        instance = this;
		photonView = GetComponent<PhotonView>();
		Player.cardWidth = 110;
		PhotonNetwork.offlineMode = offlineMode;
	    if (offlineMode) {
	        //PhotonNetwork.JoinLobby();
	        PhotonNetwork.JoinOrCreateRoom("localRoom", new RoomOptions {maxPlayers = 2}, TypedLobby.Default);
	    }
		localPlayer = PhotonNetwork.isMasterClient ? player1 : player2;
		localPlayerTurn = localPlayer == player1;

		buttonEndTurn.interactable = false;

		
		
	}

	void Start() {
		StartCoroutine(StartGame());
	}

	void InstantiateDecks() {

		// TODO

		if (localPlayer == player1 || offlineMode) {
			foreach (ActionDB.rowIds c in Enum.GetValues(typeof(ActionDB.rowIds))) {
				CardFactory.Instance.CreateAction(c, PlayerID.Player1);
			}

		}
		if(localPlayer == player2 || offlineMode){
			CardFactory.Instance.CreateActor(ActorDB.rowIds.ACTOR_ANITASARKEESIAN, PlayerID.Player2);
			CardFactory.Instance.CreateActor(ActorDB.rowIds.ACTOR_ANITASARKEESIAN, PlayerID.Player2);
			CardFactory.Instance.CreateActor(ActorDB.rowIds.ACTOR_ANITASARKEESIAN, PlayerID.Player2);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_CONFESSION, PlayerID.Player2);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_REMISEDEPRIX, PlayerID.Player2);
			CardFactory.Instance.CreateActor(ActorDB.rowIds.ACTOR_ANITASARKEESIAN, PlayerID.Player2);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_CONFESSION, PlayerID.Player2);
			CardFactory.Instance.CreateAction(ActionDB.rowIds.ACTION_REMISEDEPRIX, PlayerID.Player2);

		}		

	}
	IEnumerator StartGame() {

		if(!offlineMode) 
			startGameText.text = PhotonNetwork.playerList[0].name + " vs " + PhotonNetwork.playerList[1].name;

		startGameText.transform.DOLocalMove(Vector3.zero, 2f).SetEase(Ease.OutElastic);

		InstantiateDecks();

		yield return new WaitForSeconds(3f);

		startGameText.transform.DOLocalMove(new Vector3(1920,0,0), 1f).SetEase(Ease.InCirc);
		

		if (PhotonNetwork.isMasterClient) {
			activePlayer().DrawRPC(2);
			getOtherPlayer(activePlayer()).DrawRPC(3);
		}

		yield return new WaitForSeconds(1f);

		if (localPlayerTurn) {
			DOTween.Sequence()
				.Append(yourTurnText.DOMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce))
				.AppendInterval(1.0f)
				.Append(yourTurnText.DOMove(new Vector3(0f, (float)Screen.height * 2f, 0f), 0f));
			buttonEndTurn.interactable = true;
		}

		activePlayer().OnTurnStart();
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

		if (contextCard) {
			contextCard.destroy();
		}

		contextCard = PhotonView.Find(viewID).GetComponent<CardContext>();
		contextCard.owner.RemoveCard(contextCard);
		contextCard.show();

		contextCard.TargetType = TargetType.Context;
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
			buttonEndTurn.interactable = offlineMode;
		
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
	                case TargetType.Card: { // We clicked on another card
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
					activePlayer().ResetOutlines(true);
                }
            }
        }
        
    }

}
