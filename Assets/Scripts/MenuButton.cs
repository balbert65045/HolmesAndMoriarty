using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    public GameObject MenuScreen;
    public GameObject HolmesScreen;
    public GameObject MoriartyScreen;
	// Use this for initialization
	void Start () {
        MenuScreen.SetActive(false);
      
    }
	
    public void EnableMenuScreen()
    {
        MenuScreen.SetActive(true);
        if (FindObjectOfType<PlayerController>() !=null)
        {
            if (FindObjectOfType<PlayerController>().MyPlayerType == PlayerType.Holmes) { HolmesScreen.SetActive(true); }
            else if (FindObjectOfType<PlayerController>().MyPlayerType == PlayerType.Moriarty) { MoriartyScreen.SetActive(true); }
        }
        else if (FindObjectOfType<myPlayer>() != null)
        {
            if (FindObjectOfType<myPlayer>().GetComponentInChildren<M_PlayerController>().MyPlayerType == PlayerType.Holmes) { HolmesScreen.SetActive(true); }
            else if (FindObjectOfType<myPlayer>().GetComponentInChildren<M_PlayerController>().MyPlayerType == PlayerType.Moriarty) { MoriartyScreen.SetActive(true); }
        }
    }

    public void DisableMenuScreen()
    {
        MenuScreen.SetActive(false);
        if (FindObjectOfType<PlayerController>() != null)
        {
            if (FindObjectOfType<PlayerController>().MyPlayerType == PlayerType.Holmes) { HolmesScreen.SetActive(false); }
            else if (FindObjectOfType<PlayerController>().MyPlayerType == PlayerType.Moriarty) { MoriartyScreen.SetActive(false); }
        }
        else if (FindObjectOfType<myPlayer>() != null)
        {
            if (FindObjectOfType<myPlayer>().GetComponentInChildren<M_PlayerController>().MyPlayerType == PlayerType.Holmes) { HolmesScreen.SetActive(false); }
            else if (FindObjectOfType<myPlayer>().GetComponentInChildren<M_PlayerController>().MyPlayerType == PlayerType.Moriarty) { MoriartyScreen.SetActive(false); }
        }
    }

}
