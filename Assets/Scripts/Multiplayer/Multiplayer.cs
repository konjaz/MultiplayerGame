using UnityEngine;
using System.Collections;

public class Multiplayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void ResetIP() {
        Network.connectionTesterIP = "127.0.0.1";
        Network.connectionTesterPort = 10000;
    }
    void ConnectToServer()
    {
        Debug.Log("ConnectToServer");
        Network.Connect("127.0.0.1", 25000, "password");
    }
    void LaunchServer()
    {
        Debug.Log("LaunchServer");
        Network.incomingPassword = "password";
        Network.InitializeServer(8, 25000);
    }
    void OnGUI() {
        if (Network.isServer)
            GUILayout.Label("Running as a server");
        else
            if (Network.isClient)
                GUILayout.Label("Running as a client");
        if (GUILayout.Button("Disconnected first player"))
            if (Network.connections.Length > 0)
            {
                Debug.Log("Disconnectiong: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
                Network.CloseConnection(Network.connections[0], true);
            }
    }
}
