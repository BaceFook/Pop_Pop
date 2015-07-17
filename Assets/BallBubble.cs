using UnityEngine;
using System.Collections;

public class BallBubble : MonoBehaviour {
	public float force = 200f;

	void Start () {
		float angle = Random.Range (0f, Mathf.PI);
		GetComponent<Rigidbody2D> ().AddForce (new Vector2(Mathf.Sin (angle), Mathf.Cos (angle)) * force);
		if (!NetworkController.instance.swipeEnabled) {
			transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		}
	
	}

	void GetPopped (){
		if(NetworkController.instance.currentMode == NetworkController.GameMode.PvsP)
			NetworkPlayer.myPlayer.CmdPvsPpops (1);
		Destroy (gameObject);
	}
}
