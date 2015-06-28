using UnityEngine;
using System.Collections;

public class markUnpopped : MonoBehaviour {

	void TriggerPowerup(){
		gameInitiator.Instance.markUnpopped();
	}
}
