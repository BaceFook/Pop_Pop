using UnityEngine;
using System.Collections;

public class menuController : MonoBehaviour {

	public void LoadLevel(string name){
		Debug.Log (name);
		Application.LoadLevel (name);
	}
}
