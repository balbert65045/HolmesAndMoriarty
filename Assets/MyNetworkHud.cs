using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkHud : MonoBehaviour {

    public Text IpAddressText;
    myNetworkManager networkManager;
    public GameObject StartMenu;
    public GameObject ConnectingScreen;
    public GameObject LobbyScreen;

	// Use this for initialization
	void Start () {
        networkManager = FindObjectOfType<myNetworkManager>();
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
    }

    public void PlayerJoinedServer()
    {
        StartMenu.SetActive(false);
        ConnectingScreen.SetActive(false);
        LobbyScreen.SetActive(true);
        LobbyScreen.GetComponent<LobbyScreenManager>().PlayerJoin();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
