using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonLauncher : Photon.PunBehaviour {

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players and so new room will be created")]
    public byte MaxPlayersPerRoom = 2;

    public GameObject LevelPropertyManager;
    public GameObject LobbyPlayer;
    public GameObject GamePlayer;
    public string str_HolmesStartScene;
    public string str_MoriartyStartScene;
    public string str_LobbyScene;
    public string str_GameSceneName;
    public string str_ScoreSceneName;

    public string RoomName;
    bool CreateRoom = false;
    bool GetRooms = false;

    public bool LostInThoughtEnabled;

    public bool Rematch = false;
    bool LoadGameLevel = false;

    RoomOptions oldRoomOptions;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        PhotonLauncher[] photonLauncher = FindObjectsOfType<PhotonLauncher>();
        if (photonLauncher.Length > 1) {
            Destroy(photonLauncher[1].gameObject);
            Debug.Log("Deleted other launcher");
        }
    }



    // Update is called once per frame
    void Update () {
        //if (LoadGameLevel)
        //{
        //    if (Time.timeSinceLevelLoad > 5f)
        //    {
        //        Debug.Log("Loading game scene");
        //        LoadGameLevel = false;
        //        MoveToGameScene();
        //    }
        //}
	}

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
            if (FindObjectOfType<LostInThoughtToggle>().GetComponent<Toggle>().isOn)
            {
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "L0", 1 } };
            }
            else
            {
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "L0", 0 } };
            }
        
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "L0" };
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        }
        else if (Rematch)
        {
            Rematch = false;
            ReJoinRoom();
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
        RoomName = PhotonNetwork.room.Name;
        Debug.Log(PhotonNetwork.room.CustomProperties["L0"]);
        if ((int)PhotonNetwork.room.CustomProperties["L0"] == 1)
        {
            LostInThoughtEnabled = true;
        }
        else
        {
            LostInThoughtEnabled = false;
        }

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

        else if (SceneManager.GetActiveScene().name == str_MoriartyStartScene || SceneManager.GetActiveScene().name == str_HolmesStartScene)
        {
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.Disconnect();
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

    public void MoveToSplashScene(PlayerType PT)
    {
        switch (PT)
        {
            case PlayerType.Holmes:
                PhotonNetwork.LoadLevel(str_HolmesStartScene);
                break;
            case PlayerType.Moriarty:
                PhotonNetwork.LoadLevel(str_MoriartyStartScene);
                break; 
        }
    }

    public void MoveToGameScene()
    {
        Debug.Log("Attempting to load game scene");
        FindObjectOfType<LevelManager>().LoadLevel(str_GameSceneName);
    }

    public void MoveToBacktoLobby()
    {
        Rematch = true;
        FindObjectOfType<LevelManager>().LoadLevel(str_LobbyScene);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == str_HolmesStartScene || SceneManager.GetActiveScene().name == str_MoriartyStartScene)
        {
            Debug.Log("Loading GameScene");
        }

        else if (SceneManager.GetActiveScene().name == str_GameSceneName)
        {
            if (PhotonNetwork.room != null)
            {
                CreateGamePlayer();
            }
            else
            {
                FindObjectOfType<M_gameManager>().OpponentDiscconnected();
            }
        }
        else if (SceneManager.GetActiveScene().name == str_ScoreSceneName)
        {
            //TODO figure out how to make rematch work
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.Disconnect();
        }
        else if (SceneManager.GetActiveScene().name == str_LobbyScene)
        {
            if (Rematch)
            {
                FindObjectOfType<MyNetworkHud>().ShowconnectingScreen();
                Debug.Log("Rejoining lobby");
                PhotonNetwork.ConnectUsingSettings("");
                //ReJoinRoom();
            }
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

    public void ReJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        if (LostInThoughtEnabled)
        {
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "L0", 1 } };
        }
        else
        {
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "L0", 0 } };
        }
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
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
        Rematch = false;
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
