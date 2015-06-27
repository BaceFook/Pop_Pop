using UnityEngine;
using System.Collections;

public class gameInitiator : MonoBehaviour {

	private int groupNum = 0;

	public int bubblesX = 10;
	public int bubblesY = 5;

	public Transform normalBubble;
	public Transform[,] bubbleArray;

	void Start () {

		bubbleArray = new Transform[bubblesX,bubblesY];

		float startX = -1.0f * (bubblesX-1);
		float startY = -1.0f * (bubblesY-1);


		for (int i = 0; i < bubblesX; i++) {
			for (int j = 0; j < bubblesY; j++) {
				bubbleArray[i,j] = (Transform)Instantiate (normalBubble, new Vector3(startX + 2.0f * i, startY + 2.0f * j, 0), Quaternion.identity);
			}
			
		}
	}

}
