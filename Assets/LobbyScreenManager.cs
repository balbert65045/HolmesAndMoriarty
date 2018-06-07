using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreenManager : MonoBehaviour {

    public GameObject Player1Menu;
    public GameObject Player2Menu;
    public int PlayerIndex = 1;

    private myNetworkManager MyNetworkManager;
	// Use this for initialization
	void Start () {
        MyNetworkManager = FindObjectOfType<myNetworkManager>();
    }

    public void PlayerJoin(GameObject Player)
    {
        if (PlayerIndex == 1)
        {
            Player.transform.SetParent(Player1Menu.GetComponent<RectTransform>());
        }
        else if (PlayerIndex == 2)
        {
            Player.transform.SetParent(Player2Menu.GetComponent<RectTransform>());
        }
        else
        {
            Debug.LogError("Player Index has mooved to high from players joining only 2 should be able to join");
        }
        Player.transform.localPosition = Vector3.zero;
        PlayerIndex++;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
