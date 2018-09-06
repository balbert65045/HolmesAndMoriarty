using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonLauncher : Photon.PunBehaviour {

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players and so new room will be created")]
    public byte MaxPlayersPerRoom = 2;

    public GameObject LevelPropertyManager;
    public GameObject LobbyPlayer;
    public GameObject GamePlayer;
    public string str_GameSceneName;
    public string str_ScoreSceneName;

    string RoomName;
    bool CreateRoom = false;
    bool GetRooms = false;
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
        
	}

    private void Awake()
    {
        PhotonLauncher[] photonLauncher = FindObjectsOfType<PhotonLauncher>();
        if (photonLauncher.Length > 1) { Destroy(photonLauncher[1].gameObject); }
    }

    // Update is called once per frame
    void Update () {
		
	}

    //public override void OnConnectedToMaster()
    //{
    //    //FindObjectOfType<MyNetworkHud>().PlayerJoinedServer();
    //    Debug.Log("Connected to master");
    //    base.OnConnectedToMaster();
    //    if (CreateRoom)
    //    {
    //        CreateRoom = false;
    //        RoomOptions roomOptions = new RoomOptions();
    //        roomOptions.IsVisible = false;
    //        roomOptions.MaxPlayers = 2;
    //        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
    //    }
    //    else
    //    {
    //        MyNetworkHud hud = FindObjectOfType<MyNetworkHud>();
    //        Debug.Log(PhotonNetwork.GetCustomRoomList(TypedLobby.Default, ""));
    //        Debug.Log(PhotonNetwork.GetRoomList().Length);
    //        hud.ShowRooms(PhotonNetwork.GetRoomList());
    //    }
    //}

    public override void OnJoinedLobby()
    {
        //FindObjectOfType<MyNetworkHud>().PlayerJoinedServer();
        Debug.Log("Connected to Lobby");
        base.OnJoinedLobby();
        if (CreateRoom)
        {
            CreateRoom = false;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnReceivedRoomListUpdate()
    {
        if (GetRooms)
        {
            GetRooms = false;
            MyNetworkHud hud = FindObjectOfType<MyNetworkHud>();
            Debug.Log(PhotonNetwork.GetRoomList().Length);
            hud.ShowRooms(PhotonNetwork.GetRoomList());
        }

        base.OnReceivedRoomListUpdate();
    }


    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon");
        base.OnDisconnectedFromPhoton();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room");
        PhotonNetwork.Instantiate(this.LevelPropertyManager.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        // ro
       
        FindObjectOfType<MyNetworkHud>().PlayerJoinedServer(PhotonNetwork.room.Name);
        GameObject LPlayer = PhotonNetwork.Instantiate(this.LobbyPlayer.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        //LPlayer.GetComponent<LobbyPlayerPhoton>().SetUpLobbyPlayer();
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log(otherPlayer.ID);
        // disconnect during game
        if (SceneManager.GetActiveScene().name == str_GameSceneName)
        {
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.Disconnect();
            FindObjectOfType<M_gameManager>().OpponentDiscconnected();
        }

        else if (SceneManager.GetActiveScene().name == str_ScoreSceneName)
        {

        }
        // disconnect during lobby
        else
        {
            if (otherPlayer.ID == 1)
            {
                PhotonNetwork.LeaveRoom(false);
                PhotonNetwork.Disconnect();
                FindObjectOfType<MyNetworkHud>().GoBackToStart();
            }
            else { FindObjectOfType<MyNetworkHud>().NonHostPlayerLeft(); }
        }
        
        base.OnPhotonPlayerDisconnected(otherPlayer);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void MoveToGameScene()
    {
        PhotonNetwork.LoadLevel(str_GameSceneName);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == str_GameSceneName)
        {
            CreateGamePlayer();
        }
        else if (SceneManager.GetActiveScene().name == str_ScoreSceneName)
        {
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.Disconnect();
        }
    }

    public void CreateGamePlayer()
    {
        GameObject Player = PhotonNetwork.Instantiate(this.GamePlayer.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }


    public void MyStartMatch(string matchName)
    {
        PhotonNetwork.autoJoinLobby = true;
        RoomName = matchName;
        CreateRoom = true;
        PhotonNetwork.ConnectUsingSettings("");
    } 

    public void FindRooms()
    {
        PhotonNetwork.autoJoinLobby = true;
        GetRooms = true;
        PhotonNetwork.ConnectUsingSettings("");
    }

    public void JoinRoom(RoomInfo room)
    {
        Debug.Log("Joining a room");
        PhotonNetwork.JoinRoom(room.Name);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(false);
        PhotonNetwork.Disconnect();
        FindObjectOfType<MyNetworkHud>().GoBackToStart();
    }

    public void LeaveRoomList()
    {
        PhotonNetwork.Disconnect();
        FindObjectOfType<MyNetworkHud>().GoBackToStart();
    }

    //public void Connect()
    //{
    //    if (PhotonNetwork.connected)
    //    {
    //        Debug.Log("Joined a random room");
    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        Debug.Log("Created a random room");
    //        PhotonNetwork.ConnectUsingSettings("");
    //    }
    //}

}
