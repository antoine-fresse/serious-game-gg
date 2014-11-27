using UnityEngine;
using System.Collections;
using GoogleFu;

public class CardFactory : MonoBehaviour {

	public GameObject prefabAction;
	public GameObject prefabActor;
	public GameObject prefabContext;


	public static CardFactory Instance;
	private ActionDB _actionDB;

	public PhotonView photonView;
	// Use this for initialization
	void Awake () {
		photonView = GetComponent<PhotonView>();
		_actionDB = ActionDB.Instance;
		Instance = this;
		
	}


	public void CreateAction(ActionDB.rowIds type, PlayerID playerId) {
		photonView.RPC("CreateActionRPC", PhotonTargets.AllBuffered, _actionDB.rowNames[(int)type], (int)playerId, PhotonNetwork.AllocateViewID());
	}

	public void CreateAction(string type, PlayerID playerId) {
		photonView.RPC("CreateActionRPC", PhotonTargets.AllBuffered, type, (int)playerId, PhotonNetwork.AllocateViewID());
	}

	[RPC]
	private void CreateActionRPC(string type, int id, int viewId) {
		var playerId = (PlayerID) id;
		var row = _actionDB.GetRow(type);

		var action = (Instantiate(prefabAction, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<CardAction>();

		action.GetComponent<PhotonView>().viewID = viewId;

		action.transform.SetParent(GameObject.Find("Cards").transform);
		action.transform.localScale = new Vector3(1f,1f,1f);

		action.fullName = row._NAME;
		action.effectDesc = row._CARDEFFECTDESC;
		action.ownerID = playerId;
		action.description = row._DESC;
		action.corruptionCost = row._CORRUPTIONCOST;
		action.sexismeCost = row._SEXISMECOST;
		action.gameObject.AddComponent(row._CARDEFFECT);
		action.baseAttack = row._ATTACK;
		action.baseReputation = row._REPUTATION;
		action.canTargetAllies = row._CANTARGETALLIES;
		action.canTargetPlayers = row._CANTARGETPLAYERS;
	}



}
