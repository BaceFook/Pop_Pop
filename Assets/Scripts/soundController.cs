using UnityEngine;
using System.Collections;

public class soundController : MonoBehaviour {
	public AudioClip[] popSounds;
	private AudioSource myAudio;

	void Start(){
		myAudio = GetComponent<AudioSource> ();
	}

	void playPop(){
		myAudio.clip = popSounds [Random.Range (0, popSounds.Length)];
		myAudio.Play ();
	}
	
}
