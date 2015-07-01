using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spriteRandomizer : NetworkBehaviour {

	[SyncVar(hook="SetPhase")]
	public float phase;
	
	// Use this for initialization
	void Start () {
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
}
