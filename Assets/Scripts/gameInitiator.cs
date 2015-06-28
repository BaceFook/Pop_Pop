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

	public int connectedPairs = 5;

	public Transform normalBubble;
	public Transform[] bubbleArray;

	List<int>normalBubbles = new List<int>();
	List<int>poppableBubbles = new List<int>();
	List<int>lockableBubbles = new List<int>();

	List<int>horizontalPairs = new List<int>();
	List<int>lockedBubbles = new List<int>();
	
	private int tmp;
	private int _x;
	private int _y;


	void Start () {
		// Use random seed to generate same results
		// Random.seed = 0;

		bubblesParent = GameObject.Find ("BubblesParent");
		bubbleArray = new Transform[bubblesX * bubblesY];

		float startX = -1.0f * (bubblesX-1);
		float startY = -1.0f * (bubblesY-1);
		
		for (int i = 0; i < bubblesX; i++) {
			for (int j = 0; j < bubblesY; j++) {
				bubbleArray[i + j * bubblesX] = (Transform)Instantiate (normalBubble, new Vector3(startX + 2.0f * i, startY + 2.0f * j, 0), Quaternion.identity);
				bubbleArray[i + j * bubblesX].parent = bubblesParent.GetComponent<Transform>();
			}
		}

		for (int i = 0; i < bubblesX * bubblesY; i++) {
			normalBubbles.Add(i);	
			poppableBubbles.Add(i);	
			lockableBubbles.Add(i);	
		}
		
		for (int i = 0; i < (bubblesX - 1) * bubblesY; i++) {
			horizontalPairs.Add(i);			
		}

		// Set up powerups and powerdowns
		// ...

		// Set up connected pairs
		for (int i = 0; i < connectedPairs; i++) {
			tmp = Random.Range (0, horizontalPairs.Count);
			tmp = horizontalPairs[tmp];
			horizontalPairs.Remove(tmp);
			if((tmp + 1) % (bubblesX - 1) != 0 && tmp != (bubblesX - 1) * bubblesY - 1)
				horizontalPairs.Remove(tmp + 1);
			if(tmp % (bubblesX - 1) != 0)
				horizontalPairs.Remove(tmp - 1);

			int leftNum = tmp + Mathf.FloorToInt(tmp / (bubblesX - 1.0f));
			int rightNum = tmp + Mathf.FloorToInt(tmp / (bubblesX - 1.0f) + 1);

			if(Random.Range (0, 2) == 0)
				lockableBubbles.Remove(leftNum);
			else
				lockableBubbles.Remove(rightNum);

			poppableBubbles.Remove(leftNum);
			poppableBubbles.Remove(rightNum);
		}



		for (int i = 0; i < poppedNum; i++) {
			tmp = Random.Range (0, poppableBubbles.Count);
			tmp = poppableBubbles[tmp];
			bubbleArray[tmp].BroadcastMessage("startPopped");
			poppableBubbles.Remove(tmp);
			lockableBubbles.Remove(tmp);
		}
		
		for (int i = 0; i < lockedNum; i++) {
			tmp = Random.Range (0, lockableBubbles.Count);
			tmp = lockableBubbles[tmp];
			bubbleArray[tmp].BroadcastMessage("startLocked");
			lockableBubbles.Remove(tmp);
			lockedBubbles.Add (tmp);
		}

		tmp = Random.Range (0, lockableBubbles.Count);
		tmp = lockableBubbles[tmp];
		bubbleArray[tmp].BroadcastMessage("startWithKey");

	}

	void unlockNextBubble(){
		if (lockedBubbles.Count == 0)
			return;
		tmp = lockedBubbles [0];
		lockedBubbles.RemoveAt (0);
		bubbleArray[tmp].BroadcastMessage("unlockBubble");
		bubbleArray[tmp].BroadcastMessage("startWithKey");
	}

}
