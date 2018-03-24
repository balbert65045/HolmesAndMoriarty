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


	void Start () {
        cardHand = GetComponentInChildren<CardHand>();
        tileArea = FindObjectOfType<TileArea>();
        //_CardDeck = FindObjectOfType<CardDeck>();
        gamemanager = FindObjectOfType<gameManager>();
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
        

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit, 50f, BoardLayer))
            {
                if (Hit.transform.GetComponent<CardArea>())
                {
                    CheckForPlaceCard(Hit.transform);
                }
                else
                {
                    if (SelectedCard != null)
                    {
                        SelectedCard.transform.position = SelectedCardOriginalPosition;
                        UnselectCard();
                    }
                }
            }
        }
        // When mouse id down look for something to select. TODO change this to touch input
        else if (Input.GetMouseButtonDown(0))
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
        else if (Input.GetMouseButton(0))
        {
            CheckForCardFollow();
        }
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
        if (SelectedCard != null)
        {
            UnselectCard();
        }

        HitTransform.GetComponent<ClueCard>().SelectCard();
        SelectedCard = HitTransform.GetComponent<ClueCard>();
        SelectedCardOriginalPosition = SelectedCard.transform.position;
    }

    void CheckForCardFollow()
    {
        if (SelectedCard != null)
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit))
            {
                Vector3 NewCardPosition = new Vector3(Hit.point.x, SelectedCard.transform.position.y, Hit.point.z);
                SelectedCard.transform.position = Vector3.Lerp(SelectedCard.transform.position, NewCardPosition, Time.deltaTime * 20);
            }
        }
    }

    void CheckForPlaceCard(Transform HitTransform)
    {
        if (SelectedCard != null)
        {
            if (HitTransform.GetComponent<CardArea>().CheckForAvailableSpace(gamemanager.CurrentCaseOn))
            {
                HitTransform.GetComponent<CardArea>().PlaceCard(SelectedCard, gamemanager.CurrentCaseOn);
                SelectedCard.PlacedDown();
                FindObjectOfType<CardHand>().RemoveCard(SelectedCard);
                gamemanager.CheckEndTurn();
            }
            else
            {
                SelectedCard.transform.position = SelectedCardOriginalPosition;
            }
            UnselectCard();
        }
    }


    void UnselectCard()
    {
        SelectedCard.DeSelectCard();
        SelectedCard = null;
        SelectedCardOriginalPosition = Vector3.zero;
    }
}
