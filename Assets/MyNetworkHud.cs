﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetworkHud : NetworkBehaviour {

    public Text IpAddressText;
    public myNetworkManager networkManager;
    public GameObject StartMenu;
    public GameObject ConnectingScreen;
    public GameObject LobbyScreen;
    public GameObject Canvas;

	// Use this for initialization
	void Start () {
        networkManager = FindObjectOfType<myNetworkManager>();
        if (networkManager == null) { Debug.LogError("No network manager found"); }
    }

    public void HostCreate()
    {
        networkManager.MyStartHost();
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
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(true);
    }

    public void LeaveLobby()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
