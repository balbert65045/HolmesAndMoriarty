using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public ClueDeck _CardDeck;
    public int HolmesTilesToPlace = 0;
    public LayerMask BoardLayer;
    public LayerMask Card_TileLayer;
    public List<CaseCard> HolmesCaseCardsWon;
    public PlayerType MyPlayerType;

    public bool ClueAreaActive = true;
    public bool CrimeAreaActive = true;

    CardHand cardHand;
    gameManager gamemanager;
    ClueCard SelectedCard;
    Vector3 SelectedCardOriginalPosition;
    TileArea tileArea;
    GameObject MoriartyTile;
    GameObject HolmesTile;
    CardArea ClueArea;
    CardArea CrimeArea;
    EndTurnButton endTurnButton;

    int MoriartyTilesToPlace = 0;

    bool EnableSwapClueCards = false;


    void Start () {
        ClueAreaActive = true;
        CrimeAreaActive = true;
         cardHand = GetComponentInChildren<CardHand>();
        tileArea = FindObjectOfType<TileArea>();
        gamemanager = FindObjectOfType<gameManager>();
        endTurnButton = FindObjectOfType<EndTurnButton>();

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


        if (FindObjectOfType<LevelPropertyManager>() != null)
        {
            MyPlayerType = FindObjectOfType<LevelPropertyManager>().GetPlayerType();
        }
    }



    public void PlayerEndTurn()
    {
        gamemanager.PlayerEndTurn(MyPlayerType);
        if (gamemanager.CurrentTurnStatus == gameManager.TurnStatus.SwitchClueCards){}
        else if (gamemanager.CurrentTurnStatus == gameManager.TurnStatus.BoardInspect) {}
        else if ((gamemanager.CurrentTurnStatus == gameManager.TurnStatus.PickTileHolmes || gamemanager.CurrentTurnStatus == gameManager.TurnStatus.PickTileMoriarty) &&
            MyPlayerType == PlayerType.Moriarty) {}
        else
        {
            endTurnButton.DisableEndTurn();
        }
    }

    public void PlayerEnableSwapClueCards()
    {
        EnableSwapClueCards = true;
    }

    public void PlayerDisableSwapClueCards()
    {
        EnableSwapClueCards = false;
    }

    public List<ClueCard> GetCardsHolding()
    {
        return cardHand.GetCardsHolding();
    }

    public void DrawCards(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            ClueCard cardDrawn = _CardDeck.DrawCard() as ClueCard;
            cardHand.AddCard(cardDrawn, 0);
        }
    }

    public void RemoveAllCards()
    {
        cardHand.RemoveAllCards();
    }

    public void AddNewCards(List<ClueCard> NewCards)
    {
        int StartingPosition = 0;
        if (NewCards.Count == 5) { StartingPosition = 1; }
        else if (NewCards.Count == 3) { StartingPosition = 2; }

        for (int i = 0; i < NewCards.Count; i++)
        {
            cardHand.AddCard(NewCards[i], StartingPosition);
        }
    }

    public void PlaceMoriartyTiles(int number, GameObject MoriartyTilePrefab)
    {
        TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
        List<TileSpot> OpenTileSpots = new List<TileSpot>();
        foreach (TileSpot TS in TilesSpots)
        {
            if (!TS.Used) { OpenTileSpots.Add(TS); }
        }
        if (OpenTileSpots.Count == 0) { return; }
        MoriartyTilesToPlace = number;
        MoriartyTile = MoriartyTilePrefab;
    }

    public bool PlaceHolmesTiles(GameObject HolmesTilePrefab, CaseCard HolmesCaseCard)
    {
        Debug.Log("Looking to add Holmes Tile");
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
        if (OpenTileSpots.Count == 0) { return false; }
        HolmesTilesToPlace ++;
        HolmesTile = HolmesTilePrefab;
        HolmesCaseCard.MoveUp(HolmesTilesToPlace);
        HolmesCaseCardsWon.Add(HolmesCaseCard);
        return true;
    }



	// Update is called once per frame
	void Update () {
        
        // On Mouse/Finger up 
        if (Input.GetMouseButtonUp(0))
        {
            Remove_Unselect_PlaceDownCard();
        }
        // On Mouse/finger pressed Down 
        else if (Input.GetMouseButtonDown(0))
        {
            SelectCard_PlaceTileDown();
            CheckActiveAreas();
        }

        // On Holding Down Mouse/Finger 
        else if (Input.GetMouseButton(0))
        {
            CheckForCardFollow();
        }
	}


    void CheckEndTurn()
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

    void CheckActiveAreas()
    {
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        CardArea CrimeCardArea = null;
        CardArea ClueCardArea = null;
        foreach (CardArea CA in CardAreas)
        {
            switch (CA.ThisRow)
            {
                case CardArea.Row.Crime:
                    CrimeCardArea = CA;
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
                    ClueCardArea = CA;
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
        //if (CrimeAreaActive) {
        //    CrimeCardArea.ActiveSpot(gamemanager.CurrentCaseOn);
        //    ClueCardArea.DeActiveSpot(gamemanager.CurrentCaseOn);
        //}
        //else if (ClueAreaActive) {
        //    ClueCardArea.ActiveSpot(gamemanager.CurrentCaseOn);
        //    CrimeCardArea.DeActiveSpot(gamemanager.CurrentCaseOn);
        //}
        //else
        //{
        //    ClueCardArea.DeActiveSpot(gamemanager.CurrentCaseOn);
        //    CrimeCardArea.DeActiveSpot(gamemanager.CurrentCaseOn);
        //}

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
            if (EnableSwapClueCards)
            {
                if (Physics.Raycast(ray, out Hit))
                {
                    if (Hit.transform.GetComponent<ClueCard>() && Hit.transform.GetComponentInParent<CardArea>() &&
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
                    if (Hit.transform.GetComponent<CardArea>())
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
        clueArea.MoveCard(cardSwapping);
        clueArea.MoveCard(SelectedCard);
        clueArea.PlaceCard(cardSwapping, SelectedCardSwappingCase);
        clueArea.PlaceCard(SelectedCard, cardSwappingCase);

    }

    void CheckToRemoveCardOrPlaceDown()
    {

        if (SelectedCard.GetComponentInParent<RowAreaPosition>() != null)
        {
            cardHand.AddCard(SelectedCard, gamemanager.CurrentCaseOn - 1);
            SelectedCard.transform.position = SelectedCardOriginalPosition;
            ClueCard card = SelectedCard;
            card.GetComponentInParent<CardArea>().RemoveCard(card);
            return;
        }
        else
        {
            CardArea[] CardAreas = FindObjectsOfType<CardArea>();
            foreach (CardArea CA in CardAreas)
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
        SelectedCard.transform.position = SelectedCardOriginalPosition;
    }

    void CheckForPlaceCard(Transform HitTransform)
    {
        
        if (HitTransform.GetComponent<CardArea>().CheckForAvailableSpace(gamemanager.CurrentCaseOn))
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
                HitTransform.GetComponent<CardArea>().PlaceCard(SelectedCard, gamemanager.CurrentCaseOn);
                FindObjectOfType<CardHand>().RemoveCard(SelectedCard);
            }
            return;  
        }
        SelectedCard.transform.position = SelectedCardOriginalPosition;
    }



    void CheckToPlaceDownTile(Transform HitTransform)
    {
        if (MoriartyTilesToPlace > 0)
        {
            if (tileArea.PlaceTile(MoriartyTile, HitTransform.GetComponent<TileSpot>().Number, PlayerType.Moriarty)) { MoriartyTilesToPlace--; }
            if (MoriartyTilesToPlace == 0) { CheckEndTurn(); }
        }
        else if (HolmesTilesToPlace > 0)
        {
            // TODO need to check if possiblt to place tile 
            for (int i = 0; i < HolmesCaseCardsWon.Count; i++)
            {
                if (HolmesCaseCardsWon[i].CardTypes.Contains(HitTransform.GetComponent<TileSpot>().ThisCardType))
                {
                    if (tileArea.PlaceTile(HolmesTile, HitTransform.GetComponent<TileSpot>().Number, PlayerType.Holmes))
                    {
                        HolmesTilesToPlace--;
                        HolmesCaseCardsWon[i].MoveBackDown();
                        HolmesCaseCardsWon.Remove(HolmesCaseCardsWon[i]);
                    }
                }
            }

            if (HolmesTilesToPlace == 0)
            {
                CheckEndTurn();
            }

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
        if (card.GetComponentInParent<CardArea>() != null)
        {
            if (card.GetComponentInParent<RowAreaPosition>().Case == gamemanager.CurrentCaseOn && !EnableSwapClueCards)
            {
                SelectCard(card);
            }
            else if ((card.GetComponentInParent<CardArea>().ThisRow == CardArea.Row.Clue && EnableSwapClueCards))
            {
                SelectCard(card);
            }
            return;
        }
        // if in hand
        else if (card.GetComponentInParent<CardHand>() && !EnableSwapClueCards)
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
        card.SelectCard();
        SelectedCard = card;
        SelectedCardOriginalPosition = SelectedCard.transform.position;
    }

    void UnselectCard()
    {
        if (SelectedCard != null)
        {
            SelectedCard.DeSelectCard();
            SelectedCard = null;
            SelectedCardOriginalPosition = Vector3.zero;
        }
    }
}
