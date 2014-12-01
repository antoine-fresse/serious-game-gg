using DG.Tweening;
using GoogleFu;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardActor : Card {

    public bool canAttack = false;

	// Use this for initialization

	public bool preventAttack = false;

	public int custom_param = 0;

	public Text cardStats;

    protected override void init(){
        base.init();
        cardType = CardType.Actor;
    }

	void Update() {

		string attackText = attack < baseAttack ? "<color=maroon>" : (attack > baseAttack ? "<color=olive>" : "");
		attackText += attack;
		attackText += attack < baseAttack || attack > baseAttack ? "</color>" : "";

		string reputationText = reputation < baseReputation ? "<color=maroon>" : (reputation > baseReputation ? "<color=olive>" : "");
		reputationText += reputation;
		reputationText += (reputation < baseReputation || reputation > baseReputation) ? "</color>" : "";

		cardStats.text = attackText + (attack < 10 ? " " : "") + " " + (reputation < 10 ? " " : "") + reputationText;
	}

	[RPC]
	protected override void useOnRPC(int viewID) {

		Target c = PhotonView.Find(viewID).GetComponent<Target>();

		effect.OnAttackPerformed(c, attack);

		
		var dir = (c.transform.position - transform.position) / 10f;
		transform.DOPunchPosition(dir, Mathf.Clamp(dir.sqrMagnitude / 100f, 0.5f, 1f), 0, 0);
		canAttack = false;
		//selectable.interactable = false;
	}

    public override bool isValidTarget(Target t) {

		return effect.IsValidTarget(t);
    }

    public override void OnTurnStart() {
		if (place == Place.Board) {
			canAttack = !preventAttack;
			preventAttack = false;
			effect.OnTurnStart();
		}
    }




}
