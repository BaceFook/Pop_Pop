using UnityEngine;
using System.Collections;

public class normalBubbleState : MonoBehaviour {
	public bool isPopped = false;
	public bool isLocked = false;
	public bool isKey = false;
	public bool hasPowerup = false;
	public int id;

	public Sprite normalSprite;
	public Sprite poppedSprite;

	private SpriteRenderer highlightRenderer;
	private float highlightVal = 0;

	void Start(){
		highlightRenderer = transform.FindChild ("highlight").GetComponent<SpriteRenderer> ();
	}

	void Update(){
		if (highlightVal > 0.0f) {
			highlightVal = Mathf.Max (highlightVal - Time.deltaTime * 2.0f, 0.0f);
			highlightRenderer.color = new Color( 1, 1, 1, highlightVal);
		}
	}

	void OnMouseDown(){
		if (isPopped || isLocked) {
			//Handheld.Vibrate ();
			return;
		}

		if (isKey) {
			gameInitiator.Instance.unlockNextBubble();
			isKey = false;
		}

		if (hasPowerup)
			BroadcastMessage ("TriggerPowerup");
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

	public void highlightBubble(){
		if (isPopped)
			return;
		highlightVal = 1.0f;
	}

}
