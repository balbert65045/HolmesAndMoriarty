using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {


    public GameObject HolmesTile;
    public GameObject MoriartTile;

    TileArea tileArea;
    TileSelectionPrompt tilePromptSelection;


    public enum TurnStatus { Turn1, Turn2, Turn3, PickTileMoriarty, PickTileHolmes, CheckScore }
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

    bool[] HolmesScoreThisTurn = { false, false, false };
    bool[] MoriartyScoreThisTurn = { false, false, false };
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
        for (int i = 0; i < 3; i++)
        {
            HolmesScoreThisTurn[i] = false;
            MoriartyScoreThisTurn[i] = false;
        }
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
               if (CheckForPickTileMoriarty()) {
                    if (playerController.MyPlayerType == PlayerType.Holmes) { endTurnButton.DisableEndTurn(); }
                    CurrentTurnStatus = TurnStatus.PickTileMoriarty;
                }
               else if (CheckForPickTileHolmes())
                {
                    if (playerController.MyPlayerType == PlayerType.Holmes) { endTurnButton.DisableEndTurn(); }
                    CurrentTurnStatus = TurnStatus.PickTileHolmes;
                }
               else { CurrentTurnStatus = TurnStatus.CheckScore; }

                break;

            case TurnStatus.PickTileMoriarty:
                tilePromptSelection.gameObject.SetActive(false);
                if (CheckForPickTileHolmes())
                {
                    if (playerController.MyPlayerType == PlayerType.Holmes) { endTurnButton.DisableEndTurn(); }
                    CurrentTurnStatus = TurnStatus.PickTileHolmes;
                }
                else
                {
                    CurrentTurnStatus = TurnStatus.CheckScore;
                }  
                break;
            case TurnStatus.PickTileHolmes:
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

    bool CheckForPickTileHolmes()
    {
        List<int> HolmesCaseWon = new List<int>(); 
        for (int i = 0; i < 3; i++)
        {
            if (HolmesScoreThisTurn[i])
            {
                if (caseArea.FindCaseCard(i + 1).PlayerType == PlayerType.Holmes)
                {
                    HolmesCaseWon.Add(i);
                    switch (playerController.MyPlayerType)
                    {
                        case PlayerType.Holmes:
                            playerController.PlaceHolmesTiles(HolmesTile, caseArea.FindCaseCard(i + 1));
                            break;
                        case PlayerType.Moriarty:
                            aiController.PlaceHolmesTile(caseArea.FindCaseCard(i + 1).CardTypes, HolmesTile);
                            break;
                    }
                }
            }
        }
        if (HolmesCaseWon.Count > 0) {
            if (playerController.MyPlayerType == PlayerType.Holmes)
            {
                tilePromptSelection.gameObject.SetActive(true);
                tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + HolmesCaseWon.Count + " Tiles for Holmes that meet the Case";
            }
            return true;
        }
        return false;
    }



    // if Moriarty won 2 Holmes has to pick one tile to give Moriarty
    // if Moriarty one all 3 then Holmes has to pick 2 to give Moriarty
    bool CheckForPickTileMoriarty()
    {


        int MoriartyScore = 0;
        for (int i = 0; i <3; i++)
        {
            if (MoriartyScoreThisTurn[i]) { MoriartyScore++; }
        }
        if (MoriartyScore == 2)
        {
            
            CurrentTurnStatus = TurnStatus.PickTileMoriarty;
            switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:

                    tilePromptSelection.gameObject.SetActive(true);
                    tilePromptSelection.gameObject.GetComponent<Text>().text = " Select 1 Open Tile for Moriarty";
                    playerController.PlaceMoriartyTiles(1, MoriartTile);
                    break;
                case PlayerType.Moriarty:
                    aiController.PlaceMoriartyTile(MoriartTile);
                    break;
            }
            return true;
        }
        else if (MoriartyScore == 3)
        {
            switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    tilePromptSelection.gameObject.SetActive(true);
                    tilePromptSelection.gameObject.GetComponent<Text>().text = " Select 2 Open Tiles for Moriarty";
                    playerController.PlaceMoriartyTiles(2, MoriartTile);
                    break;
                case PlayerType.Moriarty:
                    //Pick Tile 2 times
                    aiController.PlaceMoriartyTile(MoriartTile);
                    aiController.PlaceMoriartyTile(MoriartTile);
                    break;
            }
    //        CurrentTurnStatus = TurnStatus.PickTileMoriarty;
            return true;
        }
        else
        {
      //      CurrentTurnStatus = TurnStatus.CheckScore;
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
        CardType Trump = CheckForTrump(PlayerCrimeCard, AICrimeCard);
        if (CheckForPlayerWin(Trump, PlayerClueCard, AIClueCard))
        {
           switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    // HolmesScoreThisTurn++;
                    HolmesScoreThisTurn[Case - 1] = true;
                    tileArea.PlaceTile(HolmesTile, PlayerClueCard.Number, PlayerType.Holmes);
                    //CaseCard caseCard = caseArea.FindCaseCard(Case);
                    //if (caseCard.PlayerType == PlayerType.Holmes)
                    //{
                    //    Debug.Log("Holmes won on his case card");
                    //}
                    break;
                case PlayerType.Moriarty:
                    MoriartyScoreThisTurn[Case - 1] = true;
                    tileArea.PlaceTile(MoriartTile, PlayerCrimeCard.Number, PlayerType.Moriarty);
                    break;
            }
        }
        else
        {
            switch (aiController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    HolmesScoreThisTurn[Case - 1] = true;
                    tileArea.PlaceTile(HolmesTile, AIClueCard.Number, PlayerType.Holmes);
                    break;
                case PlayerType.Moriarty:
                    MoriartyScoreThisTurn[Case - 1] = true;
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


    // put these in a score controller class??
   CardType CheckForTrump(ClueCard PlayerCrimeCard, ClueCard AICrimeCard)
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
            return CardType.Red;
        }
    }

    // Check to see who has the highest card with trump in play 
    // first check who has trump and then if either both do or do not check for highest card
    bool CheckForPlayerWin(CardType Trump, ClueCard PlayerClueCard, ClueCard AIClueCard)
    {
        // check if both have trump
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
        // check if both only player has trump
        else if (PlayerClueCard.ThisCardType == Trump)
        {
            return true;
        }
        // check if both only AI has trump
        else if (AIClueCard.ThisCardType == Trump)
        {
            return false;
        }

        // check if neither has trump
        else
        {
            if (PlayerClueCard.Number > AIClueCard.Number)
            {
                // check for wrap around effect 
                switch (AIClueCard.Number)
                {
                    case 1:
                        if (PlayerClueCard.Number > 13) { return false; }
                        break;
                    case 2:
                        if (PlayerClueCard.Number > 14) { return false; }
                        break;
                    case 3:
                        if (PlayerClueCard.Number > 15) { return false; }
                        break;
                }
                return true;
            }
            else
            {
                switch (PlayerClueCard.Number)
                {
                    case 1:
                        if (AIClueCard.Number > 13) { return true; }
                        break;
                    case 2:
                        if (AIClueCard.Number > 14) { return true; }
                        break;
                    case 3:
                        if (AIClueCard.Number > 15) { return true; }
                        break;
                }
                return false;
            }
        }
    }

}
