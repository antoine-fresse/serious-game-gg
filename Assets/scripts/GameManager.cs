using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Player player1;
    public Player player2;

    public Card cardSelected;

    public CardContext contextCard;

	// Use this for initialization
	void Awake () {
        instance = this;
	}

    void Update() {
    }

    public void playerDied(Player deadPlayer){
        
    }

    public Player getOtherPlayer(Player p){
        return p == player1 ? player2 : player1;
    }

    public void elementClicked(Target c){
        if (cardSelected == null && c.type == Type.Card) {
            cardSelected = (Card)c;
            cardSelected.setSelected(true);
        }
        else if (cardSelected != null) {
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
                    else {
                        cardSelected.setSelected(false);
                        cardSelected = (Card)c;
                        cardSelected.setSelected(true);
                        return;
                    }
                }
                else {
                    result = cardSelected.isValidTarget(c);
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
