using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_CaseArea : NetworkBehaviour {

    // Use this for initialization
    public Transform[] Positions;
    public Transform[] HighPositions;
    M_CaseDeck caseDeck;

    void Start () {
        caseDeck = FindObjectOfType<M_CaseDeck>();
    }


    public void ClearCards()
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            if (Positions[i].GetComponentInChildren<CaseCard>())
            {
                Destroy(Positions[i].GetComponentInChildren<CaseCard>().gameObject);
            }
        }
    }

    public void PlaceCards()
    {
        caseDeck = FindObjectOfType<M_CaseDeck>();
       if (isServer) { CmdPlaceCaseCards(); }
    }

    [Command]
    void CmdPlaceCaseCards()
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            int cardDrawnIndex = caseDeck.SetCard();
            RpcPlaceCards(i, cardDrawnIndex);
        }
    }

    [ClientRpc]
    void RpcPlaceCards(int positionIndex, int CardIndex)
    {
        caseDeck = FindObjectOfType<M_CaseDeck>();
        Card CardDrawn = caseDeck.GetandRemoveCard(CardIndex);
        if (!Positions[positionIndex].GetComponentInChildren<CaseCard>())
        {
            GameObject card = Instantiate(CardDrawn.gameObject, Positions[positionIndex]);
            card.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public void FlipCard(int CaseNumber)
    {

        Transform CardTransform = Positions[CaseNumber - 1].GetComponentInChildren<CaseCard>().transform;
        CardTransform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
    }

    public CaseCard FindCaseCard(int CaseNumber)
    {
        return (Positions[CaseNumber - 1].GetComponentInChildren<CaseCard>());
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
