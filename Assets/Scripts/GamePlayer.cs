using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {

    // Use this for initialization
    public PlayerType MyPlayerType = PlayerType.Holmes;
    public List<CaseCard> HolmesCaseCardsWon;
    public List<ClueCard> GetCardsHolding()
    {
        return _CardHand.GetCardsHolding();
    }

    private ClueDeck _CardDeck;
    private CardHand _CardHand;

    public Transform CardSpot;

    void Awake () {
        

        _CardDeck = FindObjectOfType<ClueDeck>();
        _CardHand = GetComponentInChildren<CardHand>();
        SetupPlayer();
    }
	
    public virtual void SetupPlayer()
    {

    }

    public virtual void ResetPlayer()
    {
        _CardHand.PutCardsBack();
    }

    public virtual void EnableSwapClueCards()
    {

    }

    public virtual void DisableSwapClueCards()
    {

    }

    public virtual bool PlaceHolmesTiles(GameObject HolmesTilePrefab, CaseCard HolmesCaseCard)
    {
        return false;
    }

    public virtual bool PlaceMoriartyTiles(GameObject MoriartyTilePrefab, int number)
    {
        return false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Draw Cards
    public void DrawCards(int Number)
    {
        IEnumerator DrawCard = DrawingCards(Number);
        StartCoroutine(DrawCard);
    }

    IEnumerator DrawingCards(int Number)
    {
        Debug.Log("Drawing cards");
        List<ClueCard> cards = new List<ClueCard>();
        for (int i = 0; i < Number; i++)
        {
            //yield return new WaitForSeconds(.2f);
            //ClueCard cardDrawn = _CardDeck.DrawCard() as ClueCard;
            //_CardHand.AddCard(cardDrawn, 0);
            ClueCard cardDrawn = _CardDeck.DrawCard() as ClueCard;
            cards.Add(cardDrawn);
            if (transform.GetComponent<PlayerController>() != null) { cardDrawn.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0)); }
        }
        _CardHand.AddCards(cards, 0);
        yield return null;
    }

    public void RemoveAllCards()
    {
        _CardHand.RemoveAllCards();
    }

    public void AddNewCards(List<ClueCard> NewCards)
    {
        Debug.Log("Adding new cards");
        int StartingPosition = 0;
        if (NewCards.Count == 5) { StartingPosition = 1; }
        else if (NewCards.Count == 3) { StartingPosition = 2; }

        for (int i = 0; i < NewCards.Count; i++)
        {
            _CardHand.AddCard(NewCards[i], StartingPosition);
            NewCards[i].transform.localRotation = Quaternion.identity;
        }
    }


}
