using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerMessage : MonoBehaviour {
	public static MultiplayerMessage instance;

	void Awake(){
		instance = this;
	}
	
	public void Show(){
		GetComponent<Text> ().enabled = true;
	}
	
	public void Hide(){
		GetComponent<Text> ().enabled = false;
	}
}
