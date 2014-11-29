using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardActor : Card {

    public bool canAttack = false;

	// Use this for initialization

	
	public Text cardStats;

    protected override void init(){
        base.init();
        cardType = CardType.Actor;
    }

	void Update() {

		string attackText = attack < baseAttack ? "<color=maroon>" : "";
		attackText += attack;
		attackText += attack < baseAttack ? "</color>" : "";

		string reputationText = reputation < baseReputation ? "<color=maroon>" : "";
		reputationText += reputation;
		reputationText += reputation < baseReputation ? "</color>" : "";

		cardStats.text = attackText + "   " + reputationText;
	}

	[RPC]
	protected override void useOnRPC(int viewID) {

		Target c = PhotonView.Find(viewID).GetComponent<Target>();

		effect.OnAttackPerformed(c);

		
		var dir = (c.transform.position - transform.position) / 10f;
		DOTween.Sequence()
				.Append(transform.DOPunchPosition(dir, Mathf.Clamp(dir.sqrMagnitude / 100f, 0.5f, 1f), 0, 0));
		canAttack = false;
		//selectable.interactable = false;
	}

    public override bool isValidTarget(Target t) {

	    return effect.IsValidTarget(t);
    }

    public override void OnTurnStart() {
		if (place == Place.Board) {
			canAttack = true;
			effect.OnTurnStart();
		}
    }



}
