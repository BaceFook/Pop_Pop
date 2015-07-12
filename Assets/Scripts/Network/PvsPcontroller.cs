using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PvsPcontroller : NetworkBehaviour {
	public GameObject wallsPrefab;
	public GameObject ballPrefab;

	public void Setup(){
		GameObject.Find ("WinButton").GetComponent<Button> ().onClick.AddListener(delegate{NetworkPlayer.myPlayer.CmdPvsPpops(1);});
		GameObject.Instantiate (wallsPrefab, Vector3.zero, Quaternion.identity);
		SpawnBall ();
	}

	void SpawnBall(){
		GameObject ball = (GameObject) GameObject.Instantiate(ballPrefab, Random.onUnitSphere, Quaternion.identity);
		ball.GetComponent<Rigidbody2D> ().AddForce (Random.insideUnitCircle * 100);
	}

	[ClientRpc]
	public void RpcPops(int number, NetworkInstanceId popId){
		if (NetworkPlayer.myPlayer.netId != popId) {
			for (int i = 0; i < number * 2; i++) {
				SpawnBall ();
			}
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
