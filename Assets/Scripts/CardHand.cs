using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    // Use this for initialization
    public Transform[] CardPositions;
    private List<ClueCard> CardsHolding = new List<ClueCard>();
    public List<ClueCard> GetCardsHolding()
    {
        return CardsHolding;
    }

	void Start () {

    }
	
    public void AddCard(ClueCard card, int StartingPosition)
    {
        for (int i = StartingPosition; i < CardPositions.Length; i++)
        {
            if (CardPositions[i].GetComponentInChildren<ClueCard>() == null)
            {
                ClueCard clueCard = FindObjectOfType<ClueDeck>().FindCardInDeck(card);
                CardsHolding.Add(clueCard);
                GameObject Card = Instantiate(card.gameObject, CardPositions[i]);
                Card.transform.localPosition = Vector3.zero;
                Card.transform.localScale = new Vector3(5, 7, .05f);
                return;
            }
        }
        Debug.LogWarning("All Card Positions full and cannot add card");
    }


    public void RemoveCard(ClueCard card)
    {
        foreach (ClueCard HCard in CardsHolding)
        {
            if (HCard.Number == card.Number && HCard.ThisCardType == card.ThisCardType)
            {
                CardsHolding.Remove(HCard);
                foreach (Transform position in CardPositions)
                {
                    if (position.GetComponentInChildren<ClueCard>())
                    {
                        if (position.GetComponentInChildren<ClueCard>().Number == card.Number)
                        {
                            Destroy(position.GetComponentInChildren<ClueCard>().gameObject);
                        }
                    }
                }
                return;
            }
        }
    }

    public void RemoveAllCards()
    {

        foreach (Transform position in CardPositions)
        {
            if (position.GetComponentInChildren<ClueCard>())
            {
                Destroy((position.GetComponentInChildren<ClueCard>().gameObject));
            }
        }
        CardsHolding.Clear();
    }


}
