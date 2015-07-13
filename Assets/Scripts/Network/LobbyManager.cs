using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class LobbyManager : NetworkManager {
	public GameObject networkController;
	public static LobbyManager instance;

	GameObject networkObject;

	List<string> attemptedMatches = new List<string>();
	private bool autoMatch = false;
	bool connected = false;
	float retryAt = 0f;

	void Awake(){
		instance = this;
	}

	void Start () {
		PlayerPrefs.SetString ("CloudNetworkingId", "124152");
	}

	public void AutoFindMatch(){
		retryAt = 0f;
		autoMatch = true;
		StartMatchMaker ();
		CreateOrJoinMatch ();
	}

	void CreateOrJoinMatch(){
		matchMaker.ListMatches (0, 25, "", OnMyMatchList);
	}

	void OnMyMatchList(ListMatchResponse matchList){
		if (!autoMatch)
			return;

		int count = matchList.matches.Count;
		if (matchList.matches.Count > 0) {
			if (matchList.matches [count - 1].currentSize < 2) {

				bool skip = false;
				foreach (string matchID in attemptedMatches) {
					if (matchList.matches [count - 1].networkId.ToString() == matchID)
						skip = true;
				}

				if(!skip){
					attemptedMatches.Add (matchList.matches [count - 1].networkId.ToString());
					matchMaker.JoinMatch (matchList.matches [count - 1].networkId, "", OnJoiningMatch);
					return;
				}
			}
		}
		StartMyServer ();
	}

	void StartMyServer(){
		var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
		matchMaker.CreateMatch(timestamp.ToString(), 2, true, "", OnMatchCreate);
	}

	public override void OnMatchCreate (CreateMatchResponse matchInfo)
	{
		attemptedMatches.Add (matchInfo.networkId.ToString());
		base.OnMatchCreate (matchInfo);
	}

	public override void OnClientError (NetworkConnection conn, int errorCode)
	{
		base.OnClientError (conn, errorCode);
		if (connected || !autoMatch) {
			return;
		}
		if (errorCode == 6 && autoMatch) {
			retryAt = Time.time + 1;
		}
	}

	void Update(){
		if (retryAt != 0f && retryAt < Time.time && autoMatch){
			StartMatchMaker ();
			CreateOrJoinMatch ();
			retryAt = 0f;
		}
	}

	void OnJoiningMatch(JoinMatchResponse response){
//		Debug.Log ("On Joining Match");
//		Debug.Log (response);
//		matchInfo = new MatchInfo (response);
//		StartClient ();
		OnMatchJoined (response);
//		if (!response.success) {
//			retryAt = Time.time + 1;
//		}
	}

	public override void OnStartServer ()
	{
//		Debug.Log ("On Start Server");
		base.OnStartServer ();
		networkObject = (GameObject)Instantiate (networkController);
		networkObject.name = networkController.name;
		NetworkServer.Spawn (networkObject);
	}
	
	public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		GameObject.Find ("StatusLabel").GetComponentInChildren<Text>().text = "Connected(S)";
		networkObject.BroadcastMessage ("StartMatch");
	}

	public override void OnStartClient (NetworkClient client)
	{
//		Debug.Log ("On Star Client");
		base.OnStartClient (client);
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		if (NetworkServer.active) {
			GameObject.Find ("StatusLabel").GetComponentInChildren<Text>().text = "Waiting for players";
			ClientScene.Ready (conn);
			connected = true;
			ClientScene.AddPlayer (0);
		}
		else
		{
			GameObject.Find ("StatusLabel").GetComponentInChildren<Text>().text = "Connected(C)";
			ClientScene.Ready (conn);
			connected = true;
			ClientScene.AddPlayer (0);
		}
	}

	public void ExitMatchMaker(){
		autoMatch = false;
		if (matchMaker != null && matchInfo != null)
			matchMaker.DestroyMatch (matchInfo.networkId, OnMatchDestoryed);
		if (NetworkServer.active)
			NetworkServer.Shutdown();
		StopMatchMaker ();
		StopHost ();
		StopClient ();
	}

	void OnDestroy(){
		ExitMatchMaker ();
	}

	void OnMatchDestoryed(BasicResponse response){

	}
}