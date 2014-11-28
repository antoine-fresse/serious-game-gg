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

		if (c.TargetType == TargetType.Player) {
			Player p = (Player)c;
			p.ReduceReputation(attack);
			effect.OnAttackPerformed(c);


		} else {
			Card ca = (Card)c;
			ca.ReduceReputation(attack);
			effect.OnAttackPerformed(c);
			ca.effect.OnAttackReceived(this);
		}
		var dir = (c.transform.position - transform.position) / 10f;
		DOTween.Sequence()
				.Append(transform.DOPunchPosition(dir, Mathf.Clamp(dir.sqrMagnitude / 100f, 0.5f, 1f), 0, 0));
		canAttack = false;
		//selectable.interactable = false;
	}

    public override bool isValidTarget(Target t) {

		if (!canAttack || place != Place.Board || t == this)
			return false;

		if (t.TargetType == TargetType.Player) {
			return owner != (Player)t;
		}

		Card ca = (Card)t;

		if (ca.place == Place.Board && ca.owner != owner) {
			return true;
		}

        return true;
    }

    public override void OnTurnStart() {
		if (place == Place.Board) {
			canAttack = true;
			effect.OnTurnStart();
		}
    }



}
