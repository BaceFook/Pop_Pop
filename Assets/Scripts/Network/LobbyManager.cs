using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkManager {

	private NetworkConnection myConnection;
	
	public void Setready(){
//		ClientScene.Ready (client.connection);
		Debug.Log (NetworkServer.connections.Count);
		
		foreach (NetworkConnection conn  in NetworkServer.connections) {
			if (conn == null)
				continue;
			Debug.Log (conn.connectionId);
			Debug.Log (conn.isReady);
		}
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
		Debug.Log ("I started server");
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient (client);
		Debug.Log ("I started client");
		Debug.Log (client.connection.connectionId);
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
		Debug.Log (conn.connectionId);
		if (NetworkServer.active) {
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am server";
			Debug.Log ("I connected to myself");
			ClientScene.Ready (conn);
		}
		else
		{
			GameObject.Find ("networkStatus").GetComponent<Text>().text = "Am client";
			Debug.Log ("I connected to someone");
			ClientScene.Ready (conn);
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