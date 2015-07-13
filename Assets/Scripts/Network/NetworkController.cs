using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkController : NetworkBehaviour {
	public GameObject gameParent;
	public static NetworkController instance;

	[SyncVar,HideInInspector]
	public bool matchStarted = false;
	[HideInInspector]
	public bool gameStarted = false;
	[HideInInspector]
	public bool gameEnded = false;

	public enum GameMode{
		PvsP
	}

	[SyncVar(hook="OnSyncMode")]
	public GameMode currentMode = GameMode.PvsP;

	[SyncVar(hook="OnSyncTime")]
	public float gameTime = 0f;
	float gameStart;
	float lastTimeSync;
	public float timeSyncInterval = 0.4f;
	public float waitTime = 5f;

	Text remainingTimeText;

	void Start(){
		instance = this;
		gameParent = GameObject.Find ("GameParent");
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
			remainingTimeText.text = (waitTime - GameTime()).ToString ("F1");
	}

	public float GameTime(){
		if (NetworkServer.active)
			return Time.realtimeSinceStartup - gameStart;
		else
			return gameTime;
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
		gameEnded = true;
		if (NetworkPlayer.myPlayer.netId == winId) {
			Iwin();
		} else {
			Ilose();
		}
		GameObject.Find ("LeaveButton").GetComponentInChildren<Text>().text = "Leave.";
		GameObject.Find ("RematchButton").GetComponentInChildren<Text>().text = "Rematch!";
	}

	void Iwin(){
		MultiplayerMenu.Game.SetActive (false);
		MultiplayerMenu.Post.SetActive (true);
		GameObject.Find ("OutcomeText").GetComponent<Text> ().text = "You've won!";
	}

	void Ilose(){
		MultiplayerMenu.Game.SetActive (false);
		MultiplayerMenu.Post.SetActive (true);
		GameObject.Find ("OutcomeText").GetComponent<Text> ().text = "You've lost!";
	}


	[ClientRpc]
	void RpcStartGame(){
		gameStarted = true;
		MultiplayerMenu.Room.SetActive (false);
		MultiplayerMenu.Game.SetActive (true);
		GetComponent<PvsPcontroller> ().enabled = true;
		GetComponent<PvsPcontroller> ().Setup ();
	}

	public void LeaveMatch(){
		Camera.main.GetComponent<MultitouchMovement> ().enabled = false;
		Camera.main.transform.position = Vector3.zero;
		foreach (Transform child in gameParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		LobbyManager.instance.ExitMatchMaker ();
		
		MultiplayerMenu.Post.SetActive (false);
		MultiplayerMenu.Game.SetActive (false);
		MultiplayerMenu.Auto.SetActive (true);
	}

	public void Rematch(){
		if (!NetworkPlayer.myPlayer.rematch)
			NetworkPlayer.myPlayer.CmdRematch ();
	}

	[ClientRpc]
	public void RpcRematch(){
		if(NetworkServer.active)
			StartMatch ();
		gameStarted = false;
		gameEnded = false;
		foreach (Transform child in gameParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		NetworkPlayer.myPlayer.Restart ();
		NetworkPlayer.enemyPlayer.Restart ();

		MultiplayerMenu.Post.SetActive (false);
		MultiplayerMenu.Game.SetActive (false);
		MultiplayerMenu.Room.SetActive (true);
	}
}
