using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Player player1;
    public Player player2;

    public Card cardSelected;

    public CardContext contextCard;

    public RectTransform yourTurnText;
    public int turn = 2;

	// Use this for initialization
	void Awake () {
        instance = this;
	}

    void Update() {
    }

    public void playerDied(Player deadPlayer){
        
    }

    public void endTurn()
    {
		
		activePlayer().OnTurnEnd();

        turn = turn == 1 ? 2 : 1;

		if (cardSelected) {
			cardSelected.setSelected(false);
			cardSelected = null;
		}
        if (turn == 1)
        {
            LeanTween.move(yourTurnText, new Vector3(0f, 0f, 0f), 1f);
            LeanTween.move(yourTurnText, new Vector3(0f, (float)Screen.height, 0f), 0.01f).setDelay(2f);
        }

        activePlayer().OnTurnStart();
    }

    public Player activePlayer()
    {
        return turn == 1 ? player1 : player2;
    }

    public Player getOtherPlayer(Player p){
        return p == player1 ? player2 : player1;
    }

    public void elementClicked(Target c){

        // If not our turn skip
        if (turn != 1)
            return;

        if (cardSelected == null && c.type == Type.Card) {
            Card ca = (Card)c;
            if (ca.owner == player1)
            {
				if (ca.cardType == CardType.Actor)
					if (!((CardActor)ca).canAttack)
						return;

                cardSelected = ca;
                cardSelected.setSelected(true);
            }
        }
        else if (cardSelected != null) { // A card is selected
            if (cardSelected == c) {
                cardSelected.setSelected(false);
                cardSelected = null;
            }
            else {

                bool result = false;
                if (c.type == Type.Card) {
                    Card ca = (Card)c;
                    if (ca.place == Place.Board) {
                        result = cardSelected.isValidTarget(c);
                    }
                    else if(ca.owner == player1) {
                        cardSelected.setSelected(false);
                        cardSelected = (Card)c;
                        cardSelected.setSelected(true);
                        return;
                    }
                }
                else if(c.type == Type.Player){
                    result = cardSelected.isValidTarget(c);
				} else { // Board
					if (cardSelected.place == Place.Hand && cardSelected.cardType == CardType.Actor) {
						cardSelected.owner.moveToBoard(cardSelected);
						cardSelected.setSelected(false);
						cardSelected = null;
					}
				}
                
                Debug.Log(result);
                if (result) {
                    cardSelected.useOn(c);
                    cardSelected.setSelected(false);
                    cardSelected = null;
                }
            }
        }
        
    }

}
