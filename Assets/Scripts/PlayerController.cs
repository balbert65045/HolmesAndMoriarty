using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    CardHand cardHand;
    gameManager gamemanager;
    Card SelectedCard;

    public PlayerType MyPlayerType;

	void Start () {
        cardHand = FindObjectOfType<CardHand>();
        gamemanager = FindObjectOfType<gameManager>();
    }



    public void DrawCards(int Number)
    {
        cardHand.DrawNewCards(Number);
    }

    public void ResetCards()
    {
        cardHand.ResetCards();
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
                if (Hit.transform.GetComponent<Card>())
                {
                    Hit.transform.GetComponent<Card>().SelectCard();
                    SelectedCard = Hit.transform.GetComponent<Card>();
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
