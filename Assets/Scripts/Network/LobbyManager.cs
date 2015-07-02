using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkManager {

	public GameObject networkGame;

	private NetworkConnection myConnection;

	public void Pop(){

	}

	void Start () {
		PlayerPrefs.SetString("CloudNetworkingId", "124152");
	}

	void Update () {

	}

	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);
		Debug.Log ("Client pressed ready");
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		ClientScene.RegisterPrefab (networkGame);
		GameObject ga = (GameObject)Instantiate (networkGame);
		ga.name = networkGame.name;
		NetworkServer.Spawn (ga);
		Debug.Log ("I started server");
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient (client);
		Debug.Log ("I started client");
	}
	
	public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am server + enemy";
		Debug.Log ("Someone connected to me");
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		myConnection = conn;
		if (NetworkServer.active) {
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am server";
			Debug.Log ("I connected to myself");
			ClientScene.Ready (conn);
			ClientScene.AddPlayer (0);
		}
		else
		{
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am client";
			Debug.Log ("I connected to someone");
			ClientScene.Ready (conn);
			ClientScene.AddPlayer (1);
		}
	}

	public override void OnMatchCreate (UnityEngine.Networking.Match.CreateMatchResponse matchInfo)
	{
		base.OnMatchCreate (matchInfo);
		Debug.Log (matchInfo);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer (conn, playerControllerId);
		Debug.Log (playerControllerId);
	}
}