using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PvsPcontroller : NetworkBehaviour {
	public GameObject wallsPrefab;

	public void Setup(){
		GameObject.Find ("WinButton").GetComponent<Button> ().onClick.AddListener(delegate{NetworkPlayer.myPlayer.CmdPvsPpops(1);});
		GameObject.Instantiate (wallsPrefab, Vector3.zero, Quaternion.identity);
	}

	[ClientRpc]
	public void RpcPops(int number, NetworkInstanceId popId){
		if (NetworkPlayer.myPlayer.netId != popId) {
			// Spawn local bubbles
			NetworkPlayer.myPlayer.bubbles += number * 2;
			NetworkPlayer.enemyPlayer.bubbles -= number;
		} else {
			NetworkPlayer.enemyPlayer.bubbles += number * 2;
			NetworkPlayer.myPlayer.bubbles -= number;
		}

		GameObject.Find ("BubblesText").GetComponent<Text> ().text = NetworkPlayer.myPlayer.bubbles.ToString ();
		if (NetworkServer.active)
			CheckForVictory ();
	}

	void CheckForVictory(){
		if (NetworkPlayer.myPlayer.bubbles >= 100)
			NetworkController.instance.RpcVictory (NetworkPlayer.enemyPlayer.netId);
		else if (NetworkPlayer.enemyPlayer.bubbles >= 100)
			NetworkController.instance.RpcVictory (NetworkPlayer.myPlayer.netId);

	}
}
