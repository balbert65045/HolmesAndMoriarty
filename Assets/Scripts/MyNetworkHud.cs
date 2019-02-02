using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class MyNetworkHud : Photon.PunBehaviour {

    public Text IpAddressText;
    public myNetworkManager networkManager;
    public PhotonLauncher photonLauncher;
    public GameObject StartMenu;
    public GameObject MatchesMenu;
    public GameObject ConnectingScreen;
    public GameObject LobbyScreen;
    public GameObject Canvas;

    public Text IPHost;
    public Text MatchName;

    public string MyMatchName;

	// Use this for initialization
	void Start () {
    }

    public void CreateMatch()
    {
        Debug.Log("Creating Match");
        photonLauncher = FindObjectOfType<PhotonLauncher>();
        if (MatchName.text == "") { Debug.LogWarning("Need room name!"); return;  }
        photonLauncher.MyStartMatch(MatchName.text);
    }

    public void FindRooms()
    {
        photonLauncher = FindObjectOfType<PhotonLauncher>();
        photonLauncher.FindRooms();
    }

    public void ShowRooms(RoomInfo[] Rooms)
    {
        StartMenu.SetActive(false);
        MatchesMenu.SetActive(true);
        MatchesMenu.GetComponent<matchScreen>().SetRooms(Rooms);
    }

    public void JoinRoom(RoomInfo room)
    {
        photonLauncher.JoinRoom(room);
    } 

    public void NonHostPlayerLeft()
    {
         LobbyScreen.GetComponent<LobbyScreenManager>().PlayerLeft(); 
    }


    public void ShowMatches(List<MatchInfoSnapshot> Matches)
    {
        StartMenu.SetActive(false);
        MatchesMenu.SetActive(true);
        MatchesMenu.GetComponent<matchScreen>().SetMatches(Matches);
    }

    public void JoinMatch(MatchInfoSnapshot Match)
    {
        Debug.Log("Attempting to join match " + Match.name);
        networkManager.JoinMatch(Match);
    }

    public void HostCreate()
    {
        string HostIP = networkManager.MyStartHost();
        IPHost.text = HostIP;
    }
	public void ClientConnect()
    {
        networkManager.MyStartClient(IpAddressText.text);
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(true);
        MatchesMenu.SetActive(false);
        ConnectingScreen.GetComponentInChildren<NetworkConectionPrompt>().gameObject.GetComponent<Text>().text = "Connecting To " + IpAddressText.text;
    }

    public void ShowconnectingScreen()
    {
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(true);
        MatchesMenu.SetActive(false);
        ConnectingScreen.GetComponentInChildren<NetworkConectionPrompt>().gameObject.GetComponent<Text>().text = "Rejoining Lobby";
    }

    public void StopClientConnect()
    {
  //      LobbyScreen.GetComponent<LobbyScreenManager>().ResetPlayers();
        networkManager.MyStopClient();
        StartMenu.SetActive(true);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(false);
    }

    public void CreateLobby()
    {
        Debug.Log("Creating Lobby");
    }

    public void SetMatchName(string MatchName)
    {
        MyMatchName = MatchName;
    }

    public void PlayerJoinedServer(string RoomName)
    {
        Debug.Log("Joining Server");
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(true);
        MatchesMenu.SetActive(false);

        LobbyScreen.GetComponent<LobbyScreenManager>().SetMatchName(RoomName);
    }


    public void GoBackToStart()
    {
        if (LobbyScreen.activeSelf)
        {
            LobbyScreen.GetComponent<LobbyScreenManager>().ResetPlayers();
        }

        StartMenu.SetActive(true);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(false);
        MatchesMenu.SetActive(false);
    }

    public void LeaveLobby()
    {
        photonLauncher = FindObjectOfType<PhotonLauncher>();
        photonLauncher.LeaveRoom();
    }

    public void LeaveRoomList()
    {
        photonLauncher = FindObjectOfType<PhotonLauncher>();
        photonLauncher.LeaveRoomList();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
