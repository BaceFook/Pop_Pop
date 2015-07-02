using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spriteRandomizer : NetworkBehaviour {

	[SyncVar(hook="SetPhase")]
	public float phase;
	
	// Use this for initialization
	void Start () {
		if(isServer)
			SetPhase(Random.Range (0f, Mathf.PI * 2f));
	}

	void SetPhase(float tmp){
		phase = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		phase += Time.deltaTime;
		transform.position = new Vector3 (1f, Mathf.Sin (phase) * 2f, 1f);
	}

	

//	void OnStartClient(){
//		Debug.Log ("Client started");
//	}
//	
//	void OnStartServer(){
//		Debug.Log ("Server started");
//	}
//	
//	void OnConnectedToServer(){
//		Debug.Log ("Connected to server");
//	}
//	
//	void OnPlayerConnected(){
//		Debug.Log ("Player connected to server");
//	}
//	
//	void OnPlayerDisconnected(){
//		Debug.Log ("Player disconnected from server");
//	}
}
