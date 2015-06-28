using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameInitiator : MonoBehaviour {

	private int groupNum = 0;
	private GameObject bubblesParent;

	public int bubblesX = 10;
	public int bubblesY = 5;

	public int lockedNum = 50;
	public int poppedNum = 10;

	public Transform normalBubble;
	public Transform[,] bubbleArray;

	List<int>availableBubbles = new List<int>();
	List<int>lockedBubbles = new List<int>();

	private int tmp;


	void Start () {

		bubblesParent = GameObject.Find ("BubblesParent");
		bubbleArray = new Transform[bubblesX,bubblesY];

		float startX = -1.0f * (bubblesX-1);
		float startY = -1.0f * (bubblesY-1);


		for (int i = 0; i < bubblesX; i++) {
			for (int j = 0; j < bubblesY; j++) {
				bubbleArray[i,j] = (Transform)Instantiate (normalBubble, new Vector3(startX + 2.0f * i, startY + 2.0f * j, 0), Quaternion.identity);
				bubbleArray[i,j].parent = bubblesParent.GetComponent<Transform>();
				availableBubbles.Add(i + bubblesX * j);
			}
		}

		// Use random seed to generate same results
		// Random.seed = 0;
		for (int i = 0; i < poppedNum; i++) {
			tmp = Random.Range (0, availableBubbles.Count);
			tmp = availableBubbles[tmp];
			bubbleArray[(tmp % bubblesX), Mathf.FloorToInt(tmp / bubblesX)].BroadcastMessage("startPopped");
			availableBubbles.Remove(tmp);
		}
		
		for (int i = 0; i < lockedNum; i++) {
			tmp = Random.Range (0, availableBubbles.Count);
			tmp = availableBubbles[tmp];
			bubbleArray[(tmp % bubblesX), Mathf.FloorToInt(tmp / bubblesX)].BroadcastMessage("startLocked");
			availableBubbles.Remove(tmp);
			lockedBubbles.Add (tmp);
		}

		tmp = Random.Range (0, availableBubbles.Count);
		tmp = availableBubbles[tmp];
		bubbleArray[(tmp % bubblesX), Mathf.FloorToInt(tmp / bubblesX)].BroadcastMessage("startWithKey");

	}

	void unlockNextBubble(){
		if (lockedBubbles.Count == 0)
			return;
		tmp = lockedBubbles [0];
		lockedBubbles.RemoveAt (0);
		bubbleArray[(tmp % bubblesX), Mathf.FloorToInt(tmp / bubblesX)].BroadcastMessage("unlockBubble");
		bubbleArray[(tmp % bubblesX), Mathf.FloorToInt(tmp / bubblesX)].BroadcastMessage("startWithKey");
	}

}
