using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public enum TargetType {
    Player,
    Card,
    Board,
	Context
}
[RequireComponent(typeof(Selectable))]
public abstract class Target : MonoBehaviour {

    public TargetType TargetType;
    public string fullName;
    public Selectable selectable;
	public PhotonView photonView;

	public void Awake() {
		photonView = GetComponent<PhotonView>();
		
	}
    public void Start()
    {
		init();
    }

	protected virtual void init() {
		selectable = GetComponent<Selectable>();
	}

    public void OnClick(BaseEventData evt)
    {

		if(GameManager.instance)
			GameManager.instance.elementClicked(this);

	    if (DeckManager.Instance && TargetType == TargetType.Card)
		    DeckManager.Instance.OnClick((PointerEventData)evt, (Card) this );
    }

	public void Shake() {
		transform.DOShakePosition(0.5f, 15f);
	}

	
}