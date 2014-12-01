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

	public bool gameEnded = false;


	// Use this for initialization
	void Awake () {
        instance = this;
		photonView = GetComponent<PhotonView>();
		PhotonNetwork.offlineMode = offlineMode;
	    if (offlineMode) {
	        //PhotonNetwork.JoinLobby();
	        PhotonNetwork.JoinOrCreateRoom("localRoom", new RoomOptions {maxPlayers = 2}, TypedLobby.Default);
	    }
		localPlayer = PhotonNetwork.isMasterClient ? player1 : player2;

		localPlayer.fullName = PhotonNetwork.playerName;
		if(!offlineMode)
			getOtherPlayer(localPlayer).fullName = PhotonNetwork.otherPlayers[0].name;
		


		localPlayerTurn = localPlayer == player1;

		if (!localPlayerTurn)
			localPlayer.Swap();
		if(offlineMode)
			player2.Swap();

		buttonEndTurn.interactable = false;

		
		
	}

	void Start() {
		StartCoroutine(StartGame());
	}

	public void LeaveGame() {
		Application.Quit();
	}

	void OnPhotonPlayerDisconnected() {
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}

	void InstantiateDecks() {

		// TODO


		if (!PlayerPrefs.HasKey("deck")) return;

		var playerid = localPlayer == player1 ? PlayerID.Player1 : PlayerID.Player2;

		for (var i = 0; i < PlayerPrefs.GetInt("deck"); i++) {

			var id = PlayerPrefs.GetString("deck_card_id_" + i);
			var nb = PlayerPrefs.GetInt("deck_card_nb_" + i);


			for (int j = 0; j < nb; j++) {
				if(id.StartsWith("ACTION"))
					CardFactory.Instance.CreateAction(id, playerid);
				else if(id.StartsWith("ACTOR"))
					CardFactory.Instance.CreateActor(id, playerid);
				else if(id.StartsWith("TREND"))
					CardFactory.Instance.CreateContext(id, playerid);
			}
		}


		if (!offlineMode) return;
		playerid = localPlayer == player1 ? PlayerID.Player2 : PlayerID.Player1;

		for (var i = 0; i < PlayerPrefs.GetInt("deck"); i++) {

			var id = PlayerPrefs.GetString("deck_card_id_" + i);
			var nb = PlayerPrefs.GetInt("deck_card_nb_" + i);


			for (int j = 0; j < nb; j++) {
				if (id.StartsWith("ACTION"))
					CardFactory.Instance.CreateAction(id, playerid);
				else if (id.StartsWith("ACTOR"))
					CardFactory.Instance.CreateActor(id, playerid);
				else if (id.StartsWith("TREND"))
					CardFactory.Instance.CreateContext(id, playerid);
			}
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
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce))
				.AppendInterval(1.0f)
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, (float)Screen.height * 2f, 0f), 0f));
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

    public void playerDied(Player deadPlayer) {
	    gameEnded = true;

	    if (deadPlayer == localPlayer) {
		    yourTurnText.GetComponent<Text>().text = "Vous avez perdu !";
		    DOTween.Sequence()
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce));
	    }

	    else {
			yourTurnText.GetComponent<Text>().text = "Victoire !";
			DOTween.Sequence()
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce));
	    }
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
		if (!gameEnded)
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
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.OutBounce))
			    .AppendInterval(1.0f)
				.Append(yourTurnText.DOLocalMove(new Vector3(0f, (float)Screen.height * 2f, 0f), 0f));
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

	    if (gameEnded)
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
