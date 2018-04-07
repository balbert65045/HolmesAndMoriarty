using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortButton : MonoBehaviour {

    public GameObject NumberSortIndicator;
    public GameObject ColorSortIndicator;

	// Use this for initialization
	void Start () {
        NumberSortIndicator.SetActive(false);
        ColorSortIndicator.SetActive(true);
    }
	
	public void Toggle()
    {
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
