using UnityEngine;
using System.Collections;

public class MyClient : MonoBehaviour {
    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public GameObject playerPrefab;
	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");
	}

    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            // Create Room
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                PhotonNetwork.CreateRoom(roomName, true, true, 5);
 
            // Join Room
            if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
                        PhotonNetwork.JoinRoom(roomsList[i].name);
                }
            }
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }
    void OnJoinedRoom()
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        newPlayer.transform.FindChild("Main Camera").gameObject.SetActive(true);
        newPlayer.rigidbody.isKinematic = false;
        Debug.Log("Connected to Room");
    }
}
