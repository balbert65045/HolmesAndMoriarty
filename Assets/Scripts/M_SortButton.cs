using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_SortButton : NetworkBehaviour {

    public GameObject NumberSortIndicator;
    public GameObject ColorSortIndicator;

	// Use this for initialization
	void Start () {
        NumberSortIndicator.SetActive(false);
        ColorSortIndicator.SetActive(true);
        transform.localPosition = new Vector3(0, 0, 0);
      //  GetComponent<RectTransform>().position = new Vector3(-40, 26.3f, 0);
       // GetComponent<RectTransform>().localPosition = new Vector3(-40, 26.3f, 0);
    }
	
	public void Toggle()
    {
        myPlayer player = FindObjectOfType<myPlayer>();
        Debug.Log(player);
        M_PlayerController Controller = player.GetComponentInChildren<M_PlayerController>();
        Debug.Log(Controller);
        M_CardHand hand = Controller.GetComponentInChildren<M_CardHand>();
        Debug.Log(hand);
        Controller.SortCardsToggle();
        if (ColorSortIndicator.activeSelf)
        {
            NumberSortIndicator.SetActive(true);
            ColorSortIndicator.SetActive(false);
        }
        else
        {
            NumberSortIndicator.SetActive(false);
            ColorSortIndicator.SetActive(true);
        }
    }
}
