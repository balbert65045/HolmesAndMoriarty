using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LevelPropertyManagerMulti : NetworkBehaviour {


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

    private void OnLevelWasLoaded(int level)
    {
        //Debug.Log(level);
        //if (level == 3)
        //{
          
        //}
    }



    public void DecidePlayersTypes(PlayerType Player1Type, PlayerType Player2Type)
    {
        if (Player1Type == PlayerType.Holmes && Player2Type == PlayerType.Holmes)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) {
                P1Moriarty_P2Holmes();
            }
            else if (PlayerID == 2) {
                P1Holmes_P2Moriarty();
            }
        }
        else if (Player1Type == PlayerType.Moriarty && Player2Type == PlayerType.Moriarty)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) {
                P1Holmes_P2Moriarty();
            }
            else if (PlayerID == 2) {
                P1Moriarty_P2Holmes();
            }
        }
        else if (Player1Type == PlayerType.Random && Player2Type == PlayerType.Random)
        {
            int PlayerID = Random.Range(1, 3);
            if (PlayerID == 1) {
                P1Holmes_P2Moriarty();
            }
            else if (PlayerID == 2) {
                P1Moriarty_P2Holmes();
            }
        }
        else if (Player1Type == PlayerType.Random)
        {
            if (Player2Type == PlayerType.Holmes) {
                P1Moriarty_P2Holmes();
            }
            else if (Player2Type == PlayerType.Moriarty) {
                P1Holmes_P2Moriarty();
            }
        }
        else if (Player2Type == PlayerType.Random)
        {
            if (Player1Type == PlayerType.Holmes) {
                P1Holmes_P2Moriarty();
            }
            else if (Player1Type == PlayerType.Moriarty) {
                P1Moriarty_P2Holmes();
            }
        }
    }

    void P1Holmes_P2Moriarty()
    {
        Player1Player = PlayerType.Holmes;
        Player2Player = PlayerType.Moriarty;
    }

    void P1Moriarty_P2Holmes()
    {
        Player1Player = PlayerType.Moriarty;
        Player2Player = PlayerType.Holmes;
    }

     // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
