using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    CardHand cardHand;
    public ClueDeck _CardDeck;
    gameManager gamemanager;
    ClueCard SelectedCard;
    TileArea tileArea;
     GameObject MoriartyTile;

    int MoriartyTilesToPlace = 0;

    public PlayerType MyPlayerType;

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
        MoriartyTilesToPlace = number;
        MoriartyTile = MoriartyTilePrefab;
    }



	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyDown(KeyCode.Space))
  //      {
  //          cardHand.DrawNewCards(7);
  //      }

        // When mouse id down look for something to select. TODO change this to touch input
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit))
            {
                //Select Card
                if (Hit.transform.GetComponent<ClueCard>())
                {
                    if (SelectedCard != null)
                    {
                        UnselectCard();
                    }
                    Hit.transform.GetComponent<ClueCard>().SelectCard();
                    SelectedCard = Hit.transform.GetComponent<ClueCard>();
                }

                //Place Card Down
                else if (Hit.transform.GetComponent<CardArea>())
                {
                    if (SelectedCard != null)
                    {
                        if (Hit.transform.GetComponent<CardArea>().CheckForAvailableSpace(gamemanager.CurrentCaseOn))
                        {
                            Hit.transform.GetComponent<CardArea>().PlaceCard(SelectedCard, gamemanager.CurrentCaseOn);
                            SelectedCard.PlacedDown();
                            FindObjectOfType<CardHand>().RemoveCard(SelectedCard);
                            gamemanager.CheckEndTurn();
                        }
                        UnselectCard();
                    }
                }

                else if (Hit.transform.GetComponent<TileSpot>())
                {
                    if (MoriartyTilesToPlace > 0)
                    {
                        if (tileArea.PlaceTile(MoriartyTile, Hit.transform.GetComponent<TileSpot>().Number, PlayerType.Moriarty)) { MoriartyTilesToPlace--; }
                        if (MoriartyTilesToPlace == 0) { gamemanager.CheckEndTurn(); }
                    }
                }

                //Unselect
                else
                {
                    if (SelectedCard != null)
                    {
                        UnselectCard();
                    }
                }
            }
        }

	}

    void UnselectCard()
    {
        SelectedCard.DeSelectCard();
        SelectedCard = null;
    }
}
