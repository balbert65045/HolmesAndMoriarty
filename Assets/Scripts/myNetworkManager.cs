using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myNetworkManager : NetworkLobbyManager {

    public MyNetworkHud Hud;
    NetworkClient ClientWorkingWith;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void MyStartHost()
    {
        StartHost();
        Debug.Log(Time.timeSinceLevelLoad + " Host started");
    }

    public void MyStartClient(string IPAddress)
    {
        Debug.Log(Time.timeSinceLevelLoad + " Client attempting to connect to " + IPAddress);
        ClientWorkingWith = StartClient();
        ClientWorkingWith.Connect(IPAddress, 7777);
    }

    public void MyStopClient()
    {
        Debug.Log("Disconnecting client");
        StopClient();
    }

    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
        
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Lobby Player Called");
        GameObject Player = base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
        Debug.Log(Player);
        // Debug.Log(LobbyPlayer);
        //Hud.PlayerJoinedServer(LobbyPlayer);
        return Player;

    }




    public override void OnStartHost()
    {
        base.OnStartHost();
         Hud.CreateLobby(); 
        Debug.Log(Time.timeSinceLevelLoad + " Host requested");
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

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Creating Game Player");
        return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Added player");
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("Client scene changed");
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("lobby player " + lobbyPlayer);
        Debug.Log("Game player " + gamePlayer);

        if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 1) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player1Player); }
        else if (lobbyPlayer.GetComponent<LobbyPlayer>().PlayerID == 2) { gamePlayer.GetComponent<M_PlayerController>().SetPlayerType(FindObjectOfType<LevelPropertyManagerMulti>().Player2Player); }
        else { Debug.Log("something went wrong"); }

        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }
}
