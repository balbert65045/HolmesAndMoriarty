using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerUI : MonoBehaviour {

    public int player;
    public GameObject ChildObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DropDownChange()
    {
        LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        foreach(LobbyPlayer LP in lobbyPlayers)
        {
           // Debug.Lo
            if (LP.PlayerID == player)
            {
                LP.ValueChanged();
            }
        }
    }

    public void ToggleChange()
    {
        LobbyPlayer[] lobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        foreach (LobbyPlayer LP in lobbyPlayers)
        {
            if (LP.PlayerID == player)
            {
                LP.ToggledReady();
            }
        }
    }

}
