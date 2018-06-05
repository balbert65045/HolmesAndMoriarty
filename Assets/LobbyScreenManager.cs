using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreenManager : MonoBehaviour {

    public GameObject Player1Menu;
    public GameObject Player2Menu;

    private myNetworkManager MyNetworkManager;
	// Use this for initialization
	void Start () {
        MyNetworkManager = FindObjectOfType<myNetworkManager>();
    }

    public void PlayerJoin()
    {
        MyNetworkManager = FindObjectOfType<myNetworkManager>();
        Debug.Log(MyNetworkManager.numPlayers);
        if (MyNetworkManager.numPlayers == 1)
        {
            Player1Menu.SetActive(true);
        }
        else if (MyNetworkManager.numPlayers == 2)
        {
            Player2Menu.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
