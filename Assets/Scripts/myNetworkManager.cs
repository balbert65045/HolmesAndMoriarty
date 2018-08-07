using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myNetworkManager : NetworkLobbyManager {

    public MyNetworkHud Hud;
    NetworkClient ClientWorkingWith;

    private void Start()
    {
        Debug.Log("Something restarted the scene");
        Hud = FindObjectOfType<MyNetworkHud>();
    }

    void Awake()
    {
       
        DontDestroyOnLoad(transform.gameObject);
        myNetworkManager[] NetworkManagers = FindObjectsOfType<myNetworkManager>();

        if (NetworkManagers.Length > 1)
        {
            Debug.Log("Destroyed duplicate");
            Destroy(NetworkManagers[1].gameObject);
        }
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

    public void MyStopClient()
    {
        Debug.Log("Disconnecting client");
        StopClient();
    }


    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject Player = base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
        return Player;

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        foreach (LobbyPlayer LP in lobbyPlayers)
        {
            if (LP.connectionToClient == conn)
            {
                Debug.Log("Found Matching client");
                LP.DisablePlayerUI();
            }
        }
        base.OnClientDisconnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        foreach (LobbyPlayer LP in lobbyPlayers)
        {
            if (LP.connectionToClient == conn)
            {
                Debug.Log("Found Matching client");
                LP.DisablePlayerUI();
            }
        }
        Debug.Log("Server leaving"); 
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
        base.OnLobbyStartClient(myClient);
        Debug.Log(Time.timeSinceLevelLoad + " Client start requested" );
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        Hud.PlayerJoinedServer();
        base.OnLobbyClientConnect(conn);
        Debug.Log(Time.timeSinceLevelLoad + " Client connected to IP:" + conn.address);
        Debug.Log(Network.player.ipAddress);
    }


    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        base.OnLobbyClientDisconnect(conn);
        Hud.StopClientConnect(); 

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
        else { MyStopClient(); }
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

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {

        if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 1) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player1Player, lobbyPlayer.GetComponent<LobbyPlayer>()); }
        else if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 2) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player2Player, lobbyPlayer.GetComponent<LobbyPlayer>()); }
        else { Debug.Log("something went wrong"); }

        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }
}
