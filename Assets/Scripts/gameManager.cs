using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {


    public GameObject HolmesTile;
    public GameObject MoriartTile;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    public GameObject ShowAreaText;
    public GameObject ShowArea;

    TileArea tileArea;
    TileSelectionPrompt tilePromptSelection;


    public enum TurnStatus { Turn1, Turn2, Turn3, SwitchClueCards, PickTileMoriarty, PickTileHolmes, BoardInspect }
    public TurnStatus CurrentTurnStatus = TurnStatus.Turn1;

    public int CurrentTurnOn = 1;
    public int CurrentCaseOn = 1;

    CardArea CrimeArea;
    CardArea ClueArea;
    AICardArea ACrimeArea;
    AICardArea AClueArea;

    CaseArea caseArea;

    GamePlayer[] Players;
    GamePlayer HolmesPlayer;
    GamePlayer MoriartyPlayer;

    public ClueDeck cardDeck;

    public List<ClueCard> HolmesCards = new List<ClueCard>();
    public List<ClueCard> MoriartyCards = new List<ClueCard>();

    public bool[] HolmesScoreThisTurn = { false, false, false };
    bool[] MoriartyScoreThisTurn = { false, false, false };

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

        scoreManager = GetComponent<ScoreManager>();
        tilePromptSelection = FindObjectOfType<TileSelectionPrompt>();
        //tilePromptSelection.gameObject.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        ShowAreaText.SetActive(false);
        turnManager = FindObjectOfType<TurnManager>();
        tileArea = FindObjectOfType<TileArea>();
        caseArea = FindObjectOfType<CaseArea>();

        foreach (GameObject obj in SwapButtonsObj)
        {
            obj.SetActive(false);
        }

        Players = FindObjectsOfType<GamePlayer>();
        foreach (GamePlayer player in Players)
        {
            switch (player.MyPlayerType)
            {
                case PlayerType.Holmes:
                    HolmesPlayer = player;
                    break;
                case PlayerType.Moriarty:
                    MoriartyPlayer = player;
                    break;
            }
        }

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

        StartCoroutine("PlayersDrawCards");
    }

    IEnumerator PlayersDrawCards()
    {
        yield return new WaitForSeconds(1.5f);
        HolmesPlayer.DrawCards(7);
        MoriartyPlayer.DrawCards(7);
        caseArea.PlaceCards();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("PopUpCaseCard", 1);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        HolmesPlayer.ResetPlayer();
        MoriartyPlayer.ResetPlayer();

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
        ChangeTurn(TurnStatus.Turn1);
        //PlayersDrawCards();
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
            HolmesEndTurn = false;
            MoriartyEndTurn = false;
            EndTurn();
        }
    }

    void ChangeTurn(TurnStatus newStatus)
    {
        CurrentTurnStatus = newStatus;
        if (HolmesPlayer.GetComponent<PlayerController>()) { HolmesPlayer.GetComponent<PlayerController>().CheckEndTurn(); }
        if (MoriartyPlayer.GetComponent<PlayerController>()) { MoriartyPlayer.GetComponent<PlayerController>().CheckEndTurn(); }
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
                // tilePromptSelection.gameObject.SetActive(true);
                //tilePromptSelection.gameObject.GetComponent<Text>().text = "Move Clue cards";
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
                //tilePromptSelection.gameObject.SetActive(false);
                tilePromptSelection.HideText();
                HolmesPlayer.DisableSwapClueCards();
                MoriartyPlayer.DisableSwapClueCards();

                StartCoroutine("CheckForScore");

               
                break;

            case TurnStatus.PickTileMoriarty:
                tilePromptSelection.HideText();
                //tilePromptSelection.gameObject.SetActive(false);
                if (CheckForPickTileHolmes())
                {
                    ChangeTurn(TurnStatus.PickTileHolmes);
                }
                else
                {
                    CheckForTotalScore();
                }
                tileArea.ConfirmTiles();
                break;
            case TurnStatus.PickTileHolmes:
                tilePromptSelection.HideText();
                //tilePromptSelection.gameObject.SetActive(false);
                tileArea.ConfirmTiles();
                CheckForTotalScore();
                break;
            case TurnStatus.BoardInspect:
                tileArea.ConfirmTiles();
                CheckForTotalScore();

                break;

        }
    }

    void CheckForTotalScore()
    {
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
            if (HolmesPlayer.GetComponent<PlayerController>())
            {         
                if (HolmesPlayer.GetComponent<PlayerController>() != null) {
                    tilePromptSelection.SetText(" Select " + HolmesCaseWon.Count + " Tiles for Holmes that meet the Case");
                    //tilePromptSelection.gameObject.SetActive(true);
                    //tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + HolmesCaseWon.Count + " Tiles for Holmes that meet the Case";
                }
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
            if (HolmesPlayer.GetComponent<PlayerController>() != null) {
                tilePromptSelection.SetText(" Select " + (MoriartyScore - 1) + " open tiles for Moriarty");
               // tilePromptSelection.gameObject.SetActive(true);
               // tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + (MoriartyScore - 1) + " open tiles for Moriarty";
            }
           
            HolmesPlayer.PlaceMoriartyTiles(MoriartTile, 1);
            //ChangeTurn(TurnStatus.PickTileMoriarty);
            return true;
        }
        else if (MoriartyScore == 3)
        {
           
            if (HolmesPlayer.GetComponent<PlayerController>() != null) {
                tilePromptSelection.SetText(" Select " + (MoriartyScore - 1) + " open tiles for Moriarty");
                //tilePromptSelection.gameObject.SetActive(true);
                //tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + (MoriartyScore - 1) + " open tiles for Moriarty";
            }
           
            HolmesPlayer.PlaceMoriartyTiles(MoriartTile, 2);
           // ChangeTurn(TurnStatus.PickTileMoriarty);
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

    void MoveToScoreScreen(PlayerType PTWon)
    {
        Time.timeScale = 0;
        FindObjectOfType<LevelPropertyManager>().SaveTileArea(tileArea.Tile2D);
        FindObjectOfType<LevelPropertyManager>().SetPlayerWon(PTWon);
        FindObjectOfType<LevelPropertyManager>().SetDetails(CurrentTurnOn - 1, totalHWins, totalMWins);

        FindObjectOfType<LevelManager>().LoadNextLevel();
    }

    IEnumerator PopUpCaseCard(int Case)
    {
        CaseCard card = caseArea.FindCaseCard(Case);
        card.GetComponent<BoxCollider>().enabled = false;
        card.MoveUp(4);
        yield return new WaitForSeconds(3);
        card.MoveBackDown();
        card.GetComponent<BoxCollider>().enabled = true;
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

            // Find the effect thats in play for this case
            int Effect = caseArea.FindCaseCard(Case).CardEffect;


            // declare trump
            // Move Clue Cards up
            // declare winner and place tile

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

        if (CrimeArea.PlayerAttached == HolmesPlayer){HolmesCrimeCard = CrimeArea.FlipCard(Case);}
        else { MoriartyCrimeCard = CrimeArea.FlipCard(Case); }
        if (ClueArea.PlayerAttached == HolmesPlayer) { HolmesClueCard = ClueArea.FlipCard(Case); }
        else { MoriartyClueCard = ClueArea.FlipCard(Case); }


        if (ACrimeArea.PlayerAttached == HolmesPlayer) { HolmesCrimeCard = ACrimeArea.FlipCard(Case); }
        else { MoriartyCrimeCard = ACrimeArea.FlipCard(Case); }
        if (AClueArea.PlayerAttached == HolmesPlayer) { HolmesClueCard = AClueArea.FlipCard(Case); }
        else { MoriartyClueCard = AClueArea.FlipCard(Case); }

        caseArea.FlipCard(Case);
    }


   
}
