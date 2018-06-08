using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropertyManagerMulti : MonoBehaviour {


    public PlayerType Player1Player;
    public PlayerType Player2Player;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectOfType<LevelPropertyManager>())
        {
            Destroy(FindObjectOfType<LevelPropertyManager>());
        }
    }



    public void DecidePlayersTypes(PlayerType Player1Type, PlayerType Player2Type)
    {
        if (Player1Type == PlayerType.Holmes && Player2Type == PlayerType.Holmes)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) { Player1Player = PlayerType.Moriarty; }
            else if (PlayerID == 2) { Player2Player = PlayerType.Moriarty; }
        }
        else if (Player1Type == PlayerType.Moriarty && Player2Type == PlayerType.Moriarty)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) { Player1Player = PlayerType.Holmes; }
            else if (PlayerID == 2) { Player2Player = PlayerType.Holmes; }
        }
        else if (Player1Type == PlayerType.Random && Player2Type == PlayerType.Random)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) {
                Player1Player = PlayerType.Holmes;
                Player2Player = PlayerType.Moriarty;
            }
            else if (PlayerID == 2) {
                Player2Player = PlayerType.Holmes;
                Player1Player = PlayerType.Moriarty;
            }
        }
        else if (Player1Type == PlayerType.Random)
        {
            if (Player2Player == PlayerType.Holmes) { Player1Player = PlayerType.Moriarty; }
            if (Player2Player == PlayerType.Moriarty) { Player1Player = PlayerType.Holmes; }
        }
        else if (Player2Type == PlayerType.Random)
        {
            if (Player1Player == PlayerType.Holmes) { Player2Player = PlayerType.Moriarty; }
            if (Player1Player == PlayerType.Moriarty) { Player2Player = PlayerType.Holmes; }
        }
    }

     // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
