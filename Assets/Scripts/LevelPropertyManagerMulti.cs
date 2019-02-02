using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LevelPropertyManagerMulti : Photon.PunBehaviour, IPunObservable
{


    public PlayerType Player1Player;
    public PlayerType Player2Player;

    public PlayerType PlayerWon;

    public int TotalNumberofRounds;
    public int NumberofHolmesCaseWon;
    public int NumberofMoriartyCaseWon;

    public TileType[,] Tile2DSaved;

    public bool LostInThoughtEnabled;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectOfType<LevelPropertyManager>())
        {
            Destroy(FindObjectOfType<LevelPropertyManager>().gameObject);
        }
        LevelPropertyManagerMulti[] LevelMultiManagers = FindObjectsOfType<LevelPropertyManagerMulti>();
        if (LevelMultiManagers.Length > 1) { Destroy(LevelMultiManagers[0].gameObject); }
    }

    private void OnLevelWasLoaded(int level)
    {
        //Debug.Log(level);
        //if (level == 3)
        //{

        //}
    }

    public PlayerType GetPlayerType(int PlayerID)
    {
        if (PlayerID == 1) { return Player1Player; }
        else if (PlayerID == 2) { return Player2Player; }

        Debug.LogError("Player ID not 1 or 2");
        return Player1Player;
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
        else if (Player1Type == PlayerType.Holmes && Player2Type == PlayerType.Moriarty)
        {
            P1Holmes_P2Moriarty();
        }
        else if (Player1Type == PlayerType.Moriarty && Player2Type == PlayerType.Holmes)
        {
            P1Moriarty_P2Holmes();
        }
    }

    void P1Holmes_P2Moriarty()
    {
        Debug.Log("P1 Holmes P2 Moriarty");
        Player1Player = PlayerType.Holmes;
        Player2Player = PlayerType.Moriarty;
        photonView.RPC("RpcSetPlayerTypes", PhotonTargets.AllViaServer, Player1Player, Player2Player);
    }

    void P1Moriarty_P2Holmes()
    {
        Debug.Log("P1 Moriarty P2 Holmes");
        Player1Player = PlayerType.Moriarty;
        Player2Player = PlayerType.Holmes;
        photonView.RPC("RpcSetPlayerTypes", PhotonTargets.AllViaServer, Player1Player, Player2Player);
    }

    [PunRPC]
    void RpcSetPlayerTypes(PlayerType P1, PlayerType P2)
    {
        Debug.Log("Players set");
        Player1Player = P1;
        Player2Player = P2;
        if (FindObjectOfType<LobbyScreenManager>() != null)
        {
            FindObjectOfType<LobbyScreenManager>().CheckifAllReady();
        }
    }

    public void SaveTileArea(TileType[,] Tile2D)
    {
        Tile2DSaved = Tile2D;
    }

    public void SetPlayerWon(PlayerType PT)
    {
        PlayerWon = PT;
    }

    public void SetDetails(int TotRounds, int HolmsWins, int MoriartyWins)
    {
        TotalNumberofRounds = TotRounds;
        NumberofHolmesCaseWon = HolmsWins;
        NumberofMoriartyCaseWon = MoriartyWins;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Player1Player);
            stream.SendNext(Player2Player);
        }
        else
        {
            this.Player1Player = (PlayerType)stream.ReceiveNext();
            this.Player2Player = (PlayerType)stream.ReceiveNext();
        }
    }

}
