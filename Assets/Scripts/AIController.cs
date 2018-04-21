using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Player {

    public ClueDeck cardDeck;

    gameManager gameManager;
    AICardArea CrimeArea;
    AICardArea ClueArea;
    CardHand cardHand;
    int[] cardsPlayedPerCase = {0, 0, 0};
    bool SwapCards = false;
    TileArea tileArea;

    public override void SetupPlayer()
    {
        base.SetupPlayer();
        if (FindObjectOfType<LevelPropertyManager>() != null)
        {

            switch (FindObjectOfType<LevelPropertyManager>().GetPlayerType())
            {
                case PlayerType.Holmes:
                    MyPlayerType = PlayerType.Moriarty;
                    break;
                case PlayerType.Moriarty:
                    MyPlayerType = PlayerType.Holmes;
                    break;
            }

        }
    }

    public override void ResetPlayer()
    {
       
        for (int i = 0; i < cardsPlayedPerCase.Length; i++)
        {
            cardsPlayedPerCase[i] = 0;
        }
        RemoveAllCards();
    }

    public override void EnableSwapClueCards()
    {
        SwapCards = true;
    }

    public override bool PlaceHolmesTiles(GameObject HolmesTile, CaseCard HolmesCaseCard)
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

    public override bool PlaceMoriartyTiles(GameObject MoriartyTile, int number)
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
        cardHand = GetComponentInChildren<CardHand>();


        StartCoroutine("CheckToPlayCard");
       
    }

    //Basic Random guess. Needs modify to play smart
    bool PlayCards(int CardPlaced)
    {
        if (cardHand.GetCardsHolding().Count == 0) { return false; }
        //Clue Card Place Down
        if (CardPlaced == 0)
        {
            int RandomIndex = Random.Range(0, cardHand.GetCardsHolding().Count);
            ClueCard ClueCard = cardHand.GetCardsHolding()[RandomIndex];
            cardHand.RemoveCard(cardHand.GetCardsHolding()[RandomIndex]);
            ClueArea.PlaceCard(ClueCard, gameManager.CurrentCaseOn);
            return true;
        }
        else if (CardPlaced == 1)
        {
            int RandomIndex2 = Random.Range(0, cardHand.GetCardsHolding().Count);
            ClueCard CrimeCard = cardHand.GetCardsHolding()[RandomIndex2];
            cardHand.RemoveCard(cardHand.GetCardsHolding()[RandomIndex2]);
            CrimeArea.PlaceCard(CrimeCard, gameManager.CurrentCaseOn);
            return true;
        }
        return false;
        // Crime Card Place Down
    }

    void AIEndTurn()
    {
        gameManager.PlayerEndTurn(MyPlayerType);
    }

    // Need to wait for reset 
    IEnumerator CheckToPlayCard()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return new WaitForSeconds(2f);
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
                    LookToPickMoriartyTile();
                    AIEndTurn();
                    break;
                case gameManager.TurnStatus.PickTileHolmes:
                    LookToPickHolmesTile();
                    AIEndTurn();
                    break;
                case gameManager.TurnStatus.BoardInspect:
                    AIEndTurn();
                    break;
            }

        }
    }

    void LookToPlayCard()
    {
        if (cardsPlayedPerCase[gameManager.CurrentCaseOn - 1] < 2)
        {
            if (PlayCards(cardsPlayedPerCase[gameManager.CurrentCaseOn - 1]))
            {
                cardsPlayedPerCase[gameManager.CurrentCaseOn - 1]++;
                if (cardsPlayedPerCase[gameManager.CurrentCaseOn - 1] == 2) { AIEndTurn(); }
            }
        }
    }

    void LookToSwapClueCards()
    {
        if (SwapCards) {
            AIEndTurn();
            SwapCards = false;
        }
    }

    void LookToPickMoriartyTile()
    {

    }

    void LookToPickHolmesTile()
    {

    }

}
