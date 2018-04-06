using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    // Use this for initialization
    public Transform[] CardPositions;
    public enum SortMethod { NumberSort, ColorSort}
    public SortMethod CurrentSortMethod = SortMethod.NumberSort;

    private List<ClueCard> CardsHolding = new List<ClueCard>();
    public List<ClueCard> GetCardsHolding()
    {
        return CardsHolding;
    }

	void Start () {

    }
	
    public void ToggleSort()
    {
        switch (CurrentSortMethod)
        {
            case SortMethod.NumberSort:
                CurrentSortMethod = SortMethod.ColorSort;
                break;
            case SortMethod.ColorSort:
                CurrentSortMethod = SortMethod.NumberSort;
                break;
        }
        Sort(FindObjectOfType<gameManager>().CurrentCaseOn - 1);
    }

    private void Sort(int StartingPosition)
    {
        switch (CurrentSortMethod)
        {
            case SortMethod.NumberSort:
                SortCardsNumber(StartingPosition);
                break;
            case SortMethod.ColorSort:
                SortCardColor(StartingPosition);
                break;
        }
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
                Sort(StartingPosition);
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

    public void SortCardsNumber(int StartingPosition)
    {
        QuickSort QSort = new QuickSort();
        List<int> CardNumbers = new List<int>();
        List<ClueCard> Cards = new List<ClueCard>();
        foreach (Transform T in CardPositions)
        {
            if (T.GetComponentInChildren<ClueCard>() != null)
            {
                CardNumbers.Add(T.GetComponentInChildren<ClueCard>().Number);
                Cards.Add(T.GetComponentInChildren<ClueCard>());
            }
        }

        int[] ArrCardNumbers = CardNumbers.ToArray();
        int[] OrderdCardNumbers = QSort.Sort(ArrCardNumbers);
        for (int i = 0; i < OrderdCardNumbers.Length; i++)
        {
            foreach (ClueCard card in Cards)
            {
                if (card.Number == OrderdCardNumbers[i])
                {
                    card.transform.parent = CardPositions[i + StartingPosition];
                    card.transform.localPosition = Vector3.zero;
                    card.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    card.transform.localScale = new Vector3(5, 7, .05f);
                    break;
                }
            }
        }
    }

    public void SortCardColor(int StartingPosition)
    {
        QuickSort QSort = new QuickSort();
        List<CardType> CardColors = new List<CardType>();
        List<ClueCard> Cards = new List<ClueCard>();
        foreach (Transform T in CardPositions)
        {
            if (T.GetComponentInChildren<ClueCard>() != null)
            {
                CardColors.Add(T.GetComponentInChildren<ClueCard>().ThisCardType);
                Cards.Add(T.GetComponentInChildren<ClueCard>());
            }
        }

        int index = 0;
        foreach (CardType cardType in CardType.GetValues(typeof(CardType)))
        {
            List<ClueCard> ColorCard = new List<ClueCard>();
            List<int> ColorCardNumbers = new List<int>();
            foreach (ClueCard card in Cards)
            {
                if (card.ThisCardType == cardType)
                {
                    ColorCard.Add(card);
                    ColorCardNumbers.Add(card.Number);
                }
            }
            int[] ArrCardNumbers = ColorCardNumbers.ToArray();
            int[] OrderdCardNumbers = QSort.Sort(ArrCardNumbers);

            for (int i = 0; i < OrderdCardNumbers.Length; i++)
            {
                foreach (ClueCard card in ColorCard)
                {
                    if (card.Number == OrderdCardNumbers[i])
                    {
                        card.transform.parent = CardPositions[i + StartingPosition + index];
                        card.transform.localPosition = Vector3.zero;
                        card.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        card.transform.localScale = new Vector3(5, 7, .05f);
                        break;
                    }
                }
            }
            index = OrderdCardNumbers.Length + index;

        }


    }

}
