using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreenManager : MonoBehaviour {

    public GameObject Player1Menu;
    public GameObject Player2Menu;
    public int PlayerIndex = 1;

    private myNetworkManager MyNetworkManager;
    private LevelPropertyManagerMulti LevelManager;

    public Text MatchName;

    // Use this for initialization
    void Start () {
        MyNetworkManager = FindObjectOfType<myNetworkManager>();
        LevelManager = FindObjectOfType<LevelPropertyManagerMulti>();
        if (LevelManager == null) { Debug.LogError("No LevelPropertyManagerMulti in the scene"); }
    }

    public void SetMatchName(string name)
    {
        MatchName.text = name;
    }

    public void PlayerLeft()
    {
        Player2Menu.GetComponent<LobbyPlayerUI>().ChildObject.SetActive(false);
    }

    public void ResetPlayers()
    {
        Player1Menu.GetComponent<LobbyPlayerUI>().ChildObject.SetActive(false);
        Player2Menu.GetComponent<LobbyPlayerUI>().ChildObject.SetActive(false);
    }


    //public int PlayerJoin(GameObject Player)
    //{
    //    if (PlayerIndex == 1)
    //    {
    //       // Player.transform.SetParent(Player1Menu.GetComponent<RectTransform>());
    //        Player.GetComponent<LobbyPlayerPhoton>().SetName("Player 1");
    //    }
    //    else if (PlayerIndex == 2)
    //    {
    //       // Player.transform.SetParent(Player2Menu.GetComponent<RectTransform>());
    //        Player.GetComponent<LobbyPlayerPhoton>().SetName("Player 2");
    //    }
    //    else
    //    {
    //        Debug.LogError("Player Index has mooved to high from players joining only 2 should be able to join");
    //    }
    //   // Player.transform.localPosition = Vector3.zero;
    // //   Player.transform.localScale = new Vector3(1,1,1);
    //    PlayerIndex++;
    //    return (PlayerIndex - 1);
    //}

    //public void PlayerLeft()
    //{
    //    PlayerIndex--;
    //}

    //public void ResetPlayers()
    //{
    //    PlayerIndex = 1;
    //}

    public void CheckifAllReady()
    {
        Debug.Log("Checking if ready on server side");
        LobbyPlayerPhoton[] LobbyPlayers = FindObjectsOfType<LobbyPlayerPhoton>();
        LobbyPlayerPhoton LobbyPlayer1 = null;
        LobbyPlayerPhoton LobbyPlayer2 = null;
        LevelManager = FindObjectOfType<LevelPropertyManagerMulti>();
        foreach (LobbyPlayerPhoton LP in LobbyPlayers)
        {
            if (LP.PlayerID == 1) { LobbyPlayer1 = LP; }
            else if (LP.PlayerID == 2) { LobbyPlayer2 = LP; }
        }
        if (LobbyPlayer1 != null && LobbyPlayer2 != null)
        {
            //LevelManager.DecidePlayersTypes(LobbyPlayer1.PT, LobbyPlayer2.PT);
            if (LobbyPlayer1.Ready && LobbyPlayer2.Ready)
            {
                FindObjectOfType<PhotonLauncher>().MoveToGameScene();
            }
        }
    }

    public void SetPlayers()
    {
        LobbyPlayerPhoton[] LobbyPlayers = FindObjectsOfType<LobbyPlayerPhoton>();
        LobbyPlayerPhoton LobbyPlayer1 = null;
        LobbyPlayerPhoton LobbyPlayer2 = null;
        LevelManager = FindObjectOfType<LevelPropertyManagerMulti>();
        foreach (LobbyPlayerPhoton LP in LobbyPlayers)
        {
            if (LP.PlayerID == 1) { LobbyPlayer1 = LP; }
            else if (LP.PlayerID == 2) { LobbyPlayer2 = LP; }
        }
        Debug.Log("Attempting to set up players");
        if (LobbyPlayer1 != null && LobbyPlayer2 != null)
        {
            LevelManager.DecidePlayersTypes(LobbyPlayer1.PT, LobbyPlayer2.PT);
        }
    }


        // Update is called once per frame
        void Update () {
		
	}
}
