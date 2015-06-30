using UnityEngine;
using System.Collections;

public class networkController : MonoBehaviour {

	private float lastHostRequest = 4.0f;
	public Transform serverButtonPrefab;
	GameObject serverListGrid;

	void Start () {
		serverListGrid = GameObject.Find ("serverList");

		Network.InitializeServer (32, 1425, false);
		MasterServer.ipAddress = "79.98.25.158";
		MasterServer.port = 3333;
		MasterServer.RegisterHost ("good", "game");
	}

	void updateServerGrid(){
		foreach (Transform child in serverListGrid.transform) {
			GameObject.Destroy(child.gameObject);
		}
		for (int i = 0; i < MasterServer.PollHostList().Length; i++) {
			Transform button = (Transform)Instantiate(serverButtonPrefab);
			button.parent = serverListGrid.transform;
			button.GetComponent<serverButton>().assignServer(MasterServer.PollHostList()[i]);
		}
	}

	void Update (){
		if (lastHostRequest + 5.0f < Time.unscaledTime) {
			Debug.Log (MasterServer.PollHostList().Length);
			updateServerGrid();
			MasterServer.ClearHostList();
			MasterServer.RequestHostList ("good");
			lastHostRequest = Time.unscaledTime;
		}


	}
}
