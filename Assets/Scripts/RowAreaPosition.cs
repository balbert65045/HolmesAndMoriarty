using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowAreaPosition : MonoBehaviour {

    public int Case;

    public bool InUse = false;

    GameObject ActiveImage;

	// Use this for initialization
	void Start () {
        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            ActiveImage = GetComponentInChildren<SpriteRenderer>().gameObject;
            ActiveImage.SetActive(false);
        }
    }
	
	public void SetActiveImage(bool value)
    {
        ActiveImage.SetActive(value);
    }
}
