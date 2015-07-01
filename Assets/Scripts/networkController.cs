using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class networkController : MonoBehaviour {

	private float lastHostRequest = 4.0f;
	public Transform serverButtonPrefab;
	GameObject serverListGrid;

	void Start () {
		serverListGrid = GameObject.Find ("serverList");
		Network.natFacilitatorIP = "79.98.25.158";
		Network.natFacilitatorPort = 4444;
		MasterServer.ipAddress = "79.98.25.158";
		MasterServer.port = 3333;
	}

	public void startServer(){
		Network.InitializeServer (32, 1425, true);
	}
	void OnServerInitialized (NetworkPlayer player){
		MasterServer.RegisterHost ("good", "game" + Random.Range (0.0f, 1.0f).ToString());
		
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

		if (Network.isServer) {
			GameObject.Find("connections").GetComponent<Text>().text = Network.connections.Length.ToString();
		}


	}
}
