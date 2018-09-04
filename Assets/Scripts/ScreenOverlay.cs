using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOverlay : MonoBehaviour {

    public GameObject Overlay;

    public void EnableOverlay()
    {
        Overlay.SetActive(true);
    }

    public void DisableOverlay()
    {
        Overlay.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
