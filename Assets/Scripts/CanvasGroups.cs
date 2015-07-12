using UnityEngine;
using System.Collections;

public class CanvasGroups : MonoBehaviour {
	public static GameObject Auto;
	public static GameObject Lobby;
	public static GameObject Room;
	public static GameObject Game;
	public static GameObject Post;

	public GameObject _Auto;
	public GameObject _Lobby;
	public GameObject _Room;
	public GameObject _Game;
	public GameObject _Post;

	void Awake(){
		Auto = _Auto;
		Lobby = _Lobby;
		Room = _Room;
		Game = _Game;
		Post = _Post;
	}
}
