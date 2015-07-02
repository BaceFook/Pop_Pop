using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class callbackTester : NetworkBehaviour {
	
	public override void OnStartClient ()
	{
		base.OnStartClient ();
		Debug.Log ("callbackTester Detects");
	}
}
