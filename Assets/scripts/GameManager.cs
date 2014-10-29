using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Player player1;
    public Player player2;

    public Card cardSelected;

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
            cardSelected.renderer.material.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else if (cardSelected != null) {
            if (cardSelected == c) {
                cardSelected.renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
                cardSelected = null;
            }
            else {

                bool result = false;
                if (c.type == Type.Card) {
                    Card ca = (Card)c;
                    if (ca.place == Place.Board) {
                        result = cardSelected.useOn(c);
                    }
                    else {
                        cardSelected.renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
                        cardSelected = (Card)c;
                        cardSelected.renderer.material.color = new Color(1.0f, 0.0f, 0.0f);
                        return;
                    }
                }
                else {
                    result = cardSelected.useOn(c);
                }
                
                Debug.Log(result);
                if (result) {
                    cardSelected.renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
                    cardSelected = null;
                }
            }
        }
        
    }

}
