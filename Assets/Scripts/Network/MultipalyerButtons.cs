using UnityEngine;
using System.Collections;

public class MultipalyerButtons : MonoBehaviour {

	public void ToggleSwipeEnabled(){
		NetworkPlayer.myPlayer.CmdToggleSwipe (!NetworkPlayer.myPlayer.swipeEnabled);
	}
}
