using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class myNetworkManager : NetworkLobbyManager {

    public MyNetworkHud Hud;
    NetworkClient ClientWorkingWith;

   // NetworkID netID;
   // NetworkMatch match;

    string originalLobbyScene;
    string originalGameScene;

    List<short> PlayerIDs = new List<short>();

    private void Start()
    {
        Debug.Log("Something restarted the scene");
        Hud = FindObjectOfType<MyNetworkHud>();
        originalLobbyScene = lobbyScene;
        originalGameScene = playScene;
        offlineScene = "";
        onlineScene = lobbyScene;
        //playScene = "";
        //lobbyScene = "";
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("New level loaded int is " + level);
    }

    // void Awake()
    //{
    //    //DontDestroyOnLoad(transform.gameObject);

    //    //LobbyPlayer[] LP = FindObjectsOfType<LobbyPlayer>();
    //    //foreach (LobbyPlayer lobbyPlayer in LP) { Destroy(lobbyPlayer.gameObject); }
    //    //myNetworkManager[] NetworkManagers = FindObjectsOfType<myNetworkManager>();

    //    //if (NetworkManagers.Length > 1)
    //    //{
    //    //    Debug.Log("Destroyed duplicate");
    //    //    Destroy(NetworkManagers[0].gameObject);
    //    //}
    //    //Hud = FindObjectOfType<MyNetworkHud>();

    //}

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("Server changing SCENE");
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnStartServer()
    {
        lobbyScene = originalLobbyScene; // Ensures the server loads correctly
        base.OnStartServer();
    }

    public void EnablePlayScene()
    {
        playScene = originalGameScene;
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
   //     netID = matchInfo.networkId;
        Debug.Log("Match Created at " + Time.timeSinceLevelLoad);
        base.OnMatchCreate(success, extendedInfo, matchInfo);
    }

    public void MyStartMatch(string LobbyName)
    {
        Hud = FindObjectOfType<MyNetworkHud>();
        Debug.Log("Start Match Maker");
        StartMatchMaker();
        matchMaker.CreateMatch(LobbyName, 2, true, "", "", "", 0, 0, OnMatchCreate);
        Hud.SetMatchName(LobbyName);
    }

    public void FindMatches()
    {
        Hud = FindObjectOfType<MyNetworkHud>();
        StartMatchMaker();
        matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        Debug.Log("Show Match");
        if (success)
        {
            Hud.ShowMatches(matchList);
        }
        base.OnMatchList(success, extendedInfo, matchList);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Player added to the server");
        PlayerIDs.Add(playerControllerId);
        foreach (short id in PlayerIDs)
        {
            Debug.Log(id);
        }
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log("Number of players active is " + numPlayers);
        
    }


    public void JoinMatch(MatchInfoSnapshot Match)
    {
        Debug.Log("Joining match");
        playScene = "";
        matchMaker.JoinMatch(Match.networkId, "", "", "", 0, 0, OnMatchJoined);
       
        NetworkServer.SetAllClientsNotReady();
        Hud.SetMatchName(Match.name);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("Match Joined");
        if (success)
        {
            Debug.Log("Success");
            StartClient(matchInfo);

        }
           // base.OnMatchJoined(success, extendedInfo, matchInfo);
    }


    public string MyStartHost()
    {
        Hud = FindObjectOfType<MyNetworkHud>();
        StartHost();
        Debug.Log(Time.timeSinceLevelLoad + " Host started");
        return Network.player.ipAddress.ToString(); 
        
    }

    public void MyStartClient(string IPAddress)
    {
        Hud = FindObjectOfType<MyNetworkHud>();
        Debug.Log(Time.timeSinceLevelLoad + " Client attempting to connect to " + IPAddress);
        ClientWorkingWith = StartClient();
        ClientWorkingWith.Connect(IPAddress, 7777);
    }

    public override void OnStartClient(NetworkClient lobbyClient)
    {
        Debug.Log("Start Client");
        lobbyScene = originalLobbyScene;
        playScene = "";
        base.OnStartClient(lobbyClient);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client connected");
        //Hud = FindObjectOfType<MyNetworkHud>();
        //Hud.PlayerJoinedServer();
        //ClientScene.Ready(conn);
        //ClientScene.AddPlayer(conn, 0);
        //onlineScene = originalLobbyScene;
        Debug.Log(onlineScene);
        Debug.Log(offlineScene);
         base.OnClientConnect(conn);
    }


    public void MyStopClient()
    {
        Debug.Log("Disconnecting client");
        StopClient();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        lobbyScene = "";
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped");
        lobbyScene = "";
        base.OnStopServer();
    }

    public override void OnLobbyStopHost()
    {
        lobbyScene = "";
        base.OnLobbyStopHost();
    }


    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject Player = base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
        return Player;

    }

    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        Debug.Log("Match Destroyed");
        base.OnDestroyMatch(success, extendedInfo);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //Debug.Log("Client disconnected from client");
        //singleton.matchMaker.DestroyMatch(netID, 0, OnDestroyMatch);
        //if (FindObjectOfType<M_gameManager>() != null) { FindObjectOfType<M_gameManager>().OpponentDiscconnected(); }
        //else
        //{
        //    LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        //    foreach (LobbyPlayer LP in lobbyPlayers)
        //    {
        //        if (LP.connectionToClient == conn)
        //        {
        //            Debug.Log("Found Matching client");
        //            LP.DisablePlayerUI();
        //        }
        //    }
        //}
        //LobbyPlayer[] MylobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        //foreach (LobbyPlayer LP in MylobbyPlayers)
        //{
        //    LP.SendNotReadyToBeginMessage();
        //    Destroy(LP.gameObject);
        //}

        base.OnClientDisconnect(conn);
    }

    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Player removed");
        base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
    }

   

    //TODO this needs work
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client disconnected from server");
        //LobbyPlayer[] ids = FindObjectsOfType<LobbyPlayer>();
        //foreach (LobbyPlayer lp in ids)
        //{
        //    if (lp.GetComponent<NetworkIdentity>().connectionToClient == conn) {
        //        Debug.Log("Removing player");
        //        ClientScene.RemovePlayer(lp.GetComponent<NetworkIdentity>().playerControllerId);
        //    }
        //}
        //base.OnServerDisconnect(conn);
        if (FindObjectOfType<M_gameManager>() != null) {

            FindObjectOfType<M_gameManager>().OpponentDiscconnected();
            LobbyPlayer[] MylobbyPlayers = FindObjectsOfType<LobbyPlayer>();
            foreach (LobbyPlayer LP in MylobbyPlayers)
            {
                if (LP != null)
                {
                    Debug.Log("Removing player");
                    LP.RemovePlayer();
                    Destroy(LP.gameObject);
                }
            }

            M_PlayerController[] Controllers = FindObjectsOfType<M_PlayerController>();
            foreach (M_PlayerController Controller in Controllers)
            {
                if (Controller != null)
                {
                    Debug.Log("Destroying Controllers");
                    //   LP.RemovePlayer();
                    Destroy(Controller.gameObject);
                }
             }

            NetworkIdentity[] ids = FindObjectsOfType<NetworkIdentity>();
            foreach (NetworkIdentity id in ids)
            {
                Destroy(id.gameObject);
            }

            // matchMaker.
        //S    matchMaker.DestroyMatch(netID, 0, OnMatchDestroy);
            StopMatchMaker();
            StopHost();
            

            //Shutdown();
            //Destroy(this.gameObject);
            Debug.Log("Server leaving");
        }
        else
        {
            LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
            foreach (LobbyPlayer LP in lobbyPlayers)
            {
                if (LP.connectionToClient == conn)
                {
                    Debug.Log("Found Matching client");
                    LP.DisablePlayerUI();
                    PlayerLeftLobby(LP);
                }
            }
        }
        
    }

    public void OnMatchDestroy(bool success, string extendedInfo)
    {
        Debug.Log("Destroyed match");
    }

    public override void OnStartHost()
    {
       
        base.OnStartHost();
        Hud.CreateLobby(); 
        
        Debug.Log(Time.timeSinceLevelLoad + " Host requested");
    }

    public override void OnLobbyClientExit()
    {
        Debug.Log("Client exited");
        base.OnLobbyClientExit();
    }

    public override void OnStopHost()
    {
        Debug.Log("Host Stopped");
        base.OnStopHost();
    }

    public override void OnLobbyStartClient(NetworkClient myClient)
    {
        playScene = "";
        Debug.Log(ClientScene.ready);
        base.OnLobbyStartClient(myClient);
        Debug.Log(Time.timeSinceLevelLoad + " Client start requested" );
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
      //  Hud.PlayerJoinedServer();
        Debug.Log(onlineScene);
        base.OnLobbyClientConnect(conn);
        Debug.Log(Time.timeSinceLevelLoad + " Client connected to IP:" + conn.address);
        Debug.Log(Network.player.ipAddress);
    }


    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        base.OnLobbyClientDisconnect(conn);
        Hud.StopClientConnect(); 

    }

    public void PlayerLeftLobby(LobbyPlayer LP)
    {
        //Find a way to make this right

        if (LP != null && LP.isServer)
        {
            LP.Disconnect(); 
            StopHost();
        }
        else { LP.Disconnect(); }
    }


    public void LeaveLobby()
    {
        //Find a way to make this right
        LobbyPlayer[] Players = FindObjectsOfType<LobbyPlayer>();
        LobbyPlayer MyLobbyPlayer = null;
        foreach (LobbyPlayer LP in Players)
        {
            if (LP.isLocalPlayer) { MyLobbyPlayer = LP; }
        }
       
        if (MyLobbyPlayer != null && MyLobbyPlayer.isServer)
        {
            foreach (LobbyPlayer LP in Players)
            {
                if (LP != MyLobbyPlayer) { LP.Disconnect(); }
            }
            StopHost();
        }
        else { FindObjectOfType<MyNetworkHud>().StopClientConnect() ; }
    }

    //public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    Debug.Log("Creating Game Player");
    //    return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    //}

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    Debug.Log("Added player");
    //    base.OnServerAddPlayer(conn, playerControllerId);
    //}

    //public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    //{
    //    Debug.Log("Client scene changed");
    //}

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("CHANGING SCENE");
        base.OnClientSceneChanged(conn);
    }



    //public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    //{
    //    Debug.Log("Loading game for client");
    //    if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 1) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player1Player, lobbyPlayer.GetComponent<LobbyPlayer>()); }
    //    else if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 2) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player2Player, lobbyPlayer.GetComponent<LobbyPlayer>()); }
    //    else { Debug.Log("something went wrong"); }

    //    return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    //}
}
