using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class M_gameManager : MonoBehaviour {


    public GameObject HolmesTile;
    public GameObject MoriartTile;

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject PlayerDisconnectScreen;

    public GameObject ShowAreaText;
    public GameObject ShowArea;

    TileArea tileArea;
    TileSelectionPrompt tilePromptSelection;


    public enum TurnStatus { Turn1, Turn2, Turn3, SwitchClueCards, PickTileMoriarty, PickTileHolmes, BoardInspect }
    public TurnStatus CurrentTurnStatus = TurnStatus.Turn1;

    public int CurrentTurnOn = 1;
    public int CurrentCaseOn = 1;

    CardArea MyPlayerCrimeArea;
    CardArea MyPlayerClueArea;
    CardArea MyOpponentCrimeArea;
    CardArea MyOpponentClueArea;

    M_CaseArea caseArea;

    M_Player[] Players;
    public M_Player HolmesPlayer;
    public M_Player MoriartyPlayer;

    public M_ClueDeck cardDeck;

    public List<ClueCard> HolmesCards = new List<ClueCard>();
    public List<ClueCard> MoriartyCards = new List<ClueCard>();

    public bool[] HolmesScoreThisTurn = { false, false, false };
    public bool[] MoriartyScoreThisTurn = { false, false, false };

    int totalHWins = 0;
    int totalMWins = 0;

   public bool HolmesEndTurn = false;
   public bool MoriartyEndTurn = false;

    public GameObject[] SwapButtonsObj;

    TurnManager turnManager;
    ScoreManager scoreManager;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Use this for initialization
    void Start () {

        scoreManager = FindObjectOfType<ScoreManager>();
        tilePromptSelection = FindObjectOfType<TileSelectionPrompt>();
        tilePromptSelection.HideText();
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        ShowAreaText.SetActive(false);
        turnManager = FindObjectOfType<TurnManager>();
        tileArea = FindObjectOfType<TileArea>();
        caseArea = FindObjectOfType<M_CaseArea>();

        foreach (GameObject obj in SwapButtonsObj)
        {
            obj.SetActive(false);
        }

       

        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea carda in CardAreas)
        {
            if (carda.ThisRow == CardArea.Row.Clue && carda.ThisCardAreaType == CardArea.CardAreaType.Player)
            {
                MyPlayerClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime && carda.ThisCardAreaType == CardArea.CardAreaType.Player)
            {
                MyPlayerCrimeArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Clue && carda.ThisCardAreaType == CardArea.CardAreaType.Opponent)
            {
                MyOpponentClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime && carda.ThisCardAreaType == CardArea.CardAreaType.Opponent)
            {
                MyOpponentCrimeArea = carda;
            }

        }

       
    }

    public void OpponentDiscconnected()
    {
        PlayerDisconnectScreen.SetActive(true);
    }



    public void setPlayer(M_Player P)
    {
        if (P.MyPlayerType == PlayerType.Holmes)
        {
            HolmesPlayer = P;
        }
        else if (P.MyPlayerType == PlayerType.Moriarty)
        {
            MoriartyPlayer = P;
        }
        if (MoriartyPlayer != null && HolmesPlayer != null)
        {
            StartCoroutine("PlayersDrawCards");
        }
    }

    IEnumerator PopUpCaseCard(int Case)
    {
        yield return new WaitForSeconds(2);
        CaseCard card = caseArea.FindCaseCard(Case);
        card.GetComponent<BoxCollider>().enabled = false;
        card.MoveUp(4);
        yield return new WaitForSeconds(3);
        card.MoveBackDown();
        card.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator PlayersDrawCards()
    {
        Debug.Log("Placing cards DOWN");
        caseArea = FindObjectOfType<M_CaseArea>();
        yield return new WaitForSeconds(1.5f);
        DrawCardsFor(PlayerType.Holmes);
    }

    //This is being received by the caseArea
    public void ShowInitialCaseCard()
    {
        StartCoroutine("PopUpCaseCard", 1);
    }
    //Draw cards for Holmes 
    //Once Holmes is done drawing then tells game manager to draw cards for moriarty
    //Then Place case cards down
    public void DrawCardsFor(PlayerType PT)
    {
        Debug.Log(PT);
        switch (PT)
        {
            case PlayerType.Holmes:
                HolmesPlayer.DrawCards(7); ;
                break;
            case PlayerType.Moriarty:
                MoriartyPlayer.DrawCards(7);
                caseArea.PlaceCards();
                break;
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        HolmesPlayer.ResetPlayer();
        MoriartyPlayer.ResetPlayer();

        MyPlayerClueArea.ClearCards();
        MyPlayerCrimeArea.ClearCards();
        MyOpponentClueArea.ClearCards();
        MyOpponentCrimeArea.ClearCards();

        cardDeck.ResetCards();

        caseArea.ClearCards();


        for (int i = 0; i < 3; i++)
        {
            HolmesScoreThisTurn[i] = false;
            MoriartyScoreThisTurn[i] = false;
        }
    }

    public void PlayerEndTurn(PlayerType PT)
    {
        switch (PT)
        {
            case PlayerType.Holmes:
                HolmesEndTurn = true;
                break;
            case PlayerType.Moriarty:
                MoriartyEndTurn = true;
                break;
        }
        if (HolmesEndTurn && MoriartyEndTurn)
        {
            tilePromptSelection.HideText();
            HolmesEndTurn = false;
            MoriartyEndTurn = false;
            EndTurn();
            Debug.Log(CurrentTurnStatus);     
        }
        else
        {
            switch (PT)
            {
                case PlayerType.Holmes:
                    if (HolmesPlayer.isTheLocalPlayer) { tilePromptSelection.SetText("Waiting on Opponent"); }
                    break;
                case PlayerType.Moriarty:
                    if (MoriartyPlayer.isTheLocalPlayer) { tilePromptSelection.SetText("Waiting on Opponent"); }
                    break;
            }
        }
    }

    void ChangeTurn(TurnStatus newStatus)
    {
        CurrentTurnStatus = newStatus;
        if (HolmesPlayer.GetComponent<M_PlayerController>())
        {
            HolmesPlayer.GetComponent<M_PlayerController>().ResetTurnEnded();
        }
        if (MoriartyPlayer.GetComponent<M_PlayerController>())
        {
            MoriartyPlayer.GetComponent<M_PlayerController>().ResetTurnEnded();
        }
    }


    public void EndTurn()
    {
        switch (CurrentTurnStatus)
        {
            case TurnStatus.Turn1:
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                StartCoroutine("PopUpCaseCard", 2);
                ChangeTurn(TurnStatus.Turn2);
                break;

            case TurnStatus.Turn2:
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                StartCoroutine("PopUpCaseCard", 3);
                ChangeTurn(TurnStatus.Turn3);
                break;

            case TurnStatus.Turn3:
                CurrentCaseOn++;
                tilePromptSelection.SetText("Move Clue cards");
                foreach (GameObject obj in SwapButtonsObj)
                {
                    obj.SetActive(true);
                }
                HolmesPlayer.EnableSwapClueCards();
                MoriartyPlayer.EnableSwapClueCards();
                ChangeTurn(TurnStatus.SwitchClueCards);
                break;

            case TurnStatus.SwitchClueCards:
                
                foreach (GameObject obj in SwapButtonsObj)
                {
                    obj.SetActive(false);
                }
                tilePromptSelection.HideText();
                HolmesPlayer.DisableSwapClueCards();
                MoriartyPlayer.DisableSwapClueCards();

                StartCoroutine("CheckForScore");

               
                break;

            case TurnStatus.PickTileMoriarty:
                if (CheckForPickTileHolmes())
                {
                    ChangeTurn(TurnStatus.PickTileHolmes);
                }
                else
                {
                    CheckForTotalScore();
                    ChangeTurn(TurnStatus.Turn1);
                }
                tileArea.ConfirmTiles();
                break;
            case TurnStatus.PickTileHolmes:
                tilePromptSelection.HideText();
                tileArea.ConfirmTiles();
                CheckForTotalScore();
                ChangeTurn(TurnStatus.Turn1);
                break;
            case TurnStatus.BoardInspect:
                tileArea.ConfirmTiles();
                CheckForTotalScore();
                ChangeTurn(TurnStatus.Turn1);
                break;

        }
    }

    void CheckForTotalScore()
    {
        tilePromptSelection.HideText();
        CurrentTurnOn++;
        turnManager.NextTurn();
        CheckForWin();
        StartCoroutine("Reset");
        StartCoroutine("PlayersDrawCards");
        CurrentCaseOn = 1;
    }

    bool CheckForPickTileHolmes()
    {
        List<int> HolmesCaseWon = new List<int>(); 
        for (int i = 0; i < 3; i++)
        {
            if (HolmesScoreThisTurn[i])
            {
                totalHWins++;
                if (caseArea.FindCaseCard(i + 1).PlayerType == PlayerType.Holmes)
                {
                    HolmesCaseWon.Add(i);
                    HolmesPlayer.PlaceHolmesTiles(HolmesTile, caseArea.FindCaseCard(i + 1));
                }
            }
        }

        if (HolmesCaseWon.Count > 0) {
            if (HolmesPlayer.GetComponentInParent<myPlayer>())
            {         
                tilePromptSelection.SetText(" Select " + HolmesCaseWon.Count + " Tiles for Holmes that meet the Case");
            }
            else
            {
                tilePromptSelection.SetText(" Waiting on oponnent to select " + HolmesCaseWon.Count + " H tiles");
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
            if (MoriartyScoreThisTurn[i])
            {
                MoriartyScore++;
                totalMWins++;
            }
        }
        if (MoriartyScore == 2)
        {
            if (HolmesPlayer.GetComponentInParent<myPlayer>() != null) {
                tilePromptSelection.SetText(" Select " + (MoriartyScore - 1) + " open tiles for Moriarty");
            }
            else
            {
                tilePromptSelection.SetText(" Waiting on oponnent to select " + (MoriartyScore - 1) + " M tiles");
            }
            HolmesPlayer.PlaceMoriartyTiles(MoriartTile, 1);
            return true;
        }
        else if (MoriartyScore == 3)
        {
           
            if (HolmesPlayer.GetComponentInParent<myPlayer>() != null) {
                tilePromptSelection.SetText(" Select " + (MoriartyScore - 1) + " open tiles for Moriarty");
            }
            else
            {
                tilePromptSelection.SetText(" Waiting on oponnent to select " + (MoriartyScore - 1) + " M tiles");
            }
            HolmesPlayer.PlaceMoriartyTiles(MoriartTile, 2);
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckForWin()
    {

        //
        if (tileArea.CheckForMoriartyWin())
        {
            MoveToScoreScreen(PlayerType.Moriarty);
        }
        else if (tileArea.CheckForHolmesWin())
        {
            MoveToScoreScreen(PlayerType.Holmes);
        }

        else if (CurrentTurnOn - 1 == 5)
        {
            MoveToScoreScreen(PlayerType.Holmes);
        }
    }

    public void MoveToWinScreen()
    {
        MoveToScoreScreen(PlayerType.Holmes);
    }

    void MoveToScoreScreen(PlayerType PTWon)
    {
        Time.timeScale = 0;
        FindObjectOfType<LevelPropertyManagerMulti>().SaveTileArea(tileArea.Tile2D);
        FindObjectOfType<LevelPropertyManagerMulti>().SetPlayerWon(PTWon);
        FindObjectOfType<LevelPropertyManagerMulti>().SetDetails(CurrentTurnOn - 1, totalHWins, totalMWins);

        FindObjectOfType<LevelManager>().LoadNextLevel();
    }


    IEnumerator SwapCards()
    {
        HolmesCards.Clear();
        MoriartyCards.Clear();
        foreach (ClueCard card in HolmesPlayer.GetCardsHolding()){
            HolmesCards.Add(card); }
        foreach (ClueCard card in MoriartyPlayer.GetCardsHolding()) { MoriartyCards.Add(card); }


        HolmesPlayer.RemoveAllCards();
        MoriartyPlayer.RemoveAllCards();

       // yield return new WaitForSeconds(.1f);

        HolmesPlayer.AddNewCards(MoriartyCards);
        MoriartyPlayer.AddNewCards(HolmesCards);
        yield return null;
    }

    IEnumerator CheckForScore()
    {
        int Case = 1;

        while (Case <= 3)
        {
            ClueCard HolmesCrimeCard;
            ClueCard HolmesClueCard;
            ClueCard MoriartyCrimeCard;
            ClueCard MoriartyClueCard;

            FlipCards(Case, out HolmesCrimeCard, out HolmesClueCard, out MoriartyCrimeCard, out MoriartyClueCard);
            // Move Crime cards up;

            // declare trump
            // Move Clue Cards up
            // declare winner and place tile
            int Effect = caseArea.FindCaseCard(Case).CardEffect;


            CardType Trump = scoreManager.CheckForTrump(HolmesCrimeCard, MoriartyCrimeCard, HolmesClueCard, MoriartyClueCard, Effect);
            if (scoreManager.CheckForHolmesWin(Trump, HolmesClueCard, MoriartyClueCard, HolmesCrimeCard, MoriartyCrimeCard, Effect))
            {
                HolmesScoreThisTurn[Case - 1] = true;
                HolmesCrimeCard.FadeCard();
                MoriartyClueCard.FadeCard();
                MoriartyCrimeCard.FadeCard();
                tileArea.PlaceTile(HolmesTile, HolmesClueCard.Number, PlayerType.Holmes);
            }
            else
            {
                MoriartyScoreThisTurn[Case - 1] = true;
                HolmesCrimeCard.FadeCard();
                MoriartyClueCard.FadeCard();
                HolmesClueCard.FadeCard();
                tileArea.PlaceTile(MoriartTile, MoriartyCrimeCard.Number, PlayerType.Moriarty);
            }
            Case ++;
            yield return new WaitForSeconds(.8f);
        }

        if (CheckForPickTileMoriarty())
        {
            ChangeTurn(TurnStatus.PickTileMoriarty);
        }
        else if (CheckForPickTileHolmes())
        {
            ChangeTurn(TurnStatus.PickTileHolmes);
        }
        else
        {
            ChangeTurn(TurnStatus.BoardInspect);
        }
        Debug.Log(CurrentTurnStatus);
        yield return null;
    }

    IEnumerator MoveCardsUp(Card HolmesCard, Card MoriartyCard)
    {
        Transform HolmesCardTransform = ShowArea.GetComponentInChildren<HolmesArea>().transform;
        Transform MoriartyCardTransfrom = ShowArea.GetComponentInChildren<MoriartyArea>().transform;
        HolmesCard.transform.SetParent(HolmesCardTransform);
        HolmesCard.transform.position = HolmesCardTransform.position;
        HolmesCard.transform.localScale = new Vector3(5, 7, .05f);

        MoriartyCard.transform.SetParent(MoriartyCardTransfrom);
        MoriartyCard.transform.position = MoriartyCardTransfrom.position;
        MoriartyCard.transform.localScale = new Vector3(5, 7, .05f);

        yield return new WaitForSeconds(10f);
    }


    void FlipCards(int Case, out ClueCard HolmesCrimeCard, out ClueCard HolmesClueCard, out ClueCard MoriartyCrimeCard, out ClueCard MoriartyClueCard)
    {
        HolmesCrimeCard = null;
        HolmesClueCard = null;
        MoriartyCrimeCard = null;
        MoriartyClueCard = null;

        if (HolmesPlayer.GetComponentInParent<myPlayer>()) { HolmesCrimeCard = MyPlayerCrimeArea.FlipCard(Case); }
        else { HolmesCrimeCard = MyOpponentCrimeArea.FlipCard(Case); }
        if (HolmesPlayer.GetComponentInParent<myPlayer>()) { HolmesClueCard = MyPlayerClueArea.FlipCard(Case); }
        else { HolmesClueCard = MyOpponentClueArea.FlipCard(Case); }

        if (MoriartyPlayer.GetComponentInParent<myPlayer>()) { MoriartyCrimeCard = MyPlayerCrimeArea.FlipCard(Case); }
        else { MoriartyCrimeCard = MyOpponentCrimeArea.FlipCard(Case); }
        if (MoriartyPlayer.GetComponentInParent<myPlayer>()) { MoriartyClueCard = MyPlayerClueArea.FlipCard(Case); }
        else { MoriartyClueCard = MyOpponentClueArea.FlipCard(Case); }

        //if (MyPlayerCrimeArea.PlayerAttached == HolmesPlayer){HolmesCrimeCard = MyPlayerCrimeArea.FlipCard(Case);}
        //else { MoriartyCrimeCard = MyPlayerCrimeArea.FlipCard(Case); }
        //if (MyPlayerClueArea.PlayerAttached == HolmesPlayer) { HolmesClueCard = MyPlayerClueArea.FlipCard(Case); }
        //else { MoriartyClueCard = MyPlayerClueArea.FlipCard(Case); }

        //// TODO fix this!!!
        //if (MyOpponentCrimeArea.PlayerAttached == HolmesPlayer) { HolmesCrimeCard = MyOpponentCrimeArea.FlipCard(Case); }
        //else { MoriartyCrimeCard = MyOpponentCrimeArea.FlipCard(Case); }
        //if (MyOpponentClueArea.PlayerAttached == HolmesPlayer) { HolmesClueCard = MyOpponentClueArea.FlipCard(Case); }
        //else { MoriartyClueCard = MyOpponentClueArea.FlipCard(Case); }

        caseArea.FlipCard(Case);
    }


  
}
