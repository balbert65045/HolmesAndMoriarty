using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetworkHud : MonoBehaviour {

    public Text IpAddressText;
    public myNetworkManager networkManager;
    public GameObject StartMenu;
    public GameObject ConnectingScreen;
    public GameObject LobbyScreen;
    public GameObject Canvas;

    public Text IPHost;

	// Use this for initialization
	void Start () {
        networkManager = FindObjectOfType<myNetworkManager>();
        if (networkManager == null) { Debug.LogError("No network manager found"); }
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
        ConnectingScreen.GetComponentInChildren<NetworkConectionPrompt>().gameObject.GetComponent<Text>().text = "Connecting To " + IpAddressText.text;
    }

    public void StopClientConnect()
    {
        networkManager.MyStopClient();
        StartMenu.SetActive(true);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(false);
    }

    public void CreateLobby()
    {
        Debug.Log("Creating Lobby");
    }

    public void PlayerJoinedServer()
    {
        Debug.Log("Joining Server");
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(true);
    }

    public void LeaveLobby()
    {
        networkManager.LeaveLobby();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
