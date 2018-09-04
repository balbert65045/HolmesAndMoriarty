using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        LobbyPlayerPhoton[] lobbyPlayers = FindObjectsOfType<LobbyPlayerPhoton>();
        foreach(LobbyPlayerPhoton LP in lobbyPlayers)
        {
           // Debug.Lo
            if (LP.LocalPlayer)
            {
                LP.ValueChanged();
            }
        }
    }

    public void ToggleChange(bool Value)
    {
        LobbyPlayerPhoton[] lobbyPlayers = FindObjectsOfType<LobbyPlayerPhoton>();
        foreach (LobbyPlayerPhoton LP in lobbyPlayers)
        {
            if (LP.LocalPlayer)
            {
                Debug.Log("Toggle Changed");
                LP.ToggledReady(Value);
            }
        }
    }

}
