using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileArea : MonoBehaviour {

    // Use this for initialization
   // public List<TileSpot> OrderdTileSpots;
    public TileSpot[] TileSpots;


    void Start () {
         TileSpots = GetComponentsInChildren<TileSpot>();
         QSort(TileSpots, 0, TileSpots.Length - 1);

    }

    void QSort(TileSpot[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = partition(TileSpots, low, high);

            QSort(TileSpots, low, pi - 1);
            QSort(TileSpots, pi + 1, high);
        }
    }



    int partition(TileSpot[] arr, int low, int high)
    {
        int pivot = arr[high].Number;
        int i = low - 1;
        for (int j = low; j < (high); j++)
        {
            if (arr[j].Number <= pivot)
            {
                i++;
                // Swap
                Swap(arr, j, i);
            }
        }
        Swap(TileSpots, i + 1, high);
        return (i + 1);

    }

    void Swap(TileSpot[] arr, int index1, int index2)
    {
        TileSpot temp = arr[index1];
        arr[index1] = arr[index2];
        arr[index2] = temp;
    }

    public void PlaceTile(GameObject tile, int Number)
    {
        GameObject newTile = Instantiate(tile, new Vector3(TileSpots[Number - 1].transform.position.x, TileSpots[Number - 1].transform.position.y + .1f, TileSpots[Number - 1].transform.position.z), Quaternion.Euler(90,0,0));
        newTile.transform.SetParent(TileSpots[Number - 1].transform);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
