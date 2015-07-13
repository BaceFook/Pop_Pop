using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultitouchMovement : MonoBehaviour {
	public float topBorder = 0.0f;
	public float rightBorder = 0.0f;
	public float bottomBorder = 0.0f;
	public float leftBorder = 0.0f;

	Vector2 startPoint = Vector2.zero;
	Vector2 mouseStart = Vector2.zero;
	Vector3 initialPosition = new Vector3 (0f, 0f, -10f);

	void Start(){
		initialPosition = transform.localPosition;
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

	void checkMultitouch()
	{
		// do not do anything until there are at least two points.
		if (Input.touchCount < 2)
			return;
		
		// capture the touch points
		Touch touch1 = Input.GetTouch (0);
		Touch touch2 = Input.GetTouch (1);
		
		// gets the midpoint between the two touches
		var midpoint = GetMidpoint (touch1, touch2);
		
		// if its just started, save the first point.
		if (IsBegun (touch1, touch2)) {
			startPoint = midpoint;
			initialPosition = transform.position;
			return;
		}
		
		// get the difference between the two points.
		moveCamera(startPoint - midpoint);
	}

	void moveCamera(Vector2 difference){
		float ratio = Screen.height / 10f;

		transform.position = new Vector3(
			Mathf.Clamp (initialPosition.x + difference.x / ratio, leftBorder, rightBorder),
			Mathf.Clamp (initialPosition.y + difference.y / ratio, bottomBorder, topBorder),
			initialPosition.z);
	}

	void Update(){
		checkMultitouch();
		if (Input.touchCount > 0)
			return;

		if(Input.GetMouseButtonDown(1))
		{	
			initialPosition = transform.position;
			mouseStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
		
		if(Input.GetMouseButton(1))
		{
			moveCamera(mouseStart - new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
	}
}
