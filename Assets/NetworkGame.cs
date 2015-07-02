using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkGame : NetworkBehaviour {

	void Start(){

	}

	public int score0 = 1;
	public int score1 = 1;

	[ClientRpc]
	public void RpcOnPop(bool byServer){
		Debug.Log ("RPC" + byServer.ToString());
		if (byServer)
			score0++;
		else
			score1++;
	}

	void Update(){
		if (NetworkServer.active) {
			GameObject.Find("myScore").GetComponent<Text>().text = score0.ToString();
			GameObject.Find("enemyScore").GetComponent<Text>().text = score1.ToString();
		} else {
			GameObject.Find("myScore").GetComponent<Text>().text = score1.ToString();
			GameObject.Find("enemyScore").GetComponent<Text>().text = score0.ToString();
			
		}
	}
}
