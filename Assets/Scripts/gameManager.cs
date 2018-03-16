﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {


    public GameObject HolmesTile;
    public GameObject MoriartTile;

    TileArea tileArea;
    TileSelectionPrompt tilePromptSelection;


    public enum TurnStatus { Turn1, Turn2, Turn3, PickTile, CheckScore }
    public TurnStatus CurrentTurnStatus = TurnStatus.Turn1;

    public int CurrentTurnOn = 1;
    public int CurrentCaseOn = 1;

    CardArea CrimeArea;
    CardArea ClueArea;
    AICardArea ACrimeArea;
    AICardArea AClueArea;

    CaseArea caseArea;

    EndTurnButton endTurnButton;
    PlayerController playerController;
    AIController aiController;
    public ClueDeck cardDeck;

    public List<ClueCard> PlayersCards = new List<ClueCard>();
    public List<ClueCard> AICards = new List<ClueCard>();

    int HolmesScoreThisTurn = 0;
    int MoriartyScoreThisTurn = 0;
    // Use this for initialization
    void Start () {

        tilePromptSelection = FindObjectOfType<TileSelectionPrompt>();
        tilePromptSelection.gameObject.SetActive(false);
        endTurnButton = FindObjectOfType<EndTurnButton>();
        tileArea = FindObjectOfType<TileArea>();
        caseArea = FindObjectOfType<CaseArea>();
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea carda in CardAreas)
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
        AICardArea[] ACardAreas = FindObjectsOfType<AICardArea>();
        foreach (AICardArea carda in ACardAreas)
        {
            if (carda.ThisRow == CardArea.Row.Clue)
            {
                AClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime)
            {
                ACrimeArea = carda;
            }
        }

        playerController = FindObjectOfType<PlayerController>();
        aiController = FindObjectOfType<AIController>();

        StartCoroutine("PlayersDrawCards");
    }
	
    IEnumerator PlayersDrawCards()
    {
        yield return new WaitForSeconds(1.5f);
        playerController.DrawCards(7);
        aiController.DrawCards(7);
        caseArea.PlaceCards();
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        playerController.RemoveAllCards();
        aiController.ResetCards();
        ClueArea.ClearCards();
        CrimeArea.ClearCards();
        AClueArea.ClearCards();
        ACrimeArea.ClearCards();
        cardDeck.ResetCards();
        caseArea.ClearCards();
        HolmesScoreThisTurn = 0;
        MoriartyScoreThisTurn = 0;
    }

    public void CheckEndTurn()
    {
        if (!ClueArea.CheckForAvailableSpace(CurrentCaseOn) && !CrimeArea.CheckForAvailableSpace(CurrentCaseOn))
        {
            endTurnButton.EnableEndTurn();
        }
    }

    public void EndTurn()
    {
        switch (CurrentTurnStatus)
        {
            case TurnStatus.Turn1:
                endTurnButton.DisableEndTurn();
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                CurrentTurnStatus = TurnStatus.Turn2;
                break;

            case TurnStatus.Turn2:
                endTurnButton.DisableEndTurn();
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                CurrentTurnStatus = TurnStatus.Turn3;
                break;

            case TurnStatus.Turn3:
                for (int i = 1; i <= 3; i++)
                {
                    CheckForScore(i);
                }
               if (CheckForPickTile()) {
                    endTurnButton.DisableEndTurn();
                    CurrentTurnStatus = TurnStatus.PickTile;
                }
               else { CurrentTurnStatus = TurnStatus.CheckScore; }

                break;

            case TurnStatus.PickTile:
                tilePromptSelection.gameObject.SetActive(false);
                CurrentTurnStatus = TurnStatus.CheckScore;
                break;
            case TurnStatus.CheckScore:
                CurrentCaseOn++;
                CheckForWin();
                endTurnButton.DisableEndTurn();
                StartCoroutine("Reset");
                StartCoroutine("PlayersDrawCards");
                CurrentCaseOn = 1;
                CurrentTurnStatus = TurnStatus.Turn1;
                break;

        }
    }

    // if Moriarty won 2 Holmes has to pick one tile to give Moriarty
    // if Moriarty one all 3 then Holmes has to pick 2 to give Moriarty
    bool CheckForPickTile()
    {
        if (MoriartyScoreThisTurn == 2)
        {
            
            CurrentTurnStatus = TurnStatus.PickTile;
            switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:

                    tilePromptSelection.gameObject.SetActive(true);
                    tilePromptSelection.gameObject.GetComponent<Text>().text = " Select 1 Open Tile for Moriarty";
                    playerController.PlaceMoriartyTiles(1);
                    break;
                case PlayerType.Moriarty:

                    break;
            }
            return true;
        }
        else if (MoriartyScoreThisTurn == 3)
        {
            switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    tilePromptSelection.gameObject.SetActive(true);
                    tilePromptSelection.gameObject.GetComponent<Text>().text = " Select 2 Open Tiles for Moriarty";
                    playerController.PlaceMoriartyTiles(2);
                    break;
                case PlayerType.Moriarty:

                    break;
            }
            CurrentTurnStatus = TurnStatus.PickTile;
            return true;
        }
        else
        {
            CurrentTurnStatus = TurnStatus.CheckScore;
            return false;
        }
    }


    void CheckForWin()
    {
        if (tileArea.CheckForMoriartyWin())
        {
            Debug.Log("Moriarty Wins");
        }

        if (CurrentTurnOn == 5)
        {
            Debug.Log("Holmes Wins");
        }
    }


    IEnumerator SwapCards()
    {
        PlayersCards.Clear();
        AICards.Clear();
        foreach (ClueCard card in playerController.GetCardsHolding()){
            PlayersCards.Add(card); }
        foreach (ClueCard card in aiController.CardsHolding) { AICards.Add(card); }


        aiController.RemoveAllCards();
        playerController.RemoveAllCards();

        yield return new WaitForSeconds(.1f);

        aiController.AddNewCards(PlayersCards);
        playerController.AddNewCards(AICards);
    }

    void CheckForScore(int Case)
    {

        ClueCard PlayerCrimeCard;
        ClueCard PlayerClueCard;
        ClueCard AICrimeCard;
        ClueCard AIClueCard;

        FlipCards(Case, out PlayerCrimeCard, out PlayerClueCard, out AICrimeCard, out AIClueCard);
        ClueCard.CardType Trump = CheckForTrump(PlayerCrimeCard, AICrimeCard);
        if (CheckForPlayerWin(Trump, PlayerClueCard, AIClueCard))
        {
           switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    HolmesScoreThisTurn++;
                    tileArea.PlaceTile(HolmesTile, PlayerClueCard.Number, PlayerType.Holmes);
                    break;
                case PlayerType.Moriarty:
                    MoriartyScoreThisTurn++;
                    tileArea.PlaceTile(MoriartTile, PlayerCrimeCard.Number, PlayerType.Moriarty);
                    break;
            }
        }
        else
        {
            switch (aiController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    HolmesScoreThisTurn++;
                    tileArea.PlaceTile(HolmesTile, AIClueCard.Number, PlayerType.Holmes);
                    break;
                case PlayerType.Moriarty:
                    MoriartyScoreThisTurn++;
                    tileArea.PlaceTile(MoriartTile, AICrimeCard.Number, PlayerType.Moriarty);
                    break;
            }
        }
       


    }

    void FlipCards(int Case, out ClueCard PlayerCrimeCard, out ClueCard PlayerClueCard, out ClueCard AICrimeCard, out ClueCard AIClueCard)
    {
        PlayerCrimeCard = CrimeArea.FlipCard(Case);
        PlayerClueCard = ClueArea.FlipCard(Case);
        AICrimeCard = ACrimeArea.FlipCard(Case);
        AIClueCard = AClueArea.FlipCard(Case);
        caseArea.FlipCard(Case);
    }

    ClueCard.CardType CheckForTrump(ClueCard PlayerCrimeCard, ClueCard AICrimeCard)
    {
        if (PlayerCrimeCard.Number > AICrimeCard.Number)
        {
            return PlayerCrimeCard.ThisCardType;
        }
        else if (AICrimeCard.Number > PlayerCrimeCard.Number)
        {
            return AICrimeCard.ThisCardType;
        }
        else
        {
            Debug.LogError("Player Card and AI card should never be the same");
            return ClueCard.CardType.Red;
        }
    }

    // Check to see who has the highest card with trump in play 
    // first check who has trump and then if either both do or do not check for highest card
    bool CheckForPlayerWin(ClueCard.CardType Trump, ClueCard PlayerClueCard, ClueCard AIClueCard)
    {
        if (PlayerClueCard.ThisCardType == Trump && AIClueCard.ThisCardType == Trump)
        {
            if (PlayerClueCard.Number > AIClueCard.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (PlayerClueCard.ThisCardType == Trump)
        {
            return true;
        }
        else if (AIClueCard.ThisCardType == Trump)
        {
            return false;
        }
        else
        {
            if (PlayerClueCard.Number > AIClueCard.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
