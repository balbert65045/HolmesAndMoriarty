using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class M_PlayerController : M_Player {

    // Use this for initialization
    public int HolmesTilesToPlace = 0;
    public int MoriartyTilesToPlace = 0;

    public LayerMask BoardLayer;
    public LayerMask Card_TileLayer;
    public bool ClueAreaActive = true;
    public bool CrimeAreaActive = true;

    M_CardHand cardHand;
    M_gameManager gamemanager;
    ClueCard SelectedCard;
    Vector3 SelectedCardOriginalPosition;
    TileArea tileArea;
    GameObject MoriartyTile;
    GameObject HolmesTile;
    CardArea ClueArea;
    CardArea CrimeArea;
    EndTurnButton endTurnButton;
   
    bool b_EnableSwapClueCards = false;
    bool Endedturn = false;

    GameObject LinkedLobbyPlayer;

    Card HighlightedCard;

    ScreenOverlay PlayerScreen;

    public List<CaseCard> HolmesCaseCardsWonThisTurn;

    void Start() {
        ClueAreaActive = true;
        CrimeAreaActive = true;
        tileArea = FindObjectOfType<TileArea>();
        gamemanager = FindObjectOfType<M_gameManager>();
        endTurnButton = FindObjectOfType<EndTurnButton>();
        cardHand = GetComponentInChildren<M_CardHand>();
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea carda in CardAreas)
        {
            if (carda.ThisCardAreaType == CardArea.CardAreaType.Player)
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
        }

        if (!isServer)
        {
            Debug.Log("Setting players command");
            CmdSetPlayers(); 
        }
        else
        {
            Debug.Log("this is server");
        }
    }



    
    [Command]
    void CmdSetPlayers()
    {
        Debug.Log("Setting players on server");
        M_PlayerController[] Controllers = FindObjectsOfType<M_PlayerController>();
        foreach (M_PlayerController controller in Controllers)
        {
            controller.SetPlayer();
        }
    }


    public void SetPlayer()
    {
        Debug.Log("Set player method");
        StartCoroutine("SetPlayers");
    }
    // Wait a delay for all players to spawn on server and then set them all 
    IEnumerator SetPlayers()
    {
        //need to wait have a check that all players have spawned on both client and host 
        yield return new WaitForSeconds(.1f);
        if (isServer)
        {
            Debug.Log("Server Acting");
            RpcSetPlayer(MyPlayerType, LinkedLobbyPlayer);
        }
    }

    //Set the player type on the server
    public void SetPlayerType(PlayerType PT, LobbyPlayer LP)
    {
        LinkedLobbyPlayer = LP.gameObject;
        Debug.Log("Player being set");
        Debug.Log(PT);
        MyPlayerType = PT;
    }


    [ClientRpc]
    void RpcSetPlayer(PlayerType PT, GameObject LP)
    {
        Debug.Log("RPC Happening");

        IEnumerator PlayerSet = SetPlayer(PT, LP);
        StartCoroutine(PlayerSet);

    }

    IEnumerator SetPlayer(PlayerType PT, GameObject LP)
    {
        yield return new WaitForSeconds(.2f);
        M_PlayerController[] controllers = FindObjectsOfType<M_PlayerController>();
        Debug.Log(controllers.Length);
        LinkedLobbyPlayer = LP;
        if (LP.GetComponent<LobbyPlayer>().LocalPlayer)
        {
            isTheLocalPlayer = true;
            if (FindObjectOfType<myPlayer>() != null)
            {
                Debug.Log("myPlayer found");
                this.transform.SetParent(FindObjectOfType<myPlayer>().transform);
                // Show what player they are
                if (PT == PlayerType.Holmes) { PlayerScreen = FindObjectOfType<HolmesScreen>(); }
                else if(PT == PlayerType.Moriarty) { PlayerScreen = FindObjectOfType<MoriartyScreen>(); }
                PlayerScreen.EnableOverlay();
                yield return new WaitForSeconds(4f);
                PlayerScreen.DisableOverlay();
            }
            else { Debug.Log("No myPlayer found"); }
        }
        else
        {
            if (FindObjectOfType<myOponnent>() != null)
            {
                Debug.Log("myOponent found");
                this.transform.SetParent(FindObjectOfType<myOponnent>().transform);
            }
            else { Debug.Log("No myOponent found"); }
        }
        transform.localPosition = Vector3.zero;
        MyPlayerType = PT;
        FindObjectOfType<M_gameManager>().setPlayer(this);
        if (GetComponentInParent<myPlayer>() != null) { GetComponentInParent<myPlayer>().SetIndicator(); }
        else { GetComponentInParent<myOponnent>().SetIndicator(); }
    }

    public override void SetupPlayer()
    {
        base.SetupPlayer();
    }

    public override void EnableSwapClueCards()
    {
        base.EnableSwapClueCards();
        b_EnableSwapClueCards = true;
        endTurnButton.EnableEndTurn();
    }

    public override void DisableSwapClueCards()
    {
        base.DisableSwapClueCards();
        b_EnableSwapClueCards = false;
    }

    public override bool PlaceMoriartyTiles(GameObject MoriartyTilePrefab, int number)
    {
        TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
        List<TileSpot> OpenTileSpots = new List<TileSpot>();
        foreach (TileSpot TS in TilesSpots)
        {
            if (!TS.Used) { OpenTileSpots.Add(TS); }
        }
        if (OpenTileSpots.Count == 0) { return false; }
        MoriartyTilesToPlace = number;
        if (GetComponentInParent<myPlayer>() != null) { endTurnButton.DisableEndTurn(); }
        MoriartyTile = MoriartyTilePrefab;
        return true;
    }

    public override bool PlaceHolmesTiles(GameObject HolmesTilePrefab, CaseCard HolmesCaseCard)
    {
        // check tiles to make sure one exist that can be gotten 
        TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
        List<TileSpot> OpenTileSpots = new List<TileSpot>();
        foreach (TileSpot TS in TilesSpots)
        {
            if (!TS.Used)
            {
                if (HolmesCaseCard.CardTypes.Contains(TS.ThisCardType)) { OpenTileSpots.Add(TS); }
            }
        }
        if (OpenTileSpots.Count == 0) {
            endTurnButton.EnableEndTurn();
            return false;
        }
        HolmesTilesToPlace++;
        if (GetComponentInParent<myPlayer>() != null) { endTurnButton.DisableEndTurn(); }
        HolmesTile = HolmesTilePrefab;
        HolmesCaseCard.MoveUp(HolmesTilesToPlace);
        HolmesCaseCardsWon.Add(HolmesCaseCard);
        HolmesCaseCardsWonThisTurn.Add(HolmesCaseCard);
        return true;
    }

  

    public void PlayerEndTurn()
    {
        CmdEndTurn();

        if (GetComponentInParent<myPlayer>())
        {
                endTurnButton.DisableEndTurn();
                Endedturn = true;
        }
    }

    [Command]
    void CmdEndTurn()
    {
        RpcEndTurn();
    }

    [ClientRpc]
    void RpcEndTurn()
    {
        gamemanager.PlayerEndTurn(MyPlayerType);
    }

    // Update is called once per frame
    void Update() {
        if (!isTheLocalPlayer) { return; }
        if (Endedturn) { return; }
        // On Mouse/Finger up 
        if (Input.GetMouseButtonUp(0))
        {
            Remove_Unselect_PlaceDownCard();
            CheckToDecreaseCardSize();
        }
        // On Mouse/finger pressed Down 
        else if (Input.GetMouseButtonDown(0))
        {
            SelectCard_PlaceTileDown();
            CheckActiveAreas();
            CheckForSwapCardsButton();
            CheckToIncreaseCardSize();
        }

        // On Holding Down Mouse/Finger 
        else if (Input.GetMouseButton(0))
        {
            CheckForCardFollow();
        }
    }

    void CheckToDecreaseCardSize()
    {
        if (HighlightedCard == null) { return; }
        if (HighlightedCard.GetComponentInParent<M_CaseArea>() != null)
        {
            HighlightedCard.MoveBackDown();
            HighlightedCard = null;
        }
    }

    void CheckToIncreaseCardSize()
    {
        RaycastHit Hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Hit, 100f, Card_TileLayer))
        {
            //Select Card
            if (Hit.transform.GetComponent<ClueCard>())
            {

                ClueCard card = Hit.transform.GetComponent<ClueCard>();
                if (card.GetComponentInParent<CardArea>() != null)
                {
                    if (card.GetComponentInParent<RowAreaPosition>().Case != gamemanager.CurrentCaseOn)
                    {
                        Debug.Log("Moving Card up");
                        HighlightedCard = card;
                        card.MoveUp(1);
                    }
                }
            }
            else if (Hit.transform.GetComponent<CaseCard>())
            {
                CaseCard card = Hit.transform.GetComponent<CaseCard>();
                HighlightedCard = card;
                card.MoveUp(1);
            }
        }
    }



    void CheckForSwapCardsButton()
    {
        RaycastHit Hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Hit, 100f))
        {
         
            if (Hit.transform.GetComponent<SwapButtons>() != null)
            {
                int buttonHit = Hit.transform.GetComponent<SwapButtons>().Button;
                if (buttonHit == 1)
                {
                    CmdSwapClueCards(1, 2);
                }
                else if (buttonHit == 2)
                {
                    CmdSwapClueCards(2, 3);
                }
            }
        }
    }

    public void ResetTurnEnded()
    {
        Endedturn = false;
        if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.PickTileHolmes ||
            gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.PickTileMoriarty ||
            gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.BoardInspect ||
             gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.SwitchClueCards
            )
        {
            CheckEndTurn();
        }
    }

    //Timing issue!!!??????
    public void CheckEndTurn()
    {
       
        if (GetComponentInParent<myPlayer>() == null) { return; }
        Debug.Log(gamemanager.CurrentTurnStatus);
        if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.Turn1 || gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.Turn2
            || gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.Turn3)
        {
            if (!ClueArea.CheckForAvailableSpace(gamemanager.CurrentCaseOn) && !CrimeArea.CheckForAvailableSpace(gamemanager.CurrentCaseOn))
            {
                endTurnButton.EnableEndTurn();
            }
            else
            {
                endTurnButton.DisableEndTurn();
            }
        }
        else if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.SwitchClueCards)
        {
            endTurnButton.EnableEndTurn();
        }

        else if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.PickTileHolmes)
        {
            Debug.Log(HolmesTilesToPlace);
            if (HolmesTilesToPlace == 0)
            {
                endTurnButton.EnableEndTurn();
            }
            else if (!tileArea.CheckForOpenTiles())
            {
                endTurnButton.EnableEndTurn();
            }

            else
            {
                endTurnButton.DisableEndTurn();
            }
        }
        else if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.PickTileMoriarty)
        {
            Debug.Log(MoriartyTilesToPlace);
            if (MoriartyTilesToPlace == 0)
            {
                endTurnButton.EnableEndTurn();
            }
            else if (!tileArea.CheckForOpenTiles())
            {
                endTurnButton.EnableEndTurn();
            }

            else
            {
                endTurnButton.DisableEndTurn();
            }
        }
        else if (gamemanager.CurrentTurnStatus == M_gameManager.TurnStatus.BoardInspect)
        {
            endTurnButton.EnableEndTurn();
        }

    }

    void CheckActiveAreas()
    {
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea CA in CardAreas)
        {
            if (CA.ThisCardAreaType == CardArea.CardAreaType.Player)
            {
                switch (CA.ThisRow)
                {
                    case CardArea.Row.Crime:
                        if (CA.CheckForAvailableSpace(gamemanager.CurrentCaseOn))
                        {
                            CrimeAreaActive = true;
                        }
                        else
                        {
                            CrimeAreaActive = false;
                        }
                        break;
                    case CardArea.Row.Clue:
                        if (CA.CheckForAvailableSpace(gamemanager.CurrentCaseOn))
                        {
                            ClueAreaActive = true;
                        }
                        else
                        {
                            ClueAreaActive = false;
                        }
                        break;
                }
            }
        }
    }

    void SelectCard_PlaceTileDown()
    {
        RaycastHit Hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Hit, 100f, Card_TileLayer))
        {
            //Select Card
            if (Hit.transform.GetComponent<ClueCard>())
            {
                CheckToSelectCard(Hit.transform);
            }
            // Place Tile Down
            else if (Hit.transform.GetComponent<TileSpot>())
            {
                CheckToPlaceDownTile(Hit.transform);
            }
        }
    }

    void Remove_Unselect_PlaceDownCard()
    {
        if (SelectedCard != null)
        {
            RaycastHit Hit;
            Ray ray = new Ray(SelectedCard.transform.position, Vector3.down);
            if (b_EnableSwapClueCards)
            {
                if (Physics.Raycast(ray, out Hit))
                {
                    if (Hit.transform.GetComponent<ClueCard>() && Hit.transform.GetComponentInParent<CardArea>() && Hit.transform.GetComponentInParent<CardArea>().ThisCardAreaType == CardArea.CardAreaType.Player &&
                        Hit.transform.GetComponentInParent<CardArea>().ThisRow == CardArea.Row.Clue)
                    {
                        ClueCard CardSwapping = Hit.transform.GetComponent<ClueCard>();
                        CardArea ClueArea = Hit.transform.GetComponentInParent<CardArea>();
                        SwapClueCards(CardSwapping, ClueArea);
                    }
                    else
                    {
                        ResetCard();
                    }
                }
            }
            else
            {
                if (Physics.Raycast(ray, out Hit, 50f, BoardLayer))
                {
                   
                    // check for placing card
                    if (Hit.transform.GetComponent<CardArea>() && Hit.transform.GetComponent<CardArea>().ThisCardAreaType == CardArea.CardAreaType.Player)
                    {
                        //CheckForPlaceCard(Hit.transform);
                        CheckToRemoveCardOrPlaceDown();
                    }
                    // check to remove card
                    else
                    {
                        CheckToRemoveCardOrPlaceDown();
                    }
                    CheckEndTurn();
                }
            }       
            UnselectCard();
        }
    }

    void ResetCard()
    {
        SelectedCard.transform.position = SelectedCardOriginalPosition;
    }

    void SwapClueCards(ClueCard cardSwapping, CardArea clueArea)
    {
        int cardSwappingCase = cardSwapping.GetComponentInParent<RowAreaPosition>().Case;
        int SelectedCardSwappingCase = SelectedCard.GetComponentInParent<RowAreaPosition>().Case;
        CmdSwapClueCards(cardSwappingCase, SelectedCardSwappingCase);
    }

    [Command]
    void CmdSwapClueCards(int cardSwappingCase, int SelectedCardSwappingCase)
    {
        RpcSwapClueCards(cardSwappingCase, SelectedCardSwappingCase);
    }

    [ClientRpc]
    void RpcSwapClueCards(int cardSwappingCase, int SelectedCardSwappingCase)
    {
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea CA in CardAreas)
        {
            if (GetComponentInParent<myOponnent>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Opponent && CA.ThisRow == CardArea.Row.Clue)
                {
                    ClueCard cardSwapping = CA.GetCard(cardSwappingCase);
                    ClueCard selectedCard = CA.GetCard(SelectedCardSwappingCase);
                    CA.MoveCard(cardSwapping);
                    CA.MoveCard(selectedCard);
                    CA.PlaceCardDown(cardSwapping, SelectedCardSwappingCase);
                    CA.PlaceCardDown(selectedCard, cardSwappingCase);
                }
            }
            else if (GetComponentInParent<myPlayer>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Player && CA.ThisRow == CardArea.Row.Clue)
                {
                    ClueCard cardSwapping = CA.GetCard(cardSwappingCase);
                    ClueCard selectedCard = CA.GetCard(SelectedCardSwappingCase);
                    CA.MoveCard(cardSwapping);
                    CA.MoveCard(selectedCard);
                    CA.PlaceCardUp(cardSwapping, SelectedCardSwappingCase);
                    CA.PlaceCardUp(selectedCard, cardSwappingCase);
                }
            }
        }
    }

    void CheckToRemoveCardOrPlaceDown()
    {
        if (SelectedCard.GetComponentInParent<RowAreaPosition>() != null)
        {
            //SelectedCard.GetComponentInParent<CardArea>().RemoveCard(SelectedCard);
            //cardHand.AddCard(SelectedCard, gamemanager.CurrentCaseOn - 1);
            int RowValue = (int)SelectedCard.GetComponentInParent<CardArea>().ThisRow;
            CmdRemoveCard(RowValue);
            SelectedCard.transform.position = SelectedCardOriginalPosition;
            return;
        }
        else
        {
            Debug.Log("Placing Card");
            CardArea[] CardAreas = FindObjectsOfType<CardArea>();
            foreach (CardArea CA in CardAreas)
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Player)
                {
                    if (CrimeAreaActive)
                    {
                        if (CA.ThisRow == CardArea.Row.Crime)
                        {
                            CheckForPlaceCard(CA.transform);
                            return;
                        }
                    }
                    else if (ClueAreaActive)
                    {
                        if (CA.ThisRow == CardArea.Row.Clue)
                        {
                            CheckForPlaceCard(CA.transform);
                            return;
                        }
                    }
                }
            }

        }
        SelectedCard.transform.position = SelectedCardOriginalPosition;
    }

    void CheckForPlaceCard(Transform HitTransform)
    {
      

        if (HitTransform.GetComponent<CardArea>().ThisCardAreaType == CardArea.CardAreaType.Player && HitTransform.GetComponent<CardArea>().CheckForAvailableSpace(gamemanager.CurrentCaseOn))
        {
            // Moving from another row area
            if (SelectedCard.GetComponentInParent<RowAreaPosition>() != null)
            {
                SelectedCard.GetComponentInParent<CardArea>().MoveCard(SelectedCard);
                HitTransform.GetComponent<CardArea>().PlaceCard(SelectedCard, gamemanager.CurrentCaseOn);
            }
            // Moving card from hand
            else
            {
                // CANT pass data through server like that find another way 
                int CardPosition = cardHand.GetCardPosition(SelectedCard);
                int RowValue = (int)HitTransform.GetComponent<CardArea>().ThisRow;
                CmdPlaceCard(RowValue, CardPosition);
                // DO a command and RPC that places the card out of hand and into a card area dependent on if they are player or opponent
                //GetComponentInChildren<M_CardHand>().RemoveCard(SelectedCard);
                //HitTransform.GetComponent<CardArea>().PlaceCard(SelectedCard, gamemanager.CurrentCaseOn);
                //   CmdPlaceCard(HitTransform.GetComponent<CardArea>().ThisRow, SelectedCard);

            }
            return;  
        }
        SelectedCard.transform.position = SelectedCardOriginalPosition;
    }

    [Command]
    public void CmdPlaceCard(int RowValue, int CardPos)
    {
        RpcPlaceCard(RowValue, CardPos);
    }

    [ClientRpc]
    public void RpcPlaceCard(int RowValue, int CardPos)
    {
        Debug.Log("RPC Moving Card");
        ClueCard cardSelected = GetComponentInChildren<M_CardHand>().GetCardFromPosition(CardPos);
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea CA in CardAreas)
        {
            if (GetComponentInParent<myOponnent>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Opponent && (int)CA.ThisRow == RowValue) {
                    cardHand.RemoveCard(cardSelected);
                    CA.PlaceCardDown(cardSelected, gamemanager.CurrentCaseOn);
                }
            }
            else if (GetComponentInParent<myPlayer>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Player && (int)CA.ThisRow == RowValue) {
                    cardHand.RemoveCard(cardSelected);
                    CA.PlaceCardUp(cardSelected, gamemanager.CurrentCaseOn);
                }
            }
        }
        CheckEndTurn();
    }

    [Command]
    void CmdRemoveCard(int RowValue)
    {
        RpcRemoveCard(RowValue);
    }

    [ClientRpc]
    void RpcRemoveCard(int RowValue)
    {
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea CA in CardAreas)
        {
            if (GetComponentInParent<myOponnent>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Opponent && (int)CA.ThisRow == RowValue)
                {
                   ClueCard card = CA.GetCard(gamemanager.CurrentCaseOn);
                    CA.RemoveCard(card);
                    cardHand.AddCard(card, gamemanager.CurrentCaseOn - 1);
                }
            }
            else if (GetComponentInParent<myPlayer>())
            {
                if (CA.ThisCardAreaType == CardArea.CardAreaType.Player && (int)CA.ThisRow == RowValue)
                {
                    ClueCard card = CA.GetCard(gamemanager.CurrentCaseOn);
                    CA.RemoveCard(card);
                    cardHand.AddCard(card, gamemanager.CurrentCaseOn - 1);
                }
            }
        }
    }

    [Command]
    void CmdRemoveTile(int TileNumber)
    {
        RpcRemoveTile(TileNumber);
    }

    [ClientRpc]
    void RpcRemoveTile(int TileNumber)
    {
        tileArea.RemoveTile(TileNumber);
    }


    void CheckToPlaceDownTile(Transform HitTransform)
    {
        // check if the tile was recently placed. If so remove it
        if (HitTransform.GetComponent<TileSpot>().GetHighlighted)
        {
            PlayerType TileType = tileArea.GetTileType(HitTransform.GetComponent<TileSpot>().Number);
            CmdRemoveTile(HitTransform.GetComponent<TileSpot>().Number);

            if (TileType == PlayerType.Holmes)
            {
                for (int i = 0; i < HolmesCaseCardsWonThisTurn.Count; i++)
                {
                    if (HolmesCaseCardsWonThisTurn[i].CardTypes.Contains(HitTransform.GetComponent<TileSpot>().ThisCardType) && !HolmesCaseCardsWonThisTurn[i].GetMovedUp)
                    {
                        HolmesTilesToPlace++;
                        HolmesCaseCardsWonThisTurn[i].MoveUp(HolmesTilesToPlace);
                        HolmesCaseCardsWon.Add(HolmesCaseCardsWonThisTurn[i]);
                        break;
                    }
                }
            }
            else if (TileType == PlayerType.Moriarty) { MoriartyTilesToPlace++; }
            CheckEndTurn();
        }


        else if (MoriartyTilesToPlace > 0)
        {
            CmdPlacedMoriartyTile(HitTransform.GetComponent<TileSpot>().Number);
        }
        else if (HolmesTilesToPlace > 0)
        {
            for (int i = 0; i < HolmesCaseCardsWon.Count; i++)
            {
                if (HolmesCaseCardsWon[i].CardTypes.Contains(HitTransform.GetComponent<TileSpot>().ThisCardType))
                {
                    CmdPlacedHolmesTile(HitTransform.GetComponent<TileSpot>().Number, i);
                }
            }
        }
    }
    [Command]
    void CmdPlacedMoriartyTile(int TileNumber)
    {
        RpcPlacedMoriartyTile(TileNumber);
    }

    [ClientRpc]
    void RpcPlacedMoriartyTile(int TileNumber)
    {
        if (tileArea.PlaceTile(MoriartyTile, TileNumber, PlayerType.Moriarty)) {
            tileArea.HighlightTile(TileNumber);
            MoriartyTilesToPlace--;
        }
        if (MoriartyTilesToPlace == 0) { CheckEndTurn(); }
    }


    [Command]
    void CmdPlacedHolmesTile(int TileNumber, int CaseCardIndex)
    {
        RpcPlacedHolmesTile(TileNumber, CaseCardIndex);
    }

    [ClientRpc]
    void RpcPlacedHolmesTile(int TileNumber, int CaseCardIndex)
    {
        if (tileArea.PlaceTile(HolmesTile, TileNumber, PlayerType.Holmes))
        {
            tileArea.HighlightTile(TileNumber);
            HolmesTilesToPlace--;
            HolmesCaseCardsWon[CaseCardIndex].MoveBackDown();
            HolmesCaseCardsWon.Remove(HolmesCaseCardsWon[CaseCardIndex]);
        }

        if (HolmesTilesToPlace == 0)
        {
            CheckEndTurn();
        }
    }

    void CheckToSelectCard(Transform HitTransform)
    {
        ClueCard card = HitTransform.GetComponent<ClueCard>();
        if (SelectedCard != null)
        {
            UnselectCard();
        }
        // if in clue or crime area
        if (card.GetComponentInParent<CardArea>() != null && card.GetComponentInParent<CardArea>().ThisCardAreaType == CardArea.CardAreaType.Player)
        {
            if (card.GetComponentInParent<RowAreaPosition>().Case == gamemanager.CurrentCaseOn && !b_EnableSwapClueCards)
            {
                SelectCard(card);
            }
            else if ((card.GetComponentInParent<CardArea>().ThisRow == CardArea.Row.Clue && b_EnableSwapClueCards))
            {
                SelectCard(card);
            }
            return;
        }
        // if in hand
        else if (card.GetComponentInParent<M_CardHand>() && !b_EnableSwapClueCards)
        {
            SelectCard(card);
        }
    }

    void CheckForCardFollow()
    {
        if (SelectedCard != null)
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit))
            {
                Vector3 NewCardPosition = new Vector3(Hit.point.x, SelectedCardOriginalPosition.y + 2f, Hit.point.z);
                SelectedCard.transform.position = Vector3.Lerp(SelectedCard.transform.position, NewCardPosition, Time.deltaTime * 20);
            }
        }
    }

    void SelectCard(ClueCard card)
    {
       // card.SelectCard();
        SelectedCard = card;
        SelectedCardOriginalPosition = SelectedCard.transform.position;
    }

    void UnselectCard()
    {
        if (SelectedCard != null)
        {
            //SelectedCard.DeSelectCard();
            SelectedCard = null;
            SelectedCardOriginalPosition = Vector3.zero;
        }
    }

    public void SortCardsToggle()
    {
        CmdToggleSort();
    }

    [Command]
    void CmdToggleSort()
    {
        RpcToggleSort();
    }

    [ClientRpc]
    void RpcToggleSort()
    {
        Debug.Log("Sorting hand");
        cardHand.ToggleSort();
    }


}
