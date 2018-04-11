using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Use this for initialization
    public PlayerType MyPlayerType = PlayerType.Holmes;
    List<ClueCard> CardsHolding;
    public List<ClueCard> GetCardsHolding() { return CardsHolding;}
    public int HolmesTilesToPlace = 0;
    public int MoriartyTilesToPlace = 0;
    public List<CaseCard> HolmesCaseCardsWon;

    GameObject MoriartyTile;
    GameObject HolmesTile;
    private ClueDeck _CardDeck;
    private CardHand _CardHand;
    bool b_EnableSwapClueCards = false;

    void Start () {
        _CardDeck = FindObjectOfType<ClueDeck>();
        _CardHand = GetComponentInChildren<CardHand>();

        if (FindObjectOfType<LevelPropertyManager>() != null)
        {
            MyPlayerType = FindObjectOfType<LevelPropertyManager>().GetPlayerType();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Draw Cards
    public void DrawCards(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            ClueCard cardDrawn = _CardDeck.DrawCard() as ClueCard;
            _CardHand.AddCard(cardDrawn, 0);
        }
    }

    public void RemoveAllCards()
    {
        _CardHand.RemoveAllCards();
    }

    public void AddNewCards(List<ClueCard> NewCards)
    {
        int StartingPosition = 0;
        if (NewCards.Count == 5) { StartingPosition = 1; }
        else if (NewCards.Count == 3) { StartingPosition = 2; }

        for (int i = 0; i < NewCards.Count; i++)
        {
            _CardHand.AddCard(NewCards[i], StartingPosition);
        }
    }

   public void EnableSwapClueCards()
    {
        b_EnableSwapClueCards = true;
    }

    public void DisableSwapClueCards()
    {
        b_EnableSwapClueCards = false;
    }

    public bool PlaceHolmesTiles(GameObject HolmesTilePrefab, CaseCard HolmesCaseCard)
    {
        if (GetComponent<PlayerController>() != null) { return GetComponent<PlayerController>().PlaceHolmesTiles(HolmesTilePrefab, HolmesCaseCard); }
        else if (GetComponent<AIController>() != null) { return GetComponent<AIController>().PlaceHolmesTile(HolmesTilePrefab, HolmesCaseCard); }
        return false;
    }

    public bool PlaceMoriartyTiles(GameObject MoriartyTilePrefab, int number)
    {
        if (GetComponent<PlayerController>() != null) { return GetComponent<PlayerController>().PlaceMoriartyTiles(MoriartyTilePrefab, number); }
        else if (GetComponent<AIController>() != null) { return GetComponent<AIController>().PlaceMoriartyTile(MoriartyTilePrefab, number); }
        return false;
    }


}
