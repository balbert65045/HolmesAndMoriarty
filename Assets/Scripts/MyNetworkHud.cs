using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MyNetworkHud : MonoBehaviour {

    public Text IpAddressText;
    public myNetworkManager networkManager;
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
        networkManager = FindObjectOfType<myNetworkManager>();
        networkManager.MyStartMatch(MatchName.text);
    }

    public void FindMatches()
    {
        networkManager = FindObjectOfType<myNetworkManager>();
        networkManager.FindMatches();
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

    public void StopClientConnect()
    {
        LobbyScreen.GetComponent<LobbyScreenManager>().ResetPlayers();
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

    public void PlayerJoinedServer()
    {
        Debug.Log("Joining Server");
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(true);
        MatchesMenu.SetActive(false);

        LobbyScreen.GetComponent<LobbyScreenManager>().SetMatchName(MyMatchName);
    }


    public void GoBackToStart()
    {
        StartMenu.SetActive(true);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(false);
        MatchesMenu.SetActive(false);
    }

    public void LeaveLobby()
    {
        networkManager.LeaveLobby();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
