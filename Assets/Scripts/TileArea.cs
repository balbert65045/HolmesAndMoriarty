using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileArea : MonoBehaviour {

    // Use this for initialization
   // public List<TileSpot> OrderdTileSpots;
    public TileSpot[] TileSpots;

    public TileType[,] Tile2D = new TileType[4,4];

    PlayerIndicator HolmesIndicator = null;
    PlayerIndicator MoriartyIndicator = null;

    void Start () {
         TileSpots = GetComponentsInChildren<TileSpot>();
         QSort(TileSpots, 0, TileSpots.Length - 1);

        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                Tile2D[i, j] = TileType.None;
            }
        }

        PlayerIndicator[] PlayerIndicators = FindObjectsOfType<PlayerIndicator>();
        foreach (PlayerIndicator PI in PlayerIndicators)
        {
            if (PI.ThisIndicator == PlayerType.Holmes)
            {
                HolmesIndicator = PI;
            }
            else if (PI.ThisIndicator == PlayerType.Moriarty)
            {
                MoriartyIndicator = PI;
            }
        }

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

    public bool PlaceTile(GameObject tile, int Number, PlayerType PT)
    {
        if (!TileSpots[Number - 1].Used)
        {
            GameObject newTile = Instantiate(tile, new Vector3(TileSpots[Number - 1].transform.position.x, TileSpots[Number - 1].transform.position.y + .1f, TileSpots[Number - 1].transform.position.z), Quaternion.Euler(90, 0, 0));
            newTile.transform.SetParent(TileSpots[Number - 1].transform);
            newTile.transform.localScale = new Vector3(.7f, .7f, .7f);
            TileSpots[Number - 1].Used = true;

            

            if (PT == PlayerType.Holmes) { newTile.transform.position = HolmesIndicator.transform.position; }
            else if (PT == PlayerType.Moriarty) { newTile.transform.position = MoriartyIndicator.transform.position; }
            Vector3 MovePos = new Vector3(0, 0, -.3f);
            newTile.GetComponent<ScoreTile>().Move(MovePos);

            int HIndex = (Number - 1) % 4;
            int VIndex = (Number - 1) / 4;

            switch (PT)
            {
                case PlayerType.Holmes:
                    Tile2D[HIndex, VIndex] = TileType.Holmes;
                   
                    break;
                case PlayerType.Moriarty:
                    Tile2D[HIndex, VIndex] = TileType.Moriarty;
                    break;
            }
            return true;
        }
        return false;
    }

    public bool CheckForMoriartyWin()
    {
        if (CheckForHorizontalWin()) { return true; };
        if (CheckForVerticalWin()) { return true; };
        if (CheckForDiagonalWin()) { return true; };
        return false;
    }

    bool CheckForHorizontalWin()
    {
        for (int j = 0; j <= 3; j++)
        {
            int InARow = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (Tile2D[i,j] == TileType.Moriarty)
                {
                    InARow++;
                    if (InARow == 3) { return true; }
                }
                else
                {
                    InARow = 0; 
                }
            }
        }

        return false;
    }


    bool CheckForVerticalWin()
    {
        for (int i = 0; i <= 3; i++)
        {
            int InARow = 0;
            for (int j = 0; j <= 3; j++)
            {
                if (Tile2D[i, j] == TileType.Moriarty)
                {
                    InARow++;
                    if (InARow == 3) { return true; }
                }
                else
                {
                    InARow = 0;
                }
            }
        }

        return false;
    }

    bool CheckForDiagonalWin()
    {
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (CheckDiagnol(i,j) >= 3) { return true; }
            }
        }

        return false;
    }

    int CheckDiagnol(int i, int j)
    {
        int MaxValue = 0;
        if (Tile2D[i, j] == TileType.Moriarty)
        {
            int a = CheckUpLeft(i, j);
            if (a > MaxValue) { MaxValue = a; }

            int b = CheckBottomLeft(i, j);
            if (b > MaxValue) { MaxValue = b; }

            int c = CheckUpRight(i, j);
            if (c > MaxValue) { MaxValue = c; }

            int d = CheckBottomRight(i, j);
            if (d > MaxValue) { MaxValue = d; }
            MaxValue++;
        }

        return MaxValue;
    }

    int CheckUpLeft(int i, int j)
    {
        //UpLeft
        if (i > 0 && j > 0)
        {
            if (Tile2D[i - 1, j - 1] == TileType.Moriarty) { return (1 + CheckUpLeft(i - 1, j - 1)); }
        }
        return 0; 
    }

    int CheckBottomLeft(int i, int j)
    {
        //BottomLeft
        if (i > 0 && j < 3)
        {
            if (Tile2D[i - 1, j + 1] == TileType.Moriarty) { return (1 + CheckBottomLeft(i - 1, j + 1)); }
        }
        return 0;
    }

    int CheckUpRight(int i, int j)
    {
        //UpRight
        if (i < 3 && j > 0)
        {
            if (Tile2D[i + 1, j - 1] == TileType.Moriarty) { return (1 + CheckUpRight(i + 1, j - 1)); }
        }
        return 0;
    }

    int CheckBottomRight(int i, int j)
    {
        //BottomRight
        if (i < 3 && j < 3)
        {
            if (Tile2D[i + 1, j + 1] == TileType.Moriarty) { return (1 + CheckBottomRight(i + 1, j + 1)); }
        }

        return 0;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
