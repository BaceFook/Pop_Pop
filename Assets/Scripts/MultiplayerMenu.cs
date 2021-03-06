﻿using UnityEngine;
using System.Collections;

public class MultiplayerMenu : MonoBehaviour {
	public static GameObject Auto;
	public static GameObject Lobby;
	public static GameObject Room;
	public static GameObject Game;
	public static GameObject Post;
	public static GameObject ModeButtons;
	
	public GameObject _Auto;
	public GameObject _Lobby;
	public GameObject _Room;
	public GameObject _Game;
	public GameObject _Post;
	public GameObject _ModeButtons;
	
	void Awake(){
		Auto = _Auto;
		Lobby = _Lobby;
		Room = _Room;
		Game = _Game;
		Post = _Post;
		ModeButtons = _ModeButtons;
	}
	
	void Start(){
		ToAuto ();
	}

	public static void ToAuto(){
		Auto.SetActive (true);
		Lobby.SetActive (false);
		Room.SetActive (false);
		Game.SetActive (false);
		Post.SetActive (false);
	}

	public void Leave(){
		NetworkController.instance.LeaveMatch ();
	}

	public void Rematch(){
		NetworkController.instance.Rematch ();
	}
}
