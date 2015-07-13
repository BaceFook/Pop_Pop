using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	Transform topWall;
	Transform rightWall;
	Transform bottomWall;
	Transform leftWall;

	void Awake (){
		topWall = transform.FindChild ("TopWall");
		rightWall = transform.FindChild ("RightWall");
		bottomWall = transform.FindChild ("BottomWall");
		leftWall = transform.FindChild ("LeftWall");
	}

	public void SetSize(float w, float h){
		if (w <= 0 || h <= 0) {
			Debug.LogError("Height or width invalid");
			return;
		}
		MultitouchMovement mm = Camera.main.GetComponent<MultitouchMovement> ();
		if (mm != null) {
			mm.enabled = true;
			float tmp = (h - 10f) / 2f;
			if(tmp > 0){
				mm.topBorder = tmp;
				mm.bottomBorder = - tmp;
			}
			tmp = (w - 10f * Camera.main.aspect) / 2f;
			if(tmp > 0){
				mm.rightBorder = tmp;
				mm.leftBorder = - tmp;
			}
		}

		topWall.localPosition = new Vector3(0f, (h + 1f) / 2f, 0);
		rightWall.localPosition = new Vector3((w + 1f) / 2f, 0f, 0);
		bottomWall.localPosition = new Vector3(0f, - (h + 1f) / 2f, 0);
		leftWall.localPosition = new Vector3(- (w + 1f) / 2f, 0f, 0);

		topWall.localScale = new Vector3(w + 2f, 1f, 1f);
		rightWall.localScale = new Vector3(1f, h + 2, 1f);
		bottomWall.localScale = new Vector3(w + 2f, 1f, 1f);
		leftWall.localScale = new Vector3(1f, h + 2, 1f);
	}
}
