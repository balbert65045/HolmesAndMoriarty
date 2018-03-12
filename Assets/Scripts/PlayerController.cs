using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    CardHand cardHand;
    public ClueDeck _CardDeck;
    gameManager gamemanager;
    ClueCard SelectedCard;

    public PlayerType MyPlayerType;

	void Start () {
        cardHand = GetComponentInChildren<CardHand>();
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
            Debug.Log("Attempting to switch new cards");
            cardHand.AddCard(NewCards[i], StartingPosition);
        }
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
                        Debug.Log(gamemanager.CurrentCaseOn);
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
