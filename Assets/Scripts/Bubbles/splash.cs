using UnityEngine;
using System.Collections;

public class splash : MonoBehaviour {
	
	void TriggerPowerup(int id){
		gameInitiator.Instance.splashPop(id);
		Destroy(gameObject);
	}
}
