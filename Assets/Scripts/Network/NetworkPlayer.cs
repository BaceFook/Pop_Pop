using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour {
	public static NetworkPlayer myPlayer;
	public static NetworkPlayer enemyPlayer;

	GameObject modeButton;
	GameObject swipeToggleButton;

	[HideInInspector, SyncVar(hook="OnRematch")]
	public bool rematch = false;
	
	[SyncVar(hook="OnPlayerSyncMode")]
	public NetworkController.GameMode selectedMode = NetworkController.GameMode.PvsP;
	
	[SyncVar(hook="OnSyncSwipeEnabled")]
	public bool swipeEnabled = false;

	public int bubbles = 1;

	public void Restart(){
		bubbles = 1;
		OnRematch (false);
	}
	
	void OnPlayerSyncMode(NetworkController.GameMode mode){
		selectedMode = mode;
	}
	
	void OnSyncSwipeEnabled(bool se){
		swipeEnabled = se;
		swipeToggleButton.GetComponentInChildren<Text> ().text = se ? "Swipe pop" : "Tap pop";
	}

	void Start(){
		if (isLocalPlayer) {
			myPlayer = this;
			modeButton = GameObject.Find ("MyModeButton");
			swipeToggleButton = GameObject.Find ("MyToggleSwipeButton");
		} else {
			enemyPlayer = this;
			modeButton = GameObject.Find ("EnemyModeButton");
			swipeToggleButton = GameObject.Find ("EnemyToggleSwipeButton");
		}
	}

	public NetworkPlayer OtherPlayer(){
		if (isLocalPlayer)
			return NetworkPlayer.enemyPlayer;
		else
			return this;
	}

	void OnRematch (bool r){
		rematch = r;
		if (!isLocalPlayer && r) {
			GameObject.Find ("LeaveButton").GetComponentInChildren<Text>().text = "Leave rematch";
			GameObject.Find ("RematchButton").GetComponentInChildren<Text>().text = "Accept rematch!";
		}
	}

	[Command]
	public void CmdToggleSwipe(bool se){
		OnSyncSwipeEnabled (se);
	}

	[Command]
	public void CmdRematch(){
		if (OtherPlayer ().rematch) {
			NetworkController.instance.RpcRematch();
			return;
		}
		OnRematch(true);
	}

	[Command]
	public void CmdPvsPpops (int number){
		NetworkController.instance.GetComponent<PvsPcontroller> ().RpcPops(number, netId);
	}
}
