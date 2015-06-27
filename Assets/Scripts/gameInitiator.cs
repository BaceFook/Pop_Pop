using UnityEngine;
using System.Collections;

public class gameInitiator : MonoBehaviour {

	public int bubblesX = 16;
	public int bubblesY = 16;

	public Transform normalBubble;

	void Start () {
		float startX = -2.0f * bubblesX / 2.0f;
		float startY = -2.0f * bubblesY / 2.0f;

		for (int i = 0; i < bubblesX; i++) {
			for (int j = 0; j < bubblesY; j++) {
				Instantiate (normalBubble, new Vector3(startX + 2.0f * i, startY + 2.0f * j, 0), Quaternion.identity);
			}
			
		}
	}

}
