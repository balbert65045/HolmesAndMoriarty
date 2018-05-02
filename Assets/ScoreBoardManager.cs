using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour {

    // Use this for initialization
    LevelPropertyManager _levelPropertyManager;
    public TileArea tileArea;


	void Start () {
        _levelPropertyManager = FindObjectOfType<LevelPropertyManager>();
        if (_levelPropertyManager == null) { Debug.LogWarning("No level property manager in scene"); }
        tileArea.CopyTileArea(_levelPropertyManager.Tile2DSaved);


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
