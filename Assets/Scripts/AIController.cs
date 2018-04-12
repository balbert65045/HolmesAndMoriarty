using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    public List<ClueCard> CardsHolding;
    gameManager gameManager;
    public ClueDeck cardDeck;
    AICardArea CrimeArea;
    AICardArea ClueArea;

    //public GameObject MoriartyTile;
    TileArea tileArea;

    public PlayerType MyPlayerType;

    public void DrawCards(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            ClueCard CardDrawn = (cardDeck.DrawCard()) as ClueCard;
            CardsHolding.Add(CardDrawn);
        }
        PlayCards();
    }

    //Basic Random guess. Needs modify to play smart
    void PlayCards()
    {
        //Clue Card Place Down
        int RandomIndex = Random.Range(0, CardsHolding.Count);
        GameObject ClueCard = Instantiate(CardsHolding[RandomIndex].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex]);
        ClueArea.PlaceCard(ClueCard.GetComponent<ClueCard>(), gameManager.CurrentCaseOn);

        // Crime Card Place Down
        int RandomIndex2 = Random.Range(0, CardsHolding.Count);
        GameObject CrimeCard = Instantiate(CardsHolding[RandomIndex2].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex2]);
        CrimeArea.PlaceCard(CrimeCard.GetComponent<ClueCard>(), gameManager.CurrentCaseOn);
        AIEndTurn();

    }
    // Do nothing but simply end turn for the moment 
    public void EnableSwapClueCards()
    {
        Debug.Log("AI swapping clue cards");
        AIEndTurn();
    }

    public void InspectBoard()
    {
        Debug.Log("AI sinspecting board");
        AIEndTurn();
    }

    void AIEndTurn()
    {
        gameManager.PlayerEndTurn(MyPlayerType);
    }

    public void ResetCards()
    {
        RemoveAllCards();
    }

    public void RemoveAllCards()
    {
        CardsHolding.Clear();
    }

    public void AddNewCards(List<ClueCard> Cards)
    {
        foreach (ClueCard card in Cards)
        {
            CardsHolding.Add(card);
        }
        PlayCards();
    }


    public bool PlaceHolmesTile(GameObject HolmesTile, CaseCard HolmesCaseCard)
    {
        TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
        List<TileSpot> OpenTileSpots = new List<TileSpot>();
        foreach (TileSpot TS in TilesSpots)
        {
            if (!TS.Used)
            {
                if (HolmesCaseCard.CardTypes.Contains(TS.ThisCardType)) { OpenTileSpots.Add(TS); }
            }
        }
        if (OpenTileSpots.Count == 0) { return false; }
        int RandomOpenTileIndex = Random.Range(0, OpenTileSpots.Count);
        tileArea.PlaceTile(HolmesTile, OpenTileSpots[RandomOpenTileIndex].Number, PlayerType.Holmes);
        return true;
    }

    public bool PlaceMoriartyTile(GameObject MoriartyTile, int number)
    {
        for (int i = 0; i < number; i++)
        {
            TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
            List<TileSpot> OpenTileSpots = new List<TileSpot>();
            foreach (TileSpot TS in TilesSpots)
            {
                if (!TS.Used) { OpenTileSpots.Add(TS); }
            }
            if (OpenTileSpots.Count == 0) { return false; }
            int RandomOpenTileIndex = Random.Range(0, OpenTileSpots.Count);
            tileArea.PlaceTile(MoriartyTile, OpenTileSpots[RandomOpenTileIndex].Number, PlayerType.Moriarty);
        }
        return true;
    }

    // Use this for initialization
    void Start () {

        if (FindObjectOfType<LevelPropertyManager>() != null)
        {
            PlayerType playerType = FindObjectOfType<LevelPropertyManager>().GetPlayerType();
            switch (playerType)
            {
                case PlayerType.Holmes:
                    MyPlayerType = PlayerType.Moriarty;
                    break;
                case PlayerType.Moriarty:
                    MyPlayerType = PlayerType.Holmes;
                    break;
            }
        }


        tileArea = FindObjectOfType<TileArea>();
         AICardArea[] CardAreas = FindObjectsOfType<AICardArea>();
        foreach (AICardArea carda in CardAreas)
        {
            if (carda.ThisRow == CardArea.Row.Clue)
            {
                ClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime)
            {
                CrimeArea = carda;
            }
        }

        gameManager = FindObjectOfType<gameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator CheckToPlayCard()
    {
        yield return new WaitForSeconds(.2f);
        switch (gameManager.CurrentTurnStatus)
        {
            case gameManager.TurnStatus.Turn1:
                LookToPlayCard();
                break;
            case gameManager.TurnStatus.Turn2:
                LookToPlayCard();
                break;
            case gameManager.TurnStatus.Turn3:
                LookToPlayCard();
                break;
            case gameManager.TurnStatus.SwitchClueCards:
                LookToSwapClueCards();
                break;
            case gameManager.TurnStatus.PickTileMoriarty:
                
                break;
        }


    }

    void LookToPlayCard()
    {

    }

    void LookToSwapClueCards()
    {

    }

}
