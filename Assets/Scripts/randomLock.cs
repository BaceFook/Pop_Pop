using UnityEngine;
using System.Collections;

public class randomLock : MonoBehaviour {
	public Sprite[] lockSprites;

	void Start () {
		GetComponent<SpriteRenderer>().sprite = lockSprites[Random.Range (0, lockSprites.Length)];
	}
}
