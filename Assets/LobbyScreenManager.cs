using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreenManager : MonoBehaviour {

    public GameObject Player1Menu;
    public GameObject Player2Menu;
    public int PlayerIndex = 1;

    private myNetworkManager MyNetworkManager;
    private LevelPropertyManagerMulti LevelManager;
    // Use this for initialization
    void Start () {
        MyNetworkManager = FindObjectOfType<myNetworkManager>();
        LevelManager = FindObjectOfType<LevelPropertyManagerMulti>();
        if (LevelManager == null) { Debug.LogError("No LevelPropertyManagerMulti in the scene"); }
    }

    public int PlayerJoin(GameObject Player)
    {
        if (PlayerIndex == 1)
        {
            Player.transform.SetParent(Player1Menu.GetComponent<RectTransform>());
            Player.GetComponent<LobbyPlayer>().ChangeName("Player 1");
        }
        else if (PlayerIndex == 2)
        {
            Player.transform.SetParent(Player2Menu.GetComponent<RectTransform>());
            Player.GetComponent<LobbyPlayer>().ChangeName("Player 2");
        }
        else
        {
            Debug.LogError("Player Index has mooved to high from players joining only 2 should be able to join");
        }
        Player.transform.localPosition = Vector3.zero;
        Player.transform.localScale = new Vector3(1,1,1);
        PlayerIndex++;
        return (PlayerIndex - 1);
    }

    public void CheckifAllReady()
    {
        if (Player1Menu.GetComponentInChildren<LobbyPlayer>() != null && Player2Menu.GetComponentInChildren<LobbyPlayer>() != null)
        {
            if (Player1Menu.GetComponentInChildren<LobbyPlayer>().Ready && Player2Menu.GetComponentInChildren<LobbyPlayer>().Ready)
            {
                LevelManager.DecidePlayersTypes(Player1Menu.GetComponent<LobbyPlayer>().PT, Player2Menu.GetComponent<LobbyPlayer>().PT);
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
