﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_CaseArea : Photon.PunBehaviour
{

    // Use this for initialization
    public Transform[] Positions;
    public Transform[] HighPositions;
    M_CaseDeck caseDeck;

    public List<Card> Cards;

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
        Debug.Log("set case cards");
        caseDeck = FindObjectOfType<M_CaseDeck>();
        photonView.RPC("CmdPlaceCaseCards", PhotonTargets.MasterClient); 
    }

    [PunRPC]
    void CmdPlaceCaseCards()
    {
        Debug.Log("CMD seting case cards");
        for (int i = 0; i < Positions.Length; i++)
        {
            int cardDrawnIndex = caseDeck.SetCard();
            photonView.RPC("RpcPlaceCards", PhotonTargets.AllViaServer, i, cardDrawnIndex);
        }
    }

    [PunRPC]
    void RpcPlaceCards(int positionIndex, int CardIndex)
    {
        caseDeck = FindObjectOfType<M_CaseDeck>();
        Card CardDrawn = caseDeck.GetandRemoveCard(CardIndex);
        if (!Positions[positionIndex].GetComponentInChildren<CaseCard>())
        {
            GameObject card = Instantiate(CardDrawn.gameObject, Positions[positionIndex]);
            card.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
            card.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (positionIndex == 1) { FindObjectOfType<M_gameManager>().ShowInitialCaseCard(); }
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
