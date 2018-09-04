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

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnConnectedToMaster()
    {
        //FindObjectOfType<MyNetworkHud>().PlayerJoinedServer();
        Debug.Log("Connected to master");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("Ben5's Room", roomOptions, TypedLobby.Default);
        base.OnConnectedToMaster();
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
        FindObjectOfType<MyNetworkHud>().PlayerJoinedServer();
        GameObject LPlayer = PhotonNetwork.Instantiate(this.LobbyPlayer.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        //LPlayer.GetComponent<LobbyPlayerPhoton>().SetUpLobbyPlayer();
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
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
    }

    public void CreateGamePlayer()
    {
        GameObject Player = PhotonNetwork.Instantiate(this.GamePlayer.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        //LobbyPlayerPhoton[] LobbyPlayers = FindObjectsOfType<LobbyPlayerPhoton>();
        //foreach (LobbyPlayerPhoton LP in LobbyPlayers)
        //{
        //    if (LP.PlayerID == 1 && LP.PlayerID == Player.GetPhotonView().ownerId) { Player.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player1Player, LP); }
        //    else if (LP.PlayerID == 2 && LP.PlayerID == Player.GetPhotonView().ownerId) { Player.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player1Player, LP); }
        //    else Debug.LogError("Something went wrong");
        //}
    }



    public void Connect()
    {
        if (PhotonNetwork.connected)
        {
            Debug.Log("Joined a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Created a random room");
            PhotonNetwork.ConnectUsingSettings("");
        }
    }

}
