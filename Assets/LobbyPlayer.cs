using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour {

    public Text Name;
    public PlayerType PT = PlayerType.Holmes;
    public bool Ready = false;

    private Dropdown playerDropdown;
    private Toggle ReadyToggle;

	// Use this for initialization
	void Start () {
        playerDropdown = GetComponentInChildren<Dropdown>();
        ReadyToggle = GetComponentInChildren<Toggle>();
        LobbyScreenManager Lobby = FindObjectOfType<LobbyScreenManager>();
        Lobby.PlayerJoin(this.gameObject);

        if (!isLocalPlayer)
        {
            playerDropdown.interactable = false;
            ReadyToggle.interactable = false;
        }
    }

    public void ChangeName(string name)
    {
        Debug.Log("Name Change");
        CmdNameChange(name);
    }

    [Command]
    void CmdNameChange(string name)
    {
        RpcNameChange(name);
    }

    [ClientRpc]
    void RpcNameChange(string name)
    {
        Name.text = name;
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
    }
	
	// Update is called once per frame
	void Update () {
   
    }
}
