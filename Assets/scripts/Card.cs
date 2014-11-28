using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections;
using DG.Tweening;

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

public enum PlayerID {
	Player1,
	Player2
}


public abstract class Card : Target {

    public string description;
	public string effectDesc;

	public Text cardName;
	public Text cardDesc;
	public Text cardCost;
	public Text cardEffect;

	[HideInInspector]
    public int attack = 1;
	[HideInInspector]
    public int reputation = 1;

	public int baseAttack = 1;
	public int baseReputation = 1;

    public int corruptionCost = 0;
    public int sexismeCost = 0;

    public Sprite spriteNormal;
    public Sprite spriteSelected;
    public Sprite spriteHidden;

    public bool hidden = false;

	public PlayerID ownerID;

	

	public Player owner {
		get { return ownerID == PlayerID.Player1 ? GameManager.instance.player1 : GameManager.instance.player2; }
	}

	public Place place = Place.Deck;
    public CardType cardType = CardType.Action;


	public CardEffectInterface effect;

	public void useOn(Target c) {
		photonView.RPC("useOnRPC", PhotonTargets.AllBuffered, c.photonView.viewID);
	}

	[RPC]
	protected abstract void useOnRPC(int viewID);

    public abstract bool isValidTarget(Target c);
    public void ReduceReputation(int value) {
        reputation -= value;
		Shake();
        if (reputation <= 0) {
			destroy();
        }
    }

	public virtual void OnTurnStart() { effect.OnTurnStart(); }

	public virtual void OnTurnEnd() { effect.OnTurnEnd(); }

    protected override void init()
    {
		base.init();	    
        TargetType = TargetType.Card;
	    attack = baseAttack;
	    reputation = baseReputation;
		cardName.text = fullName;

	    cardDesc.text = description;

		cardEffect.text = Regex.Replace(effectDesc, "%ATTACK%", attack.ToString(), RegexOptions.IgnoreCase);

		switch (place) {
			case Place.Board:
				if (!owner.board.Contains(this)) {
					owner.board.Add(this);
				}

				break;
			case Place.Deck:
				if (!owner.deck.Contains(this)) {
					owner.deck.Add(this);
				}


				break;
			case Place.Hand:
				if (!owner.hand.Contains(this)) {
					owner.hand.Add(this);
				}
				break;
		}

	    var costText = "<color=maroon>" + corruptionCost + "</color>";
	    costText += "\n";
		costText += "<color=#0080ffff>" + sexismeCost + "</color>";

	    cardCost.text = costText;


		effect = GetComponent<CardEffectInterface>();
		effect.OnInit();
    }

    public void setSelected(bool isSelected)
    {
        if (isSelected)
        {
            gameObject.GetComponent<Image>().sprite = spriteSelected;
	        transform.DOScale(new Vector3(1.2f, 1.2f,1f), 0.2f);
			effect.OnSelected();
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = spriteNormal;
			transform.DOScale(new Vector3(1.0f, 1.0f, 1f), 0.2f);
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
        owner.RemoveCard(this);
        place = Place.Graveyard;


		transform.SetParent(GameObject.Find("Cards").transform);
	    DOTween.Sequence()
		    .Append(transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 1.0f))
			.Append(transform.DOMove(new Vector3(10000f,0f), 0f));



    }
}

