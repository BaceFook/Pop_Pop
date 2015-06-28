using UnityEngine;
using System.Collections;

public class bubbleSound : MonoBehaviour {
	public AudioClip[] popSounds;
	private AudioSource myAudio;

	void Start(){
		myAudio = GetComponent<AudioSource> ();
	}

	public void playPop(){
		myAudio.clip = popSounds [Random.Range (0, popSounds.Length)];
		myAudio.PlayDelayed(Random.Range(0.0f, 0.1f));
	}
	
}
