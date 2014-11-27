using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private Text _textUI;

	public InputField Pseudo;

	public Button ButtonRoom;

	public GameObject PanelRoom;

	private bool _ready = false;

	private Text _p1;
	private Text _p2;

	private Button _launchButton;

	void Awake() {
		_textUI = GetComponent<Text>();


		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.ConnectUsingSettings("0.1f");
		_textUI.text = "Game server : <color=orange>Connecting</color>";

		if(PlayerPrefs.HasKey("pseudo")) {
			Pseudo.text = PlayerPrefs.GetString("pseudo");
		}

		_p1 = PanelRoom.transform.FindChild("Player1").GetComponent<Text>();
		_p2 = PanelRoom.transform.FindChild("Player2").GetComponent<Text>();
		_launchButton = PanelRoom.transform.FindChild("Launch").GetComponent<Button>();
	}


	void OnPhotonPlayerConnected(PhotonPlayer player) {
		UpdateRoom();
	}

	void OnPhotonPlayerDisconnected() {
		UpdateRoom();
	}

	void OnJoinedLobby() {
		_textUI.text = "Game server : <color=green>Connected</color>";
		_ready = true;
		ButtonRoom.interactable = true;
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause) {
		_textUI.text = "Game server : <color=maroon>Could not connect, retrying</color>";
		PhotonNetwork.ConnectUsingSettings("0.1f");
	}

	void OnDisconnectedFromPhoton() {
		_ready = false;
		ButtonRoom.interactable = false;
		_textUI.text = "Game server : <color=maroon>Could not connect, retrying</color>";
		PhotonNetwork.ConnectUsingSettings("0.1f");
	}

	public void SavePseudo() {
		if (Pseudo.text == "") {
			Pseudo.text = "Anonymous";
		}
		PlayerPrefs.SetString("pseudo", Pseudo.text);
		PhotonNetwork.playerName = Pseudo.text;
	}

	public void OnButtonClick() {
		if (!_ready) return;

		if (PhotonNetwork.inRoom) {
			ButtonRoom.interactable = false;
			PhotonNetwork.LeaveRoom();
			return;
		}
		
		var rooms = PhotonNetwork.GetRoomList();

		if (rooms.Length == 0) {
			PhotonNetwork.CreateRoom("room1", new RoomOptions {maxPlayers = 2, isOpen = true, isVisible = true}, TypedLobby.Default);
		}
		else {
			ButtonRoom.interactable = false;
			PhotonNetwork.JoinRoom(rooms[0].name);
		}

	}

	void OnLeftRoom() {
		ButtonRoom.GetComponentInChildren<Text>().text = "Find random opponent";
		ButtonRoom.interactable = true;
		Pseudo.interactable = true;

		PanelRoom.SetActive(false);
	}


	void OnJoinedRoom() {
		PhotonNetwork.playerName = Pseudo.text;
		Pseudo.interactable = false;
		ButtonRoom.interactable = true;
		ButtonRoom.GetComponentInChildren<Text>().text = "Leave room";

		PanelRoom.SetActive(true);
		UpdateRoom();
	}

	void UpdateRoom() {
		var players = PhotonNetwork.playerList;

		if (players.Length == 2) {
			_p1.text = PhotonNetwork.isMasterClient ? players[0].name : players[1].name;
			_p2.text = PhotonNetwork.isMasterClient ? players[1].name : players[0].name;
		} else {
			_p1.text = PhotonNetwork.isMasterClient ? players[0].name : "Waiting for another player...";
			_p2.text = PhotonNetwork.isMasterClient ? "Waiting for another player..." : players[0].name;
		}

		_launchButton.interactable = PhotonNetwork.isMasterClient && players.Length == 2;
	}

	public void LaunchGame() {
		if (!PhotonNetwork.isMasterClient) return; // Should never happen

		PhotonNetwork.LoadLevel(1);
	}
}
