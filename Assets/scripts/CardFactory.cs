using UnityEngine;
using System.Collections;
using GoogleFu;

public class CardFactory : MonoBehaviour {

	public GameObject prefabAction;
	public GameObject prefabActor;
	public GameObject prefabContext;

	public Transform CardsContainer;

	public static CardFactory Instance;
	private ActionDB _actionDB;
    private ActorDB _actorDB;
	private TrendingDB _trendingDB;

	public PhotonView photonView;
	// Use this for initialization
	void Awake () {
		photonView = GetComponent<PhotonView>();
		_actionDB = ActionDB.Instance;
	    _actorDB = ActorDB.Instance;
		_trendingDB = TrendingDB.Instance;

		Instance = this;
		Debug.Log("DB ready");
	}

	public void CreateAction(ActionDB.rowIds type, PlayerID playerId) {
		photonView.RPC("CreateActionRPC", PhotonTargets.AllBuffered, _actionDB.rowNames[(int)type], (int)playerId, PhotonNetwork.AllocateViewID());
	}

	public void CreateAction(string type, PlayerID playerId) {
		photonView.RPC("CreateActionRPC", PhotonTargets.AllBuffered, type, (int)playerId, PhotonNetwork.AllocateViewID());
	}

	[RPC]
	public void CreateActionRPC(string type, int id, int viewId) {
		var playerId = (PlayerID) id;
		var row = _actionDB.GetRow(type);

		var action = (Instantiate(prefabAction, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<CardAction>();

		action.GetComponent<PhotonView>().viewID = viewId;

		action.transform.SetParent(CardsContainer);
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
		action.canTargetEnemyPlayer = row._CANTARGETENEMYPLAYER;
		action.canTargetSelf = row._CANTARGETSELF;
		action.canTargetActors = row._CANTARGETACTORS;

		action.id = type;
	}


    public void CreateActor(ActorDB.rowIds type, PlayerID playerId)
    {
        photonView.RPC("CreateActorRPC", PhotonTargets.AllBuffered, _actorDB.rowNames[(int)type], (int)playerId, PhotonNetwork.AllocateViewID());
    }

    public void CreateActor(string type, PlayerID playerId)
    {
        photonView.RPC("CreateActorRPC", PhotonTargets.AllBuffered, type, (int)playerId, PhotonNetwork.AllocateViewID());
    }

    [RPC]
	public void CreateActorRPC(string type, int id, int viewId)
    {
        var playerId = (PlayerID)id;
        var row = _actorDB.GetRow(type);

        var actor = (Instantiate(prefabActor, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<CardActor>();

        actor.GetComponent<PhotonView>().viewID = viewId;

		actor.transform.SetParent(CardsContainer);
        actor.transform.localScale = new Vector3(1f, 1f, 1f);

        actor.fullName = row._NAME;
        actor.effectDesc = row._CARDEFFECTDESC;
        actor.ownerID = playerId;
        actor.description = row._DESC;
        actor.corruptionCost = row._CORRUPTIONCOST;
        actor.sexismeCost = row._SEXISMECOST;
        actor.gameObject.AddComponent(row._CARDEFFECT);
        actor.baseAttack = row._ATTACK;
        actor.baseReputation = row._REPUTATION;

		actor.id = type;
    }



	public void CreateContext(TrendingDB.rowIds type, PlayerID playerId) {
		photonView.RPC("CreateContextRPC", PhotonTargets.AllBuffered, _trendingDB.rowNames[(int)type], (int)playerId, PhotonNetwork.AllocateViewID());
	}

	public void CreateContext(string type, PlayerID playerId) {
		photonView.RPC("CreateContextRPC", PhotonTargets.AllBuffered, type, (int)playerId, PhotonNetwork.AllocateViewID());
	}

	[RPC]
	public void CreateContextRPC(string type, int id, int viewId) {
		var playerId = (PlayerID)id;
		var row = _trendingDB.GetRow(type);

		var context = (Instantiate(prefabContext, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<CardContext>();

		context.GetComponent<PhotonView>().viewID = viewId;

		context.transform.SetParent(CardsContainer);
		context.transform.localScale = new Vector3(1f, 1f, 1f);

		context.fullName = row._NAME;
		context.effectDesc = row._CARDEFFECTDESC;
		context.ownerID = playerId;
		context.description = row._DESC;
		context.corruptionCost = 0;//row._CORRUPTIONCOST;
		context.sexismeCost = 0;//row._SEXISMECOST;
		context.gameObject.AddComponent(row._CARDEFFECT);
		context.baseAttack = 0;//row._ATTACK;
		context.baseReputation = 1;//row._REPUTATION;
		context.corruptionMultiplier = row._CORRUPTIONMULT;
		context.sexismeMultiplier = row._SEXISMEMULT;

		context.id = type;
	}


}
