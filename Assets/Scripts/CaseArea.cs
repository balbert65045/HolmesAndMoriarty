using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseArea : MonoBehaviour {

    // Use this for initialization
    public Transform[] Positions;
    public Transform[] HighPositions;
    CaseDeck caseDeck;

    void Start () {
        caseDeck = FindObjectOfType<CaseDeck>();
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
        caseDeck = FindObjectOfType<CaseDeck>();
        for (int i = 0; i < Positions.Length; i++)
        {
            Card cardDrawn = caseDeck.DrawCard();
            if (!Positions[i].GetComponentInChildren<CaseCard>())
            {
                GameObject card = Instantiate(cardDrawn.gameObject, Positions[i]);
                card.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
            }
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
