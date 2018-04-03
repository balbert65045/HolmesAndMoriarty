using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    public GameObject MenuScreen;
	// Use this for initialization
	void Start () {
        MenuScreen.SetActive(false);

    }
	
    public void EnableMenuScreen()
    {
        MenuScreen.SetActive(true);
    }

    public void DisableMenuScreen()
    {
        MenuScreen.SetActive(false);
    }

}
