using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardDisplay : MonoBehaviour {


    public Text cardName;
    public Text cardDesc;
    public Text cardCost;
    public Card card;
    
	// Use this for initialization
	void Start () {
        card = GetComponent<Card>();

        cardName.text = card.fullName;

        cardDesc.text = card.description;
        string cost = "";
        if (card.corruptionCost > 0 || card.sexismeCost > 0)
        {
            cost += "Cout : ";
            if (card.corruptionCost > 0)
                cost += card.corruptionCost + " corruption";
            if (card.sexismeCost > 0)
                cost += card.sexismeCost + " sexisme";
        }

        cardCost.text = cost;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
