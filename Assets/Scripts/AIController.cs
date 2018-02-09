using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    public List<Card> CardsHolding;
    gameManager gameManager;
    CardDeck cardDeck;
    AICardArea CrimeArea;
    AICardArea ClueArea;

    public PlayerType MyPlayerType;

    public void DrawCards(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            Card CardDrawn = cardDeck.DrawCard();
            CardsHolding.Add(CardDrawn);
        }
        PlayCards();
    }

    //Basic Random guess. Needs modify to play smart
    void PlayCards()
    {
        //Clue Card Place Down
        int RandomIndex = Random.Range(0, CardsHolding.Count);
        GameObject ClueCard = Instantiate(CardsHolding[RandomIndex].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex]);
        ClueArea.PlaceCard(ClueCard.GetComponent<Card>(), gameManager.CurrentCaseOn);

        // Crime Card Place Down
        int RandomIndex2 = Random.Range(0, CardsHolding.Count);
        GameObject CrimeCard = Instantiate(CardsHolding[RandomIndex2].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex2]);
        CrimeArea.PlaceCard(CrimeCard.GetComponent<Card>(), gameManager.CurrentCaseOn);


    }

    public void ResetCards()
    {
        RemoveAllCards();
    }

    public void RemoveAllCards()
    {
        CardsHolding.Clear();
    }

    public void AddNewCards(List<Card> Cards)
    {
        foreach (Card card in Cards)
        {
            CardsHolding.Add(card);
        }
        PlayCards();
    }

    // Use this for initialization
    void Start () {
        cardDeck = FindObjectOfType<CardDeck>();
        AICardArea[] CardAreas = FindObjectsOfType<AICardArea>();
        foreach (AICardArea carda in CardAreas)
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

        gameManager = FindObjectOfType<gameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
