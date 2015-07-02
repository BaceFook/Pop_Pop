using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyManager : NetworkManager {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("CloudNetworkingId", "124152");
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		Debug.Log ("networkScarter Detects");
	}
}