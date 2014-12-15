using UnityEngine;
using UnityEngine.EventSystems;
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

	public bool InAnimation = false;

	public string id;



	public Player owner {
		get { return ownerID == PlayerID.Player1 ? GameManager.instance.player1 : GameManager.instance.player2; }
	}

	public Place place = Place.Deck;
    public CardType cardType = CardType.Action;


	public AbstractCardEffect effect;

	public void useOn(Target c) {
		photonView.RPC("useOnRPC", PhotonTargets.AllBuffered, c.photonView.viewID);
	}

	[RPC]
	protected abstract void useOnRPC(int viewID);

    public abstract bool isValidTarget(Target c);
    public void ChangeReputation(int value) {
        reputation += value;
		var color = "green";
	    if (value < 0) {
		    Shake();
		    color = "maroon";
	    }

	    var go = (GameObject)Instantiate(Resources.Load("FloatingText"), transform.position, Quaternion.identity);
		go.GetComponent<Text>().text = "<color=" + color + ">" + value + "</color>";

        if (reputation <= 0) {
	        reputation = 0;
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

		cardEffect.text = Regex.Replace(effectDesc, "%ATTACK%", "<b>"+attack.ToString()+"</b>", RegexOptions.IgnoreCase);
		cardEffect.text = Regex.Replace(cardEffect.text, "%REPUTATION%", "<b>" + reputation.ToString() + "</b>", RegexOptions.IgnoreCase);
	    if (GameManager.instance) {
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
	    }
	    var costText = "<color=maroon>" + corruptionCost + "</color>";
	    costText += "\n";
		costText += "<color=#0080ffff>" + sexismeCost + "</color>";

	    cardCost.text = costText;


		effect = GetComponent<AbstractCardEffect>();
		effect.OnInit();
    }

    public void setSelected(bool isSelected)
    {
        if (isSelected)
        {
			SoundManager.Instance.PlayCardShove();
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

    public void destroy(bool suicide = false)
    {
		if (place == Place.Board) {
			effect.OnDeath();
		}
	    show();
        owner.RemoveCard(this);
        place = Place.Graveyard;
	    TargetType = TargetType.Graveyard;

		transform.SetParent(owner.graveyardPos);
	    


	    if (cardType == CardType.Action && !suicide) {
		    transform.DOMove(GameObject.Find("Cards").transform.position, 0.5f);
			transform.DOMove(owner.graveyardPos.transform.position, 1.5f).SetDelay(1.5f).SetEase(Ease.OutCubic);


		    transform.DOScale(new Vector3(1.5f, 1.5f, 1.0f), 0.5f);
		    transform.DOScale(Vector3.one, 1.0f).SetDelay(1.5f);
	    }
	    else {
		    var cross = (RectTransform)Instantiate(Resources.Load<RectTransform>("Cross"));
			cross.SetParent(transform, false);

		    //cross.anchoredPosition = Vector2.zero;
			
		    cross.localScale = Vector3.one;

			transform.DOMove(owner.graveyardPos.transform.position, 1.5f).SetDelay(1f).SetEase(Ease.OutCubic);
	    }
	    var outline = GetComponent<Outline>();
	    var color = outline.effectColor;
	    color.a = 0;
	    outline.effectColor = color;

	    //GetComponent<EventTrigger>().enabled = false;


    }

	public void OnCursorEnter() {

		if (place == Place.Deck || place == Place.Graveyard) return;
		transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);


		owner.HoveredCard = this;

	}

	public void OnCursorExit() {
		
		//if(owner.HoveredCard == this)
		owner.HoveredCard = null;

		if(GameManager.instance.cardSelected != this)
			transform.DOScale(Vector3.one, 0.2f);
	}
}

