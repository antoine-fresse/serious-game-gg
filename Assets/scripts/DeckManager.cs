using System.Collections.Generic;
using System.Linq;
using GoogleFu;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour {

	public RectTransform content;
	public Text list;

	public static DeckManager Instance { get; private set; }

	public Dictionary<string, int> deck;
	public int nbCardsInDeck = 0;

	private const int MaxOfOneCard = 3;
	// Use this for initialization
	void Awake () {
		Instance = this;
		deck = new Dictionary<string, int>();

		if (PlayerPrefs.HasKey("deck")) {
			for (var i = 0; i < PlayerPrefs.GetInt("deck"); i++) {

				var id = PlayerPrefs.GetString("deck_card_id_" + i);
				var nb = PlayerPrefs.GetInt("deck_card_nb_" + i);
				deck.Add(id,nb);
				nbCardsInDeck += nb;
			}
		}

		if (!list) return;
		var newList = "Deck(" + nbCardsInDeck + ")\n";
		newList += deck.Keys.Aggregate("", (current, k) => current + ("\n" + GetNameFromId(k) + " : " + deck[k]));
		list.text = newList;
	}

	void Start() {

		foreach (var c in ActionDB.Instance.rowNames) {
			CardFactory.Instance.CreateActionRPC(c, (int)PlayerID.Player1, PhotonNetwork.AllocateViewID());
		}
		foreach (var c in ActorDB.Instance.rowNames) {
			CardFactory.Instance.CreateActorRPC(c, (int)PlayerID.Player1, PhotonNetwork.AllocateViewID());
		}
		foreach (var c in TrendingDB.Instance.rowNames) {
			CardFactory.Instance.CreateContextRPC(c, (int)PlayerID.Player1, PhotonNetwork.AllocateViewID());
		}

		//GameObject.Find("DeckEditor").SetActive(false);
	}

	public void SaveDeck() {
		int i = 0;
		PlayerPrefs.DeleteAll();
		foreach (var k in deck.Keys) {

			PlayerPrefs.SetString("deck_card_id_" + i, k);
			PlayerPrefs.SetInt("deck_card_nb_" + i, deck[k]);

			i++;
		}
		
		PlayerPrefs.SetInt("deck", i);

		Debug.Log("Deck saved");
	}

	public void ClearDeck() {
		deck = new Dictionary<string, int>();
		nbCardsInDeck = 0;
		if (!list) return;
		var newList = "Deck(" + nbCardsInDeck + ")\n";
		newList += deck.Keys.Aggregate("", (current, k) => current + ("\n" + GetNameFromId(k) + " : " + deck[k]));
		list.text = newList;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick(PointerEventData evt, Card card) {
		if (evt.button == PointerEventData.InputButton.Left) {

			if (nbCardsInDeck >= 20)
				return;

			if (deck.ContainsKey(card.id)) {
				if (deck[card.id] < MaxOfOneCard) {
					deck[card.id] = deck[card.id] + 1;
					nbCardsInDeck++;
				}
			}
			else {
				deck.Add(card.id, 1);
				nbCardsInDeck++;
			}
		} else if (evt.button == PointerEventData.InputButton.Right) {
			if (deck.ContainsKey(card.id)) {
				deck[card.id] = deck[card.id] - 1;
				nbCardsInDeck--;
				if (deck[card.id] == 0) {
					deck.Remove(card.id);
				}
			}
		}
		if (!list) return;
		var newList = "Deck(" + nbCardsInDeck + ")\n";
		newList += deck.Keys.Aggregate("", (current, k) => current + ("\n" + GetNameFromId(k) + " : " + deck[k]));
		list.text = newList;
	}

	public string GetNameFromId(string id) {

		if (id.StartsWith("ACTION")) {
			return ActionDB.Instance.GetRow(id)._NAME;
		}
		if (id.StartsWith("ACTOR")) {
			return ActorDB.Instance.GetRow(id)._NAME;
		}
		if (id.StartsWith("TREND")) {
			return TrendingDB.Instance.GetRow(id)._NAME;
		}

		return "NOT_FOUND";
	}
}
