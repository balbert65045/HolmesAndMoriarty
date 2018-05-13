﻿using System.Collections;
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

    ClueCard clueCardUsing;
    ClueCard crimeCardUsing;

    public List<ClueCard> cardsOponentHas;
    public ClueCard GuessOponentClueCard;
    public ClueCard GuessOponnentCrimeCard;

    public LevelPropertyManager.Difficulty difficulty;

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
            difficulty = FindObjectOfType<LevelPropertyManager>().DifficultyPicked;
        }
    }

    public override void ResetPlayer()
    {
        base.ResetPlayer();
        for (int i = 0; i < cardsPlayedPerCase.Length; i++)
        {
            cardsPlayedPerCase[i] = 0;
        }
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

            List<TileSpot> NotLoseTileSpots = new List<TileSpot>();
            foreach (TileSpot TS in OpenTileSpots)
            {
                if (!tileArea.CheckForMoriartyWinWithTile(TS.Number)) { NotLoseTileSpots.Add(TS); }
                else { Debug.Log("Would have lost with that tile"); }
            }


            int RandomOpenTileIndex = Random.Range(0, NotLoseTileSpots.Count);
            if (NotLoseTileSpots.Count == 0) { RandomOpenTileIndex = Random.Range(0, OpenTileSpots.Count); }
            else {RandomOpenTileIndex = Random.Range(0, NotLoseTileSpots.Count); }
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

        List<ClueCard> cardsHolding = cardHand.GetCardsHolding();
        switch (difficulty)
        {
            case LevelPropertyManager.Difficulty.Easy:
                //Clue Card Place Down
                if (CardPlaced == 0)
                {
                    int RandomIndex = Random.Range(0, cardHand.GetCardsHolding().Count);
                    ClueCard ClueCard = cardHand.GetCardsHolding()[RandomIndex];
                    cardHand.RemoveCard(cardHand.GetCardsHolding()[RandomIndex]);
                    ClueArea.PlaceCard(ClueCard, gameManager.CurrentCaseOn);
                    return true;
                }
                // Crime Card Place Down
                else if (CardPlaced == 1)
                {
                    int RandomIndex2 = Random.Range(0, cardHand.GetCardsHolding().Count);
                    ClueCard CrimeCard = cardHand.GetCardsHolding()[RandomIndex2];
                    cardHand.RemoveCard(cardHand.GetCardsHolding()[RandomIndex2]);
                    CrimeArea.PlaceCard(CrimeCard, gameManager.CurrentCaseOn);
                    return true;
                }
                return false;
            case LevelPropertyManager.Difficulty.Medium:
               
                if (clueCardUsing == null && crimeCardUsing == null)
                {
                    FindBestCrimeClueCard(cardsHolding, out clueCardUsing, out crimeCardUsing);
                }

                if (CardPlaced == 0)
                {
                    cardHand.RemoveCard(clueCardUsing);
                    ClueArea.PlaceCard(clueCardUsing, gameManager.CurrentCaseOn);
                    return true;
                }
                else if (CardPlaced == 1)
                {
                    cardHand.RemoveCard(crimeCardUsing);
                    CrimeArea.PlaceCard(crimeCardUsing, gameManager.CurrentCaseOn);
                    clueCardUsing = null;
                    crimeCardUsing = null;

                    return true;
                }
                return false;
            case LevelPropertyManager.Difficulty.Hard:
                switch (gameManager.CurrentCaseOn)
                {
                    case 1:
                        if (clueCardUsing == null && crimeCardUsing == null)
                        {
                            FindBestCrimeClueCard(cardsHolding, out clueCardUsing, out crimeCardUsing);
                        }

                        if (CardPlaced == 0)
                        {
                            cardHand.RemoveCard(clueCardUsing);
                            ClueArea.PlaceCard(clueCardUsing, gameManager.CurrentCaseOn);
                            return true;
                        }
                        else if (CardPlaced == 1)
                        {
                            cardHand.RemoveCard(crimeCardUsing);
                            CrimeArea.PlaceCard(crimeCardUsing, gameManager.CurrentCaseOn);
                            clueCardUsing = null;
                            crimeCardUsing = null;
                            List<ClueCard> OldCards =new List<ClueCard>();
                            foreach (ClueCard card in cardHand.GetCardsHolding())
                            {
                                OldCards.Add(card);
                            }
                            cardsOponentHas = OldCards;
                            return true;
                        }
                        return false;
                    case 2:

                        if (GuessOponentClueCard == null && GuessOponnentCrimeCard == null)
                        {
                            FindBestCrimeClueCard(cardsOponentHas, out GuessOponentClueCard, out GuessOponnentCrimeCard);
                        }

                        if (clueCardUsing == null && crimeCardUsing == null)
                        {
                            Debug.Log("Oponent Crime Card suspected is " + GuessOponnentCrimeCard.Number);
                            Debug.Log("Oponent Clue Card suspected is " + GuessOponentClueCard.Number);
                            if (!CheckifPossibletoBeatOponentsBest(GuessOponnentCrimeCard, GuessOponentClueCard, cardsHolding))
                            {
                                FindBestCrimeClueCard(cardsHolding, out clueCardUsing, out crimeCardUsing);
                            }
                        }


                        if (CardPlaced == 0)
                        {
                            cardHand.RemoveCard(clueCardUsing);
                            ClueArea.PlaceCard(clueCardUsing, gameManager.CurrentCaseOn);
                            return true;
                        }
                        else if (CardPlaced == 1)
                        {
                            cardHand.RemoveCard(crimeCardUsing);
                            CrimeArea.PlaceCard(crimeCardUsing, gameManager.CurrentCaseOn);
                            clueCardUsing = null;
                            crimeCardUsing = null;
                            GuessOponentClueCard = null;
                            GuessOponnentCrimeCard = null;
                            List<ClueCard> OldCards = new List<ClueCard>();
                            foreach (ClueCard card in cardHand.GetCardsHolding())
                            {
                                OldCards.Add(card);
                            }
                            cardsOponentHas = OldCards;
                            return true;
                        }


                        break;
                    case 3:
                        if (GuessOponentClueCard == null && GuessOponnentCrimeCard == null)
                        {
                            FindBestCrimeClueCard(cardsOponentHas, out GuessOponentClueCard, out GuessOponnentCrimeCard);
                        }

                        if (clueCardUsing == null && crimeCardUsing == null)
                        {
                            Debug.Log("Oponent Crime Card suspected is " + GuessOponnentCrimeCard.Number);
                            Debug.Log("Oponent Clue Card suspected is " + GuessOponentClueCard.Number);
                            if (!CheckifPossibletoBeatOponentsBest(GuessOponnentCrimeCard, GuessOponentClueCard, cardsHolding))
                            {
                                FindBestCrimeClueCard(cardsHolding, out clueCardUsing, out crimeCardUsing);
                            }
                        }


                        if (CardPlaced == 0)
                        {
                            cardHand.RemoveCard(clueCardUsing);
                            ClueArea.PlaceCard(clueCardUsing, gameManager.CurrentCaseOn);
                            return true;
                        }
                        else if (CardPlaced == 1)
                        {
                            cardHand.RemoveCard(crimeCardUsing);
                            CrimeArea.PlaceCard(crimeCardUsing, gameManager.CurrentCaseOn);
                            clueCardUsing = null;
                            crimeCardUsing = null;
                            GuessOponentClueCard = null;
                            GuessOponnentCrimeCard = null;
                            cardsOponentHas = cardHand.GetCardsHolding();
                            return true;
                        }


                        break;
                }

                return false;
        }
        return false;
    }

    bool CheckifPossibletoBeatOponentsBest(ClueCard OponentCrimeCard, ClueCard OponentClueCard, List<ClueCard> CardsAvailable)
    {

        // Look to beat the crime and match 
        foreach (ClueCard CrimeCard in CardsAvailable)
        {
            if ((CrimeCard.Number > OponentCrimeCard.Number || ((OponentCrimeCard.Number > 13) && (CrimeCard.Number < OponentCrimeCard.Number % 4))) && (CrimeCard.ThisCardType != OponentClueCard.ThisCardType))
            {
                foreach (ClueCard ClueCard in CardsAvailable)
                {
                    if (ClueCard != CrimeCard)
                    {
                        if (ClueCard.ThisCardType == CrimeCard.ThisCardType)
                        {
                            Debug.Log("Found a card that will win");
                            crimeCardUsing = CrimeCard;
                            clueCardUsing = ClueCard;
                            return true;
                        }
                    }
                }
            }

        }

       // Crime doesnt match, so try to put a clue card that matches opponent crime 
        if (OponentCrimeCard.ThisCardType != OponentClueCard.ThisCardType)
        {
            foreach (ClueCard clueCard in CardsAvailable)
            {
                if (clueCard.ThisCardType == OponentCrimeCard.ThisCardType)
                {
                    foreach (ClueCard crimeCard in CardsAvailable)
                    {
                        if (clueCard != crimeCard)
                        {
                            if (crimeCard.Number < OponentCrimeCard.Number)
                            {
                                Debug.Log("Found a card that matches oponent crime");
                                crimeCardUsing = crimeCard;
                                clueCardUsing = clueCard;
                                return true;
                            }
                        }
                    }
                }
            }
        }

        // look if possible to put a clue thats higher than theirs 
        foreach (ClueCard clueCard in CardsAvailable)
        {
            if (clueCard.ThisCardType == OponentClueCard.ThisCardType && clueCard.Number > OponentClueCard.Number)
            {
                foreach(ClueCard crimeCard in CardsAvailable)
                {
                    if (clueCard != crimeCard)
                    {
                        if (crimeCard.Number < OponentCrimeCard.Number)
                        {
                            Debug.Log("Found a card clue card that will beat theirs in trump");
                            crimeCardUsing = crimeCard;
                            clueCardUsing = clueCard;
                            return true;
                        }
                    }
                }
            }
        }


        return false;
    }

    bool FindBestCrimeClueCard(List<ClueCard> cards, out ClueCard clueCard, out ClueCard crimeCard)
    {
        QuickSort QSort = new QuickSort();
        List<int> CardNumbers = new List<int>();
        foreach (ClueCard card in cards){ CardNumbers.Add(card.Number);}
        int[] ArrCardNumbers = CardNumbers.ToArray();
        int[] OrderdCardNumbers = QSort.Sort(ArrCardNumbers);


        // Order the cards
        ClueCard[] OrderedCards = new ClueCard[OrderdCardNumbers.Length];
        for (int i = 0; i < OrderdCardNumbers.Length; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j].Number == OrderdCardNumbers[i])
                {
                    OrderedCards[i] = cards[j];
                    break;
                }
            }
        }

        // Find highest card and see if it shares a color 
        for (int i = OrderedCards.Length - 1; i >= 0; i--)
        {
            for (int j = OrderedCards.Length - 1; j >= 0; j--)
            {
                if (OrderedCards[j].ThisCardType == OrderedCards[i].ThisCardType && OrderedCards[j] != OrderedCards[i])
                {

                    clueCard = OrderedCards[j];
                    crimeCard = OrderedCards[i];
                    return true;
                }
            }
        }
        crimeCard = OrderedCards[0];
        clueCard = OrderedCards[OrderedCards.Length - 1];
        return false;
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
