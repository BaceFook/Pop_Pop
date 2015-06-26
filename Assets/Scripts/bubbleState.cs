using UnityEngine;
using System.Collections;

public class bubbleState : MonoBehaviour {
	public enum State{
		Main, Credits
	}

	void OnMouseDown(){
		Handheld.Vibrate ();
	}
}
