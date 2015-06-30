using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class serverButton : MonoBehaviour {
	HostData myServer;

	public void assignServer( HostData hostInfo){
		myServer = hostInfo;
		GetComponentInChildren<Text> ().text = myServer.gameName;
	}

	public void connectToServer(){
		Debug.Log (myServer.ip[0]);
	}
}
