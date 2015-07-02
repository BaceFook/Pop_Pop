using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class networkStarter : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("CloudNetworkingId", "124152");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
