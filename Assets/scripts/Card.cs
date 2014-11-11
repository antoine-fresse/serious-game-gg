using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CardType
{
    Action,
    Actor,
    Context
}

public enum Place {
    Deck,
    Hand,
    Board,
    Graveyard
}

[RequireComponent(typeof(CardEffectInterface))]
public abstract class Card : Target {

    public string description;

    public int attack = 1;
    public int reputation = 1;

    public int corruptionCost = 0;
    public int sexismeCost = 0;

    public Sprite spriteNormal;
    public Sprite spriteSelected;
    public Sprite spriteHidden;

    public bool hidden = false;
    public Player owner;

    public Place place = Place.Deck;
    public CardType cardType = CardType.Action;


	public CardEffectInterface effect;

    public abstract void useOn(Target c);
    public abstract bool isValidTarget(Target c);
    public void reduceReputation(int value) {
        reputation -= value;
        if (reputation <= 0) {
            Debug.Log("Card is ded yo ");
        }
    }

	public virtual void OnTurnStart() { effect.OnTurnStart(); }

	public virtual void OnTurnEnd() { effect.OnTurnEnd(); }

    protected override void init()
    {
		base.init();
        type = Type.Card;
		effect = GetComponent<CardEffectInterface>();
		effect.OnInit();
    }

    public void setSelected(bool isSelected)
    {
        if (isSelected)
        {
            gameObject.GetComponent<Image>().sprite = spriteSelected;
			effect.OnSelected();
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = spriteNormal;
			effect.OnDeselected();
        }
    }

    public void hide()
    {
        hidden = true;
        gameObject.GetComponent<Image>().sprite = spriteHidden;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

    }
    public void show()
    {
        hidden = false;
        gameObject.GetComponent<Image>().sprite = spriteNormal;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void destroy()
    {
		if (place == Place.Board) {
			effect.OnDeath();
		}
        owner.removeCard(this);
        place = Place.Graveyard;
        gameObject.transform.position = new Vector3(0, 10000, 0);
    }
}

