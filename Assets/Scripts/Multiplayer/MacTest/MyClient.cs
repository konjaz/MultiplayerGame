using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyClient : MonoBehaviour {
    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public static BulletScript[] bulletsPool;
    public BulletScript[] bulletsPool2;
    PhotonView photonView;
    public List<CharacterSystem> listOfPlayers; 
    
    void Awake()
    {
        listOfPlayers = new List<CharacterSystem>();
        bulletsPool = bulletsPool2;
        photonView = GetComponent<PhotonView>();
    }
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
            {
                PhotonNetwork.CreateRoom(roomName + Random.Range(0,99999));
            }
                //PhotonNetwork.CreateRoom(roomName, true, true, 5);
                
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
    //void OnJoinedLobby()
    //{
    //    PhotonNetwork.JoinRandomRoom();
    //}
    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }
     
    void OnJoinedRoom()
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        //newPlayer.name = "Player " + photonView.ownerId;
        //newPlayer.transform.GetComponentInChildren<TextMesh>().text = newPlayer.name;
        
        newPlayer.transform.FindChild("Main Camera").gameObject.SetActive(true);
        newPlayer.rigidbody.isKinematic = false;
        //myAvatar = newPlayer.GetComponent<CharacterSystem>();
        listOfPlayers.Add(newPlayer.GetComponent<CharacterSystem>());
        Debug.Log("Connected to Room");
        
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) 
    {
        foreach (CharacterSystem avatar in listOfPlayers)
        {
            avatar.SetName();
        }
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
    }

    const int NUMBER_OF_BULLETS_IN_POOL = 200;

    //void BulletPool()
    //{
    //    bulletsPool = new BulletScript[NUMBER_OF_BULLETS_IN_POOL];
    //    for (int i = 0; i < NUMBER_OF_BULLETS_IN_POOL; i++)
    //    {
    //        GameObject bul = PhotonNetwork.Instantiate(bulletPrefab.name, Vector3.zero, Quaternion.identity, 0);
    //        bulletsPool[i] = bul.GetComponent<BulletScript>();
    //        bulletsPool[i].transform.parent = bulletParent;
    //        bulletsPool[i].name = bulletPrefab.name;
    //        bulletsPool[i].DeactivateBullet();
    //    }
    //    //bulletsPool2 = bulletsPool;
    //}

    public static BulletScript GetInactiveBullet()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
        {
            //Debug.Log("bullet " + i);
            if (bulletsPool[i].gameObject.activeSelf == false)
            {
                return bulletsPool[i];
            }
        }
        Debug.LogError("Error: BattleSystem  Not enought bullets prespawned to shoot with");
        return null;
    }
}
