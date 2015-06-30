using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameController : MonoBehaviour {

//	public Transform[] bubbleArray;
//
//	private static gameController instance = null;
//	
//	public static gameController Instance {
//		get {
//			return instance;
//		}
//	}
//
//	void Awake () {
//		instance = this;
//	}
//	List<Touch>normalBubbles = new List<Touch>();

	public class Finger{
		public bool isValid = true;
		public float pressDuration = 0.0f;
		public float swipeDuration = 0.0f;
		public GameObject lastObject;
		public int id;
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), new Vector2(point.x, point.y), 0.0f);
			if(hit.collider != null)
				Debug.Log (hit.collider.gameObject.transform.position);
			return;
		}

		for (int i = 0; i < Input.touchCount; i++) {
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), new Vector2(point.x, point.y), 0.0f);
			if(hit.collider != null)
				Debug.Log (hit.point);
		}
	}
}
