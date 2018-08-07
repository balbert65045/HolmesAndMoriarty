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
           // Player.transform.SetParent(Player1Menu.GetComponent<RectTransform>());
            Player.GetComponent<LobbyPlayer>().SetName("Player 1");
        }
        else if (PlayerIndex == 2)
        {
           // Player.transform.SetParent(Player2Menu.GetComponent<RectTransform>());
            Player.GetComponent<LobbyPlayer>().SetName("Player 2");
        }
        else
        {
            Debug.LogError("Player Index has mooved to high from players joining only 2 should be able to join");
        }
       // Player.transform.localPosition = Vector3.zero;
     //   Player.transform.localScale = new Vector3(1,1,1);
        PlayerIndex++;
        return (PlayerIndex - 1);
    }

    public void PlayerLeft()
    {
        PlayerIndex--;
    }

    public void CheckifAllReady()
    {
        Debug.Log("Checking if ready on server side");
        LobbyPlayer[] LobbyPlayers = FindObjectsOfType<LobbyPlayer>();
        LobbyPlayer LobbyPlayer1 = null;
        LobbyPlayer LobbyPlayer2 = null;
        LevelManager = FindObjectOfType<LevelPropertyManagerMulti>();
        foreach (LobbyPlayer LP in LobbyPlayers)
        {
            if (LP.PlayerID == 1) { LobbyPlayer1 = LP; }
            else if (LP.PlayerID == 2) { LobbyPlayer2 = LP; }
        }
        LevelManager.DecidePlayersTypes(LobbyPlayer1.PT, LobbyPlayer2.PT);

        if (LobbyPlayer1 != null && LobbyPlayer2 != null)
        {
            if (LobbyPlayer1.Ready && LobbyPlayer2.Ready)
            {
                MyNetworkManager.CheckReadyToBegin();
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
