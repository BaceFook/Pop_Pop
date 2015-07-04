using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class LobbyManager : NetworkManager {

	public GameObject networkGame;

	List<MatchDesc> attemptedMatches = new List<MatchDesc>();

	private NetworkConnection myConnection;
	bool connected = false;

	float retryAt = 0f;

	public void Pop(){

	}

	void Start () {
		PlayerPrefs.SetString ("CloudNetworkingId", "124152");
		StartMatchMaker ();
		CreateOrJoin ();
	}

	void CreateOrJoin(){
		matchMaker.ListMatches (0, 25, "", OnMyMatchList);
	}

	void OnMyMatchList(ListMatchResponse matchList){
		Debug.Log ("Cecking list");
		bool skip = false;

		int count = matchList.matches.Count;
		if (matchList.matches.Count > 0) {

			if (matchList.matches [count - 1].currentSize < 2) {

				foreach (MatchDesc match in attemptedMatches) {
					if (matchList.matches [count - 1].networkId == match.networkId)
						skip = true;
				}

				if(!skip){
					attemptedMatches.Add (matchList.matches [count - 1]);

					Debug.Log ("Joining: " + matchList.matches [count - 1].networkId.ToString ());
					matchMaker.JoinMatch (matchList.matches [count - 1].networkId, "", OnJoiningMatch);
					return;
				}
			}
		}

		Debug.Log ("Creating my own");
		matchMaker.CreateMatch("pop", 2, true, "", OnMatchCreate);

	}

	public override void OnClientError (NetworkConnection conn, int errorCode)
	{
		base.OnClientError (conn, errorCode);
		Debug.Log (errorCode);
		if (connected) {
			Debug.Log ("SHOULD BE CONNECTED");
			return;
		}
		if (errorCode == 6) {
			retryAt = Time.time + 1;
		}
	}

	void Update(){
		if (retryAt != 0f && retryAt < Time.time){
			StartMatchMaker ();
			CreateOrJoin ();
			retryAt = 0f;
		}
	}

	void OnJoiningMatch(JoinMatchResponse response){
		OnMatchJoined (response);
		if (!response.success) {
			CreateOrJoin();
		}
	}

	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);
//		Debug.Log ("Client pressed ready");
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		ClientScene.RegisterPrefab (networkGame);
		GameObject ga = (GameObject)Instantiate (networkGame);
		ga.name = networkGame.name;
		NetworkServer.Spawn (ga);
//		Debug.Log ("I started server");
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient (client);
//		Debug.Log ("I started client");
	}
	
	public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am server + enemy";
//		Debug.Log ("Someone connected to me");
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		myConnection = conn;
		if (NetworkServer.active) {
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am server";
//			Debug.Log ("I connected to myself");
			ClientScene.Ready (conn);
			connected = true;
			ClientScene.AddPlayer (0);
		}
		else
		{
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am client";
//			Debug.Log ("I connected to someone");
			ClientScene.Ready (conn);
			connected = true;
			ClientScene.AddPlayer (1);
		}
	}

	public override void OnMatchCreate (UnityEngine.Networking.Match.CreateMatchResponse matchInfo)
	{
		base.OnMatchCreate (matchInfo);
//		Debug.Log (matchInfo);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer (conn, playerControllerId);
//		Debug.Log (playerControllerId);
	}

	void OnDestroy(){
//		Debug.Log ("Should drop match from matchmaker");
		matchMaker.DestroyMatch (matchInfo.networkId, OnMatchDestoryed);
		NetworkServer.Shutdown ();
	}

	void OnMatchDestoryed(BasicResponse response){

	}
}