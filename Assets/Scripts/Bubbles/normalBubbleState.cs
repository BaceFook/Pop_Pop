using UnityEngine;
using System.Collections;

public class normalBubbleState : MonoBehaviour {
	public bool isPopped = false;
	public bool isLocked = false;
	public bool isKey = false;

	public Sprite normalSprite;
	public Sprite poppedSprite;

	void OnMouseDown(){
		if (isPopped || isLocked) {
			//Handheld.Vibrate ();
			return;
		}

		if (isKey) {
			GameObject.Find ("GameController").BroadcastMessage("unlockNextBubble");
			isKey = false;
		}
		popBubble ();
	}

	void popBubble(){
		SpriteRenderer renderer = transform.FindChild ("bubble").GetComponent<SpriteRenderer>();
		renderer.sprite = poppedSprite;
		isPopped = true;
	}

	void startPopped(){
		popBubble ();
	}

	void lockBubble(){
		SpriteRenderer renderer = transform.FindChild ("lock").GetComponent<SpriteRenderer>();
		renderer.enabled = true;
		isLocked = true;
	}
	
	void startLocked(){
		lockBubble ();
	}

	void startWithKey(){
		isKey = true;
	}
	
	void unlockBubble(){
		SpriteRenderer renderer = transform.FindChild ("lock").GetComponent<SpriteRenderer>();
		renderer.enabled = false;
		isLocked = false;
	}

}
