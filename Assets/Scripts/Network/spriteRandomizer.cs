using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spriteRandomizer : NetworkBehaviour {
	
	[Command]
	public void CmdOnPop(bool player0){
		Debug.Log ("Command" + player0.ToString());
		GameObject.FindObjectOfType<NetworkGame> ().RpcOnPop (player0);
	}


	[SyncVar(hook="SetPhase")]
	public float phase;
	private float phaseSyncTime;
	
	// Use this for initialization
	void Start () {
		if(isServer)
			SetPhase(Random.Range (0f, Mathf.PI * 2f));
	}

	void SetPhase(float tmp){
		phase = tmp;
		phaseSyncTime = Time.unscaledTime;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (1f, Mathf.Sin (phase + Time.unscaledTime - phaseSyncTime) * 2f, 1f);
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
