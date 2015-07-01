using UnityEngine;
using System.Collections;

public class spriteRandomizer : MonoBehaviour {

	private float phase;
	// Use this for initialization
	void Start () {
		phase = Random.Range (0f, Mathf.PI * 2f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (1f, Mathf.Sin (Time.time + phase) * 2f, 1f);
		Debug.Log (transform.localPosition);
	}
}
