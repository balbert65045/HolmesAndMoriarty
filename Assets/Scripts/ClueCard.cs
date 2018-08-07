using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueCard : Card {

    // Use this for initialization
    public int Number;
    public CardType ThisCardType;

    public GameObject SelectionOutline;
    public bool Used = false;

    public SpriteRenderer FrontSprite;
    public SpriteRenderer BackSprite;

    GameObject OldParent;

    bool CardFaded = false;

    void Start () {
        SelectionOutline.SetActive(false);

    }
    public override void MoveUp(int pos)
    {
        OldParent = transform.parent.gameObject;
        Transform MovePos;

        //TODO change how to find the area to put it
        if (FindObjectOfType<CaseArea>() == null) { MovePos = FindObjectOfType<M_CaseArea>().HighPositions[pos - 1]; }
        else { MovePos = FindObjectOfType<CaseArea>().HighPositions[pos - 1]; }

        transform.SetParent(MovePos);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(5, 7, .05f);

        if (BackSprite.enabled == false)
        {
            UnfadeCard();
            CardFaded = true;
        }
        // float newY = transform.position

    }

    public override void MoveBackDown()
    {
        Transform MovePos = OldParent.transform;
        transform.SetParent(MovePos);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(.7f, .246f, .07f);
        if (CardFaded) { FadeCard(); }
    }


    public void FadeCard()
    {
        BackSprite.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        FrontSprite.color = new Color(FrontSprite.color.r, FrontSprite.color.g, FrontSprite.color.b, .5f);
    }

    public void UnfadeCard()
    {
        BackSprite.enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        FrontSprite.color = new Color(FrontSprite.color.r, FrontSprite.color.g, FrontSprite.color.b, 1f);
    }

    public void SelectCard()
    {
       // Debug.Log("Card " + gameObject.name + " Selected");
        SelectionOutline.SetActive(true);
    }

    public void DeSelectCard()
    {
        SelectionOutline.SetActive(false);
    }

    public void PlacedDown()
    {
        Used = true;
        //GetComponent<BoxCollider>().enabled = false;
    }

}
