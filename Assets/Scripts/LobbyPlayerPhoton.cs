using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPlayerPhoton : Photon.PunBehaviour, IPunObservable {

    
    public PlayerType PT = PlayerType.Holmes;
    public bool Ready = false;

    public int PlayerID = 0;

    LobbyPlayerUI[] LobbyPlayersUI;
    LobbyPlayerUI ThisLobbyPlayerUI;

    private Text Name;
    public Dropdown playerDropdown;
    public Button ReadyToggle;
    public LobbyScreenManager Lobby;
    public bool LocalPlayer = false;
    string PlayerName;

    IEnumerator ToggleReady;

    // Use this for initialization
     void Start () {
        DontDestroyOnLoad(this.gameObject);
        Lobby = FindObjectOfType<LobbyScreenManager>();

        if (photonView.ownerId > 2){ PlayerID = 2; }
        else{ PlayerID = photonView.ownerId; }

        LobbyPlayersUI = FindObjectsOfType<LobbyPlayerUI>();
        foreach (LobbyPlayerUI LPU in LobbyPlayersUI)
        {
            if (PlayerID == LPU.player) { ThisLobbyPlayerUI = LPU; }
        }
        ThisLobbyPlayerUI.ChildObject.SetActive(true);

        Name = ThisLobbyPlayerUI.GetComponentInChildren<NameText>().GetComponent<Text>();
        playerDropdown = ThisLobbyPlayerUI.GetComponentInChildren<Dropdown>();
        ReadyToggle = ThisLobbyPlayerUI.GetComponentInChildren<Button>();

        PlayerName = "Player " + PlayerID;
        Name.text = PlayerName;
        ChangeName();

        if (!photonView.isMine)
        {
            playerDropdown.interactable = false;
            ReadyToggle.interactable = false;
        }
        else
        {
            playerDropdown.interactable = true;
            ReadyToggle.interactable = true;
            LocalPlayer = true;
        }
    }

    //public override void OnClientExitLobby()
    //{
    //    Debug.Log("Client Exited Lobby");
    // //   RemovePlayer();
    //    base.OnClientExitLobby();
    //}

    //public void Disconnect()
    //{
    //    CmdDisconnect();
    //}

    //[Command]
    //void CmdDisconnect()
    //{
    //    RpcDisconnect();
    //}

    //[ClientRpc]
    //void RpcDisconnect()
    //{
    //    myNetworkManager networkManager = FindObjectOfType<myNetworkManager>();
    //    networkManager.MyStopClient();
    //}

    //public void DisablePlayerUI()
    //{
    //    Debug.Log("Disabling UI");
    //    ThisLobbyPlayerUI.ChildObject.SetActive(false);
    //    Lobby.PlayerLeft();
    //}

    public void SetName(string name)
    {
        PlayerName = name;
    }

    public void ChangeName()
    {
        Debug.Log("Name Change");
        photonView.RPC("RpcNameChange", PhotonTargets.AllViaServer, PlayerName);
    }


    [PunRPC]
    void RpcNameChange(string name)
    {
        Debug.Log("RPC setting name " + name);
        if (Name != null)
        {
            Name.text = name;
        }
    }

    public void ToggledReady(bool value)
    {
        Debug.Log("Toggle changining on player");
        if (photonView.isMine)
        {
            photonView.RPC("RpcToggleChanged", PhotonTargets.AllViaServer, value);
            photonView.RPC("SetPlayers", PhotonTargets.Others);
        }
    }

    [PunRPC]
    void RpcToggleChanged(bool Value)
    {
        Debug.Log("Toggle Changed");
        Ready = Value;
        ReadyToggle.GetComponent<ReadyButton>().SetValue(Ready);
    }

    [PunRPC]
    void SetPlayers()
    {
        Lobby.SetPlayers();
    }

    //[PunRPC]
    //void CheckIfReady()
    //{
    //    Debug.Log("Checking if ready");
    //    Lobby.CheckifAllReady();
    //}


    public void ValueChanged()
    {
        Debug.Log("Value of Dropdown Changed");
        int DropdownValue = playerDropdown.value;
        photonView.RPC("RpcDropDownChanged", PhotonTargets.AllViaServer, DropdownValue);
    }



    [PunRPC]
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
        ReadyToggle.GetComponent<ReadyButton>().SetValue(false);
    }

    // Update is called once per frame
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(ToggledReady);
        }
    }
}
