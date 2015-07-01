using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class punNetwork : MonoBehaviour {

	void Start () {
		PhotonNetwork.ConnectUsingSettings (Application.version);
	}

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(Random.Range(0.0f, 1.0f).ToString(), true, true, 2);
	}

	void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient)
			GameObject.Find ("userInfo").GetComponent<Text>().text = "MasterClient";
		else
			GameObject.Find ("userInfo").GetComponent<Text>().text = "ConnectedToMasrter";
	}

}
