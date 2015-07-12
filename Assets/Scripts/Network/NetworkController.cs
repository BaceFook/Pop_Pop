using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkController : NetworkBehaviour {

	public static NetworkController instance;

	[SyncVar]
	bool matchStarted = false;

	bool gameStarted = false;

	public enum GameMode{
		PvsP
	}

	[SyncVar(hook="OnSyncMode")]
	public GameMode currentMode = GameMode.PvsP;

	[SyncVar(hook="OnSyncTime")]
	public float gameTime = 0f;
	float gameStart;
	float lastTimeSync;
	public float timeSyncInterval = 0.2f;
	public float waitTime = 10f;

	Text remainingTimeText;

	void Start(){
		instance = this;
		remainingTimeText = GameObject.Find ("RemainingTimeText").GetComponent<Text> ();
	}

	void StartMatch(){
		matchStarted = true;
		gameStart = Time.realtimeSinceStartup;
	}

	void Update(){
		if (!matchStarted)
			return;

		// Syncing game time
		if (NetworkServer.active) {
			if (Time.realtimeSinceStartup > lastTimeSync + timeSyncInterval)
				OnSyncTime (Time.realtimeSinceStartup - gameStart);
			if (Time.realtimeSinceStartup - gameStart > waitTime && !gameStarted)
				RpcStartGame();
		} else {
			OnSyncTime(gameTime + Time.realtimeSinceStartup - lastTimeSync);
		}

		if(!gameStarted)
			remainingTimeText.text = (waitTime - gameTime).ToString ("F1");
	}

	void OnSyncTime(float time){
		gameTime = time;
		lastTimeSync = Time.realtimeSinceStartup;
	}

	void OnSyncMode(GameMode mode){
		currentMode = mode;
	}
	
	[ClientRpc]
	public void RpcVictory(NetworkInstanceId winId){
		if (NetworkPlayer.myPlayer.netId == winId) {
			Iwin();
		} else {
			Ilose();
		}
	}

	void Iwin(){
		Debug.Log ("Victory");
	}

	void Ilose(){
		Debug.Log ("Defeat");
	}


	[ClientRpc]
	void RpcStartGame(){
		gameStarted = true;
		CanvasGroups.Room.SetActive (false);
		CanvasGroups.Game.SetActive (true);
		GetComponent<PvsPcontroller> ().enabled = true;
		GetComponent<PvsPcontroller> ().Setup ();
	}
}
