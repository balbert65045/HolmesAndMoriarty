using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

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
        
    }
	
	// Update is called once per frame
	void Update () {
    }
}
