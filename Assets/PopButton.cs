using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PopButton : MonoBehaviour {

	public void Pop(){
		GameObject.FindObjectOfType<NetworkGame> ().CmdOnPop (NetworkServer.active);
	}
}
