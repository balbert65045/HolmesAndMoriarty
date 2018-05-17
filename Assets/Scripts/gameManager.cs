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

    Player[] Players;
    Player HolmesPlayer;
    Player MoriartyPlayer;

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

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Use this for initialization
    void Start () {

        tilePromptSelection = FindObjectOfType<TileSelectionPrompt>();
        tilePromptSelection.gameObject.SetActive(false);
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

        Players = FindObjectsOfType<Player>();
        foreach (Player player in Players)
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
            if (HolmesPlayer.GetComponent<PlayerController>()) { HolmesPlayer.GetComponent<PlayerController>().CheckEndTurn(); }
            if (MoriartyPlayer.GetComponent<PlayerController>()) { MoriartyPlayer.GetComponent<PlayerController>().CheckEndTurn(); }
            EndTurn();
        }
    }

    public void EndTurn()
    {
        switch (CurrentTurnStatus)
        {
            case TurnStatus.Turn1:
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                CurrentTurnStatus = TurnStatus.Turn2;
                break;

            case TurnStatus.Turn2:
                CurrentCaseOn++;
                StartCoroutine("SwapCards");
                CurrentTurnStatus = TurnStatus.Turn3;
                break;

            case TurnStatus.Turn3:
                CurrentCaseOn++;
                tilePromptSelection.gameObject.SetActive(true);
                tilePromptSelection.gameObject.GetComponent<Text>().text = "Move Clue cards";
                foreach (GameObject obj in SwapButtonsObj)
                {
                    obj.SetActive(true);
                }
                HolmesPlayer.EnableSwapClueCards();
                MoriartyPlayer.EnableSwapClueCards();
                CurrentTurnStatus = TurnStatus.SwitchClueCards;
                break;

            case TurnStatus.SwitchClueCards:
                
                foreach (GameObject obj in SwapButtonsObj)
                {
                    obj.SetActive(false);
                }
                tilePromptSelection.gameObject.SetActive(false);
                HolmesPlayer.DisableSwapClueCards();
                MoriartyPlayer.DisableSwapClueCards();

                StartCoroutine("CheckForScore");

               
                break;

            case TurnStatus.PickTileMoriarty:
                tilePromptSelection.gameObject.SetActive(false);
                if (CheckForPickTileHolmes())
                {
                    CurrentTurnStatus = TurnStatus.PickTileHolmes;
                }
                else
                {
                    CheckForTotalScore();
                    CurrentTurnStatus = TurnStatus.Turn1;
                }  
                break;
            case TurnStatus.PickTileHolmes:
                tilePromptSelection.gameObject.SetActive(false);
                CheckForTotalScore();
                CurrentTurnStatus = TurnStatus.Turn1;
                break;
            case TurnStatus.BoardInspect:
                CheckForTotalScore();
                CurrentTurnStatus = TurnStatus.Turn1;
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
                    tilePromptSelection.gameObject.SetActive(true);
                    tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + HolmesCaseWon.Count + " Tiles for Holmes that meet the Case";
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
                tilePromptSelection.gameObject.SetActive(true);
                tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + (MoriartyScore - 1) + " open tiles for Moriarty";
            }
            CurrentTurnStatus = TurnStatus.PickTileMoriarty;
            HolmesPlayer.PlaceMoriartyTiles(MoriartTile, 1);
            return true;
        }
        else if (MoriartyScore == 3)
        {
           
            if (HolmesPlayer.GetComponent<PlayerController>() != null) {
                tilePromptSelection.gameObject.SetActive(true);
                tilePromptSelection.gameObject.GetComponent<Text>().text = " Select " + (MoriartyScore - 1) + " open tiles for Moriarty";
            }
            CurrentTurnStatus = TurnStatus.PickTileMoriarty;
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

    void MoveToScoreScreen(PlayerType PTWon)
    {
        Time.timeScale = 0;
        FindObjectOfType<LevelPropertyManager>().SaveTileArea(tileArea.Tile2D);
        FindObjectOfType<LevelPropertyManager>().SetPlayerWon(PTWon);
        FindObjectOfType<LevelPropertyManager>().SetDetails(CurrentTurnOn - 1, totalHWins, totalMWins);

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

            CardType Trump = CheckForTrump(HolmesCrimeCard, MoriartyCrimeCard);
            if (CheckForHolmesWin(Trump, HolmesClueCard, MoriartyClueCard))
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
            CurrentTurnStatus = TurnStatus.PickTileMoriarty;
        }
        else if (CheckForPickTileHolmes())
        {
            CurrentTurnStatus = TurnStatus.PickTileHolmes;
        }
        else
        {
            CurrentTurnStatus = TurnStatus.BoardInspect;
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


    // put these in a score controller class??
   CardType CheckForTrump(ClueCard HolmesCrimeCard, ClueCard MoriartyCrimeCard)
    {

        if (HolmesCrimeCard.Number > MoriartyCrimeCard.Number)
        {
            // check for wrap around effect 
            switch (MoriartyCrimeCard.Number)
            {
                case 1:
                    if (HolmesCrimeCard.Number > 13) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (HolmesCrimeCard.Number > 14) { return MoriartyCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (HolmesCrimeCard.Number > 15) { return MoriartyCrimeCard.ThisCardType; }
                    break;
            }
            return HolmesCrimeCard.ThisCardType;
        }
        else
        {
            switch (HolmesCrimeCard.Number)
            {
                case 1:
                    if (MoriartyCrimeCard.Number > 13) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 2:
                    if (MoriartyCrimeCard.Number > 14) { return HolmesCrimeCard.ThisCardType; }
                    break;
                case 3:
                    if (MoriartyCrimeCard.Number > 15) { return HolmesCrimeCard.ThisCardType; }
                    break;
            }
            return MoriartyCrimeCard.ThisCardType; 
        }
    }

    // Check to see who has the highest card with trump in play 
    // first check who has trump and then if either both do or do not check for highest card
    bool CheckForHolmesWin(CardType Trump, ClueCard HolmesClueCard, ClueCard MoriartyClueCard)
    {
        // check if both have trump
        if (HolmesClueCard.ThisCardType == Trump && MoriartyClueCard.ThisCardType == Trump)
        {
            if (HolmesClueCard.Number > MoriartyClueCard.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // check if both only player has trump
        else if (HolmesClueCard.ThisCardType == Trump)
        {
            return true;
        }
        // check if both only AI has trump
        else if (MoriartyClueCard.ThisCardType == Trump)
        {
            return false;
        }

        // check if neither has trump
        else
        {
            if (HolmesClueCard.Number > MoriartyClueCard.Number)
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
