using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour {
	public static NetworkPlayer myPlayer;
	public static NetworkPlayer enemyPlayer;
	[HideInInspector, SyncVar(hook="OnRematch")]
	public bool rematch = false;
	public int bubbles = 1;

	public void Restart(){
		bubbles = 1;
		OnRematch (false);
	}

	void Start(){
		if (isLocalPlayer) {
			myPlayer = this;
		} else {
			enemyPlayer = this;
		}
	}

	public NetworkPlayer OtherPlayer(){
		if (isLocalPlayer)
			return NetworkPlayer.enemyPlayer;
		else
			return this;
	}

	void OnRematch (bool r){
		rematch = r;
		if (!isLocalPlayer && r) {
			GameObject.Find ("LeaveButton").GetComponentInChildren<Text>().text = "Leave rematch";
			GameObject.Find ("RematchButton").GetComponentInChildren<Text>().text = "Accept rematch!";
		}
	}

	[Command]
	public void CmdRematch(){
		if (OtherPlayer ().rematch) {
			NetworkController.instance.RpcRematch();
		}
		OnRematch(true);
	}

	[Command]
	public void CmdPvsPpops (int number){
		NetworkController.instance.GetComponent<PvsPcontroller> ().RpcPops(number, netId);
	}
}
