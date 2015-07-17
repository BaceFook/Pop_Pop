using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PvsPcontroller : NetworkBehaviour {
	public GameObject wallsPrefab;
	public GameObject ballPrefab;

	public int bubblesToLose = 10;

	public float roomWidth = 40f;
	public float roomHeight = 20f;


	public void Setup(){

		GameObject go = (GameObject) GameObject.Instantiate (wallsPrefab, Vector3.zero, Quaternion.identity);
		go.transform.parent = NetworkController.instance.gameParent.transform;
		go.GetComponent<WallScript> ().SetSize (roomWidth, roomHeight);
		SpawnBall ();
		GameObject.Find ("BubblesText").GetComponent<Text> ().text = "1";
	}

	void SpawnBall(){
		Vector2 tmp = Random.insideUnitCircle * 5f;

		GameObject ball = (GameObject) GameObject.Instantiate(ballPrefab, new Vector3(tmp.x, tmp.y, 0f), Quaternion.identity);
		ball.transform.parent = NetworkController.instance.gameParent.transform;
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

		if (NetworkController.instance.gameEnded)
			return;
		GameObject.Find ("BubblesText").GetComponent<Text> ().text = NetworkPlayer.myPlayer.bubbles.ToString ();
		if (NetworkServer.active)
			CheckForVictory ();
	}

	void CheckForVictory(){
		if (NetworkPlayer.myPlayer.bubbles >= bubblesToLose)
			NetworkController.instance.RpcVictory (NetworkPlayer.enemyPlayer.netId);
		else if (NetworkPlayer.enemyPlayer.bubbles >= bubblesToLose)
			NetworkController.instance.RpcVictory (NetworkPlayer.myPlayer.netId);

	}
}
