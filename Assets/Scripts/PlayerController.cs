using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public ClueDeck _CardDeck;
    public int HolmesTilesToPlace = 0;
    public LayerMask BoardLayer;
    public List<CaseCard> HolmesCaseCardsWon;
    public PlayerType MyPlayerType;

    CardHand cardHand;
    gameManager gamemanager;
    ClueCard SelectedCard;
    Vector3 SelectedCardOriginalPosition;
    TileArea tileArea;
    GameObject MoriartyTile;
    GameObject HolmesTile;

    int MoriartyTilesToPlace = 0;

    bool EnableSwapClueCards = false;


    void Start () {
        cardHand = GetComponentInChildren<CardHand>();
        tileArea = FindObjectOfType<TileArea>();
        gamemanager = FindObjectOfType<gameManager>();
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
        }

        // On Holding Down Mouse/Finger 
        else if (Input.GetMouseButton(0))
        {
            CheckForCardFollow();
        }
	}


    void SelectCard_PlaceTileDown()
    {
        RaycastHit Hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Hit))
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
                        CheckForPlaceCard(Hit.transform);
                    }
                    // check to remove card
                    else
                    {
                        CheckToRemoveCard();
                    }
                    gamemanager.CheckEndTurn();
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

    void CheckToRemoveCard()
    {

        if (SelectedCard.GetComponentInParent<RowAreaPosition>() != null)
        {
            cardHand.AddCard(SelectedCard, gamemanager.CurrentCaseOn - 1);
            SelectedCard.transform.position = SelectedCardOriginalPosition;
            ClueCard card = SelectedCard;
            card.GetComponentInParent<CardArea>().RemoveCard(card);
            return;
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
            if (MoriartyTilesToPlace == 0) { gamemanager.CheckEndTurn(); }
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
                gamemanager.CheckEndTurn();
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
