using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PopInput : NetworkBehaviour {

	Vector3 lastPoint;
	LayerMask bubbleMask;
	bool lastActive = false;

	void Start(){
		bubbleMask = LayerMask.GetMask ("Bubbles");
	}

	void Update(){
		if (NetworkController.instance.gameEnded)
			return;
		if (NetworkController.instance.swipeEnabled) {
			SwipePop();
		} else {
			TapPop();
		}
	}

	[ClientCallback]
	void SwipePop(){
		
		Vector3 point = Vector3.zero;
		bool tmp = false;
		if (Input.GetMouseButton (0)) {
			point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			tmp = true;
		}  else if(Input.touchCount > 0) {
			point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			tmp = true;
		}
		
		if (tmp) {
			if (!lastActive)
				lastPoint = point;
			lastActive = true;
			RaycastHit2D hit = Physics2D.Linecast ((Vector2)point, (Vector2)lastPoint, bubbleMask.value);
			if (hit.transform != null){
				hit.transform.BroadcastMessage ("GetPopped");
			}
			
			lastPoint = point;
		} else {
			lastActive = false;
		}
	}

	[ClientCallback]
	void TapPop(){
		if (Input.GetMouseButtonDown (0)) {
			TapBubble(Camera.main.ScreenToWorldPoint (Input.mousePosition));
		}

		for (int i = 0; i < Input.touchCount; i++) {
			if(Input.GetTouch(i).phase == TouchPhase.Began){
				TapBubble(Camera.main.ScreenToWorldPoint (Input.GetTouch(i).position));
			}
		}
	}

	void TapBubble(Vector3 worldPoint){
		Collider2D col = Physics2D.OverlapPoint (worldPoint);
		if (col != null)
			col.transform.BroadcastMessage ("GetPopped");
	}
}
