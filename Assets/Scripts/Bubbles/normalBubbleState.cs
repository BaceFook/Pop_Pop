using UnityEngine;
using System.Collections;


public class normalBubbleState : MonoBehaviour {
	public bool isPopped = false;
	public bool isLocked = false;
	public bool isKey = false;
	public bool hasPowerup = false;
	public int id;

	public Sprite[] normalSprites;
	public Sprite[] poppedSprites;
	public AudioClip poppingSfx;

	private SpriteRenderer highlightRenderer;
	private float highlightVal = 0;

	private SpriteRenderer myRenderer;

	void Awake(){
		myRenderer = transform.FindChild ("bubble").GetComponent<SpriteRenderer>();
	}

	void Start(){
		if(!isPopped)
			myRenderer.sprite = normalSprites [Random.Range (0, normalSprites.Length)];
		highlightRenderer = transform.FindChild ("highlight").GetComponent<SpriteRenderer> ();
	}

	void Update(){
		if (highlightVal > 0.0f) {
			highlightVal = Mathf.Max (highlightVal - Time.deltaTime * 2.0f, 0.0f);
			highlightRenderer.color = new Color( 1, 1, 1, highlightVal);
		}
	}

	void OnMouseDown(){
		popBubble ();
	}

	public void popBubble(){
		if (isPopped || isLocked) {
			//Handheld.Vibrate ();
			return;
		}
		isPopped = true;
		if (isKey) {
			gameInitiator.Instance.unlockNextBubble();
			isKey = false;
		}

		if (hasPowerup)
			BroadcastMessage ("TriggerPowerup", id);
		GetComponent<bubbleSound> ().playPop ();
		toNormalBubble ();
	}

	void toNormalBubble(){
		myRenderer.sprite = poppedSprites [Random.Range (0, poppedSprites.Length)];
		AudioSource.PlayClipAtPoint(poppingSfx, Camera.main.transform.position);
		isPopped = true;
	}

	void startPopped(){
		toNormalBubble ();
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
