using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_Player : NetworkBehaviour {

    // Use this for initialization
    public PlayerType MyPlayerType = PlayerType.Holmes;
    public List<CaseCard> HolmesCaseCardsWon;
    public List<ClueCard> GetCardsHolding()
    {
        return _CardHand.GetCardsHolding();
    }

    private M_ClueDeck _CardDeck;
    private M_CardHand _CardHand;

    public Transform CardSpot;

    public bool isTheLocalPlayer = false;

    void Awake () {
        

        _CardDeck = FindObjectOfType<M_ClueDeck>();
        _CardHand = GetComponentInChildren<M_CardHand>();
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
        _CardDeck = FindObjectOfType<M_ClueDeck>();
        if (!isTheLocalPlayer) { return; }
        IEnumerator DrawCard = DrawingCards(Number);
        StartCoroutine(DrawCard);
    }

    IEnumerator DrawingCards(int Number)
    {
        CmdDrawCard(Number);
        yield return null;
    }

    [Command]
    void CmdDrawCard(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            int CardIndex =_CardDeck.SetCard();
            RpcDrawCard(CardIndex);
        }
    }

    [ClientRpc]
    void RpcDrawCard(int CardIndex)
    {
        _CardDeck = FindObjectOfType<M_ClueDeck>();
        ClueCard cardDrawn = _CardDeck.GetandRemoveCard(CardIndex) as ClueCard;
        if (transform.GetComponentInParent<myPlayer>() != null) { cardDrawn.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0)); }
        _CardHand.AddCard(cardDrawn, 0);
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
            NewCards[i].transform.localRotation = Quaternion.identity;
        }
    }


}
