using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class multitouchMovement : MonoBehaviour {
	Vector2 startPoint = Vector2.zero;
	Vector3 initialPosition = Vector3.zero;
	Text debugText;

	void Start(){
		debugText = GameObject.Find ("Canvas").GetComponentInChildren<Text> ();
	}
	
	Vector2 GetMidpoint(Touch touch1, Touch touch2)
	{
		return Vector2.Lerp (touch1.position, touch2.position, 0.5f);
	}

	bool IsBegun(Touch touch1, Touch touch2)
	{
		return (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began) ||
			(touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Began) ||
				(touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Began);
	}    

	void MoveCamera()
	{
		// do not do anything until there are at least two points.
		debugText.text = Input.touchCount.ToString ();
		
		if(Input.touchCount < 2)
			return;
		
		// capture the touch points
		Touch touch1 = Input.GetTouch(0);
		Touch touch2 = Input.GetTouch(1);
		
		// gets the midpoint between the two touches
		var midpoint = GetMidpoint(touch1, touch2);
		
		// if its just started, save the first point.
		if (IsBegun (touch1, touch2)) {
			startPoint = midpoint;
			initialPosition = transform.position;
			return;
		}
		
		// get the difference between the two points.
		Vector2 difference = startPoint - midpoint;
		debugText.text = difference.ToString ();

		float ratio = Screen.height / 10f;
		transform.position = new Vector3(
			Mathf.Clamp (initialPosition.x + difference.x / ratio, -8.0f, 6.0f),
			Mathf.Clamp (initialPosition.y + difference.y / ratio, -4.0f, 4.0f),
			initialPosition.z);
	}

	void Update(){
		MoveCamera ();
	}
}
