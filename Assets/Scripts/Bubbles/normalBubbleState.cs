using UnityEngine;
using System.Collections;

public class normalBubbleState : MonoBehaviour {
	public bool isPopped = false;

	public Sprite normalSprite;
	public Sprite poppedSprite;

	void OnMouseDown(){
		if (isPopped) {
			//Handheld.Vibrate ();
			return;
		}

		SpriteRenderer renderer = transform.FindChild ("bubble").GetComponent<SpriteRenderer>();
		renderer.sprite = poppedSprite;
		isPopped = true;
	}
}
