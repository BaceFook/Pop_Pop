using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PvsPcontroller : NetworkBehaviour {
	public GameObject wallsPrefab;
	public GameObject ballPrefab;

	Vector3 lastPoint;
	LayerMask bubbleMask;

	public void Setup(){
		bubbleMask = LayerMask.GetMask ("Bubbles");

		GameObject.Find ("WinButton").GetComponent<Button> ().onClick.AddListener(delegate{NetworkPlayer.myPlayer.CmdPvsPpops(1);});
		GameObject go = (GameObject) GameObject.Instantiate (wallsPrefab, Vector3.zero, Quaternion.identity);
		go.transform.parent = NetworkController.instance.gameParent.transform;
		SpawnBall ();
	}

	void SpawnBall(){
		GameObject ball = (GameObject) GameObject.Instantiate(ballPrefab, Random.onUnitSphere, Quaternion.identity);
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
		if (NetworkPlayer.myPlayer.bubbles >= 100)
			NetworkController.instance.RpcVictory (NetworkPlayer.enemyPlayer.netId);
		else if (NetworkPlayer.enemyPlayer.bubbles >= 100)
			NetworkController.instance.RpcVictory (NetworkPlayer.myPlayer.netId);

	}

	void Update(){
		if (NetworkController.instance.gameEnded)
			return;

		Vector3 point = Vector3.zero;
		if (Input.GetMouseButton (0)) {
			point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}  else if(Input.touchCount > 0) {
			point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		}

		if (point.magnitude > float.Epsilon) {
			Debug.Log ((Vector2)point);
			if (lastPoint.magnitude < float.Epsilon)
				lastPoint = point;
			RaycastHit2D[] hits = Physics2D.LinecastAll ((Vector2)point, (Vector2)lastPoint, bubbleMask.value);
			if (hits.Length > 0)
				NetworkPlayer.myPlayer.CmdPvsPpops (hits.Length);
			foreach (RaycastHit2D hit in hits) {
				hit.transform.BroadcastMessage ("GetPopped");
			}
			lastPoint = point;
		} else {
			lastPoint = Vector3.zero;
		}
	}
}
