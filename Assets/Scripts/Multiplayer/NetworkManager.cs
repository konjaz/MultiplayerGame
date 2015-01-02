using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private const string typeName = "TestGame";
    private const string gameName = "RoomName";
    private HostData[] hostList;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void StartServer()
    {
        //MasterServer.ipAddress = "127.0.0.1";
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }
    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        GameObject player = Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0) as GameObject;
        if (player.networkView.isMine) 
        {
            player.transform.FindChild("Main Camera").gameObject.SetActive(true);
        }
    }
    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }
    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        SpawnPlayer();
    }
    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();
            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }

        }
    }
    
}