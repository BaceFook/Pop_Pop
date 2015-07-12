using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour {
	public static NetworkPlayer myPlayer;
	public static NetworkPlayer enemyPlayer;
	public int bubbles = 1;

	void Start(){
		if (isLocalPlayer) {
			myPlayer = this;
		} else {
			enemyPlayer = this;
		}
	}

	[Command]
	public void CmdPvsPpops (int number){
		NetworkController.instance.GetComponent<PvsPcontroller> ().RpcPops(number, netId);
	}
}
