using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PopButton : MonoBehaviour {

	public void Pop(){
		GameObject.FindObjectOfType<spriteRandomizer> ().CmdOnPop (NetworkServer.active);
	}
}
