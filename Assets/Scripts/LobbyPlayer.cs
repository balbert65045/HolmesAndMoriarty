﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour {

    
    public PlayerType PT = PlayerType.Holmes;
    public bool Ready = false;

    public int PlayerID = 0;

    LobbyPlayerUI[] LobbyPlayersUI;
    LobbyPlayerUI ThisLobbyPlayerUI;

    private Text Name;
    public Dropdown playerDropdown;
    public Toggle ReadyToggle;
    public LobbyScreenManager Lobby;
    public bool LocalPlayer = false;
    string PlayerName;

    // Use this for initialization
    void Start () {
        Lobby = FindObjectOfType<LobbyScreenManager>();
        PlayerID = Lobby.PlayerJoin(this.gameObject);

        LobbyPlayersUI = FindObjectsOfType<LobbyPlayerUI>();
        foreach (LobbyPlayerUI LPU in LobbyPlayersUI)
        {
            if (PlayerID == LPU.player) { ThisLobbyPlayerUI = LPU; }
        }
        ThisLobbyPlayerUI.ChildObject.SetActive(true);

        Name = ThisLobbyPlayerUI.GetComponentInChildren<NameText>().GetComponent<Text>();
        playerDropdown = ThisLobbyPlayerUI.GetComponentInChildren<Dropdown>();
        ReadyToggle = ThisLobbyPlayerUI.GetComponentInChildren<Toggle>();
        ChangeName();

        if (!isLocalPlayer)
        {
            playerDropdown.interactable = false;
            ReadyToggle.interactable = false;
        }
        else
        {
            LocalPlayer = true;
        }
    }
    
    public void SetName(string name)
    {
        PlayerName = name;
    }

    public void ChangeName()
    {
        Debug.Log("Name Change");
        CmdNameChange(PlayerName);
    }

    [Command]
    void CmdNameChange(string name)
    {
        RpcNameChange(name);
    }

    [ClientRpc]
    void RpcNameChange(string name)
    {
        if (Name != null)
        {
            Name.text = name;
        }
    }

    public void ToggledReady()
    {
        Debug.Log("Toggled Ready");
        bool Value = ReadyToggle.isOn;
        CmdToggleChanged(Value);
    }

    [Command]
    void CmdToggleChanged(bool Value)
    {
        RpcToggleChanged(Value);
    }

    [ClientRpc]
    void RpcToggleChanged(bool Value)
    {
        ReadyToggle.isOn = Value;
        Ready = Value;
        GetComponent<NetworkLobbyPlayer>().readyToBegin = Value;
        Lobby.CheckifAllReady();
    }

    public void ValueChanged()
    {
        Debug.Log("Value of Dropdown Changed");
        int DropdownValue = playerDropdown.value;
        CmdDropDownChanged(DropdownValue);
    }

    [Command]
    void CmdDropDownChanged(int DropDownValue)
    {
        RpcDropDownChanged(DropDownValue);
    }

    [ClientRpc]
    void RpcDropDownChanged(int DropDownValue)
    {
        playerDropdown.value = DropDownValue;
        if (playerDropdown.value == 0)
        {
            PT = PlayerType.Holmes;
        }
        else if (playerDropdown.value == 1)
        {
            PT = PlayerType.Moriarty;
        }
        else if (playerDropdown.value == 2)
        {
            PT = PlayerType.Random;
        }
        ReadyToggle.isOn = false;
        GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
    }
	
	// Update is called once per frame
	void Update () {
   
    }
}
