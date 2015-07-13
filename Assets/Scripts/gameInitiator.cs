using UnityEngine;
using UnityEngine.UI;
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
	public int markUnpoppedNum = 5;
	public int splashNum = 5;

	public Transform normalBubble;
	public Transform markUnpoppedPrefab;
	public Transform splashPrefab;
	public Transform[] bubbleArray;

	List<int>normalBubbles = new List<int>();
	List<int>poppableBubbles = new List<int>();
	List<int>lockableBubbles = new List<int>();

	List<int>horizontalPairs = new List<int>();
	List<int>lockedBubbles = new List<int>();
	
	private int tmp;
	private float tmpf;
	
	private static gameInitiator instance = null;
	
	public static gameInitiator Instance {
		get {
			return instance;
		}
	}
	
	void Awake () {
		instance = this;
	}

	void Start () {
		// Use random seed to generate same results
		// Random.seed = 0;

		bubblesParent = GameObject.Find ("BubblesParent");
		bubbleArray = new Transform[bubblesX * bubblesY];

		float startX = -1.0f * (bubblesX-1);
		float startY = -1.0f * (bubblesY-1);

		MultitouchMovement cameraMovement = Camera.main.GetComponent<MultitouchMovement> ();
		float cameraSize = Camera.main.orthographicSize;
		tmpf = Mathf.Max (bubblesY - cameraSize, 0);
		cameraMovement.topBorder = tmpf;		
		cameraMovement.bottomBorder = - tmpf;
		tmpf = Mathf.Max (bubblesX - cameraSize * Camera.main.aspect, 0);
		cameraMovement.rightBorder = tmpf;
		cameraMovement.leftBorder = - tmpf;

		for (int i = 0; i < bubblesX; i++) {
			for (int j = 0; j < bubblesY; j++) {
				tmp = i + j * bubblesX;
				bubbleArray[tmp] = (Transform)Instantiate (normalBubble, new Vector3(startX + 2.0f * i, startY + 2.0f * j, 0), Quaternion.identity);
				bubbleArray[tmp].parent = bubblesParent.GetComponent<Transform>();
				bubbleArray[tmp].GetComponent<bubbleState>().id = tmp;
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

		// Give mark unpopped powerups
		for (int i = 0; i < markUnpoppedNum; i++) {
			tmp = Random.Range (0, normalBubbles.Count);
			tmp = normalBubbles[tmp];
			Transform prefab = (Transform)Instantiate (markUnpoppedPrefab);
			prefab.parent = bubbleArray[tmp].transform;
			bubbleArray[tmp].GetComponent<bubbleState>().hasPowerup = true;
			prefab.localPosition = Vector3.zero;
			normalBubbles.Remove (tmp);
			poppableBubbles.Remove (tmp);
			
		}
		
		// Give mark splash powerups
		for (int i = 0; i < splashNum; i++) {
			tmp = Random.Range (0, normalBubbles.Count);
			tmp = normalBubbles[tmp];
			Transform prefab = (Transform)Instantiate (splashPrefab);
			prefab.parent = bubbleArray[tmp].transform;
			bubbleArray[tmp].GetComponent<bubbleState>().hasPowerup = true;
			prefab.localPosition = Vector3.zero;
			normalBubbles.Remove (tmp);
			poppableBubbles.Remove (tmp);
			
		}

		// Set up connected pairs
		for (int i = 0; i < connectedPairs; i++) {
			tmp = Random.Range (0, horizontalPairs.Count);
			tmp = horizontalPairs[tmp];

			int leftNum = tmp + Mathf.FloorToInt(tmp / (bubblesX - 1.0f));
			int rightNum = tmp + Mathf.FloorToInt(tmp / (bubblesX - 1.0f) + 1);

			if(Random.Range (0, 2) == 0)
				lockableBubbles.Remove(leftNum);
			else
				lockableBubbles.Remove(rightNum);
			
			horizontalPairs.Remove(tmp);
			if((tmp + 1) % (bubblesX - 1) != 0 && tmp != (bubblesX - 1) * bubblesY - 1)
				horizontalPairs.Remove(tmp + 1);
			if(tmp % (bubblesX - 1) != 0)
				horizontalPairs.Remove(tmp - 1);

			poppableBubbles.Remove(leftNum);
			poppableBubbles.Remove(rightNum);
		}

		// Pop random bubbles

		for (int i = 0; i < poppedNum; i++) {
			tmp = Random.Range (0, poppableBubbles.Count);
			tmp = poppableBubbles[tmp];
			bubbleArray[tmp].BroadcastMessage("startPopped");
			poppableBubbles.Remove(tmp);
			lockableBubbles.Remove(tmp);
		}

		// Lock random bubbles
		
		for (int i = 0; i < lockedNum; i++) {
			tmp = Random.Range (0, lockableBubbles.Count);
			tmp = lockableBubbles[tmp];
			bubbleArray[tmp].BroadcastMessage("startLocked");
			lockableBubbles.Remove(tmp);
			lockedBubbles.Add (tmp);
		}

		// Give key to some bubble

		tmp = Random.Range (0, lockableBubbles.Count);
		tmp = lockableBubbles[tmp];
		bubbleArray[tmp].BroadcastMessage("startWithKey");

	}

	public void unlockNextBubble(){
		if (lockedBubbles.Count == 0)
			return;
		tmp = lockedBubbles [0];
		lockedBubbles.RemoveAt (0);
		bubbleArray[tmp].BroadcastMessage("unlockBubble");
		bubbleArray[tmp].BroadcastMessage("startWithKey");
	}

	public void splashPop(int bubbleId){
		if (bubbleId >= bubblesX) {
			bubbleArray [bubbleId - bubblesX].GetComponent<bubbleState> ().popBubble ();
		}
		if (bubbleId < bubblesX * (bubblesY - 1)) {
			bubbleArray [bubbleId + bubblesX].GetComponent<bubbleState> ().popBubble ();
		}
		if (bubbleId % bubblesX != 0) {
			bubbleArray [bubbleId - 1].GetComponent<bubbleState> ().popBubble ();
		}
		if ((bubbleId + 1) % bubblesX != 0) {
			bubbleArray [bubbleId + 1].GetComponent<bubbleState> ().popBubble ();
		}
	}
	
	public void markUnpopped(){
		for (int i = 0; i < bubbleArray.Length; i++) {
			bubbleArray[i].GetComponent<bubbleState>().highlightBubble();
		}
	}

	public void popped(int bubbleId){
		bool skip = false;
		tmp = Mathf.FloorToInt (bubbleId / bubblesX);
		for (int i = 0; i <= bubblesX; i++) {
			if (i == bubblesX){
				for (int j = 0; j < bubblesX; j++) {
					bubbleArray[tmp * bubblesX + j].GetComponent<bubbleState>().lowlightBubble();
				}
				break;
			}

			if (!bubbleArray[tmp * bubblesX + i].GetComponent<bubbleState>().isPopped){
				skip = true;
				break;
			}
		}

		tmp = bubbleId % bubblesX;
		for (int i = 0; i <= bubblesY; i++) {
			if (i == bubblesY){
				for (int j = 0; j < bubblesY; j++) {
					bubbleArray[tmp + j * bubblesX].GetComponent<bubbleState>().lowlightBubble();
				}
				break;
			}
			
			if (!bubbleArray[tmp + i * bubblesX].GetComponent<bubbleState>().isPopped){
				break;
				skip = true;
			}
		}

		if (skip)
			return;

		for (int i = 0; i < bubbleArray.Length; i++) {
			if(!bubbleArray[i].GetComponent<bubbleState>().isPopped)
				return;
		}

		GameObject.Find ("Canvas").transform.FindChild ("Victory").GetComponent<Image> ().enabled = true;
	}
}
