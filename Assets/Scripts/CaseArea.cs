using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseArea : MonoBehaviour {

    // Use this for initialization
    public Transform[] Positions;
    public Transform[] HighPositions;
    CaseDeck caseDeck;

    public List<Card> Cards;

    void Start () {
        caseDeck = FindObjectOfType<CaseDeck>();
    }


    public void ClearCards()
    {
        foreach (Card card in Cards)
        {
            card.transform.position = new Vector3(100, 100, 100);
            card.transform.SetParent(caseDeck.transform);
        }
        Cards.Clear();


        //for (int i = 0; i < Positions.Length; i++)
        //{
        //    if (Positions[i].GetComponentInChildren<CaseCard>())
        //    {
        //        Positions[i].GetComponentInChildren<CaseCard>().transform.position = new Vector3(100, 100, 100);
        //        Positions[i].GetComponentInChildren<CaseCard>().transform.SetParent(caseDeck.transform);
        //    }
        //}
    }

    public void PlaceCards()
    {
        caseDeck = FindObjectOfType<CaseDeck>();
        for (int i = 0; i < Positions.Length; i++)
        {
            Card cardDrawn = caseDeck.DrawCard();
            if (!Positions[i].GetComponentInChildren<CaseCard>())
            {
                Cards.Add(cardDrawn);
                Debug.Log("setting card down");
                cardDrawn.transform.SetParent(Positions[i]);
                cardDrawn.transform.localPosition = new Vector3(0, 0, 0);
                cardDrawn.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
            }
        }
    }

    public void FlipCard(int CaseNumber)
    {

       // Transform CardTransform = Positions[CaseNumber - 1].GetComponentInChildren<CaseCard>().transform;
      //  CardTransform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
    }

    public CaseCard FindCaseCard(int CaseNumber)
    {
        //return (Positions[CaseNumber - 1].GetComponentInChildren<CaseCard>());
        return ((CaseCard)Cards[CaseNumber - 1]);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
