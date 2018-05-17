using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileArea : MonoBehaviour {

    // Use this for initialization
   // public List<TileSpot> OrderdTileSpots;
    public TileSpot[] TileSpots;

    public TileType[,] Tile2D = new TileType[4,4];

    public GameObject HolmesTile;
    public GameObject MoriartyTile;

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

    public bool CheckForMoriartyWinWithTile(int number)
    {
        int HIndex = (number - 1) % 4;
        int VIndex = (number - 1) / 4;
        Tile2D[HIndex, VIndex] = TileType.Moriarty;

        if (CheckForHorizontalWin()) {
            Tile2D[HIndex, VIndex] = TileType.None;
            return true; };
        if (CheckForVerticalWin()) {
            Tile2D[HIndex, VIndex] = TileType.None;
            return true; };
        if (CheckForDiagonalWin()) {
            Tile2D[HIndex, VIndex] = TileType.None;
            return true; };
        Tile2D[HIndex, VIndex] = TileType.None;
        return false;
    }


    public bool CheckForMoriartyWin()
    {
        if (CheckForHorizontalWin()) { return true; };
        if (CheckForVerticalWin()) { return true; };
        if (CheckForDiagonalWin()) { return true; };
        return false;
    }

    public bool CheckForHolmesWin()
    {
        if (!CheckForHorizontalPossible() && !CheckForVerticalPossible() && !CheckForDiagonalPossible())
        {
            return true;
        }
        return false;
    }

    bool CheckForHorizontalPossible()
    {
        for (int j = 0; j <= 3; j++)
        {
            int InARow = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (Tile2D[i, j] == TileType.Moriarty || Tile2D[i, j] == TileType.None)
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

    bool CheckForVerticalPossible()
    {
        for (int i = 0; i <= 3; i++)
        {
            int InARow = 0;
            for (int j = 0; j <= 3; j++)
            {
                if (Tile2D[i, j] == TileType.Moriarty || Tile2D[i, j] == TileType.None)
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

    bool CheckForDiagonalPossible()
    {
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (CheckDiagnolP(i, j) >= 3) { return true; }
            }
        }

        return false;
    }

    int CheckDiagnolP(int i, int j)
    {
        int MaxValue = 0;
        if (Tile2D[i, j] == TileType.Moriarty || Tile2D[i, j] == TileType.None)
        {
            int a = CheckUpLeftP(i, j);
            if (a > MaxValue) { MaxValue = a; }

            int b = CheckBottomLeftP(i, j);
            if (b > MaxValue) { MaxValue = b; }

            int c = CheckUpRightP(i, j);
            if (c > MaxValue) { MaxValue = c; }

            int d = CheckBottomRightP(i, j);
            if (d > MaxValue) { MaxValue = d; }
            MaxValue++;
        }

        return MaxValue;
    }

    int CheckUpLeftP(int i, int j)
    {
        //UpLeft
        if (i > 0 && j > 0)
        {
            if (Tile2D[i - 1, j - 1] == TileType.Moriarty || Tile2D[i - 1, j - 1] == TileType.None) { return (1 + CheckUpLeftP(i - 1, j - 1)); }
        }
        return 0;
    }

    int CheckBottomLeftP(int i, int j)
    {
        //BottomLeft
        if (i > 0 && j < 3)
        {
            if (Tile2D[i - 1, j + 1] == TileType.Moriarty || Tile2D[i - 1, j + 1] == TileType.None) { return (1 + CheckBottomLeftP(i - 1, j + 1)); }
        }
        return 0;
    }

    int CheckUpRightP(int i, int j)
    {
        //UpRight
        if (i < 3 && j > 0)
        {
            if (Tile2D[i + 1, j - 1] == TileType.Moriarty || Tile2D[i + 1, j - 1] == TileType.None) { return (1 + CheckUpRightP(i + 1, j - 1)); }
        }
        return 0;
    }

    int CheckBottomRightP(int i, int j)
    {
        //BottomRight
        if (i < 3 && j < 3)
        {
            if (Tile2D[i + 1, j + 1] == TileType.Moriarty || Tile2D[i + 1, j + 1] == TileType.None) { return (1 + CheckBottomRightP(i + 1, j + 1)); }
        }

        return 0;
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

    public void CopyTileArea(TileType[,] NewTile2D)
    {
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (NewTile2D[i, j] == TileType.Holmes)
                {
                    GameObject Tile = Instantiate(HolmesTile);
                    Tile.transform.position = TileSpots[i + 4 * j].transform.position;
                    Tile.transform.SetParent(TileSpots[i + 4 * j].transform);
                }
                else if (NewTile2D[i, j] == TileType.Moriarty)
                {
                    GameObject Tile = Instantiate(MoriartyTile);
                    Tile.transform.position = TileSpots[i + 4 * j].transform.position;
                    Tile.transform.SetParent(TileSpots[i + 4 * j].transform);
                }
            }
        }
    }

    public bool CheckForOpenTiles()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j< 4; j++)
            {
                if (Tile2D[i,j] == TileType.None) { return true; }
            }
        }
        return false; 
    }

    public void DetermineThreatLevelOfOpenTiles()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Tile2D[i, j] == TileType.None)
                {
                    int ThreatSeen = 0;
                    // CheckAsEnd
                    int Endthreat = CheckAsEnd(i, j);

                    // CheckAsMiddle
                    int MiddleThreat = CheckAsMiddle(i, j);

                    if (Endthreat > MiddleThreat || Endthreat == -1)
                    {
                         ThreatSeen = Endthreat;
                    }
                    else
                    {
                        ThreatSeen = MiddleThreat;
                    }

                    TileSpots[i + 4 * j].ThreatLevel = ThreatSeen;
                }
            }
        }
    }


    int CheckAsMiddle(int i, int j)
    {
        int ThreatSeen = 0;
        int ThreatSeenDiagonal1 = CheckDiagonal1Threat(i, j);
        int ThreatVertical = ChecVerticalThreat(i, j);
        int ThreatSeenDiagonal2 = CheckDiagonal2Threat(i, j);
        int ThreatHorizontal = CheckHorizontalThreat(i, j);

        if (ThreatSeen < ThreatSeenDiagonal1) { ThreatSeen = ThreatSeenDiagonal1; }
        if (ThreatSeen < ThreatVertical) { ThreatSeen = ThreatVertical; }
        if (ThreatSeen < ThreatSeenDiagonal2) { ThreatSeen = ThreatSeenDiagonal2; }
        if (ThreatSeen < ThreatHorizontal) { ThreatSeen = ThreatHorizontal; }

        return ThreatSeen;
    }

    int CheckDiagonal1Threat(int i, int j)
    {
        int threat = 0;
        //UpLeft
        if (i > 0 && j > 0)
        {
            if (Tile2D[i - 1, j - 1] == TileType.Moriarty)
            {
                threat++;
            }
        }
        //bottomRight
        if (i < 3 && j < 3)
        {
            if (Tile2D[i + 1, j + 1] == TileType.Moriarty)
            {
                threat++;
            }
        }
        return threat;
    }

    int ChecVerticalThreat(int i, int j)
    {
        int threat = 0;
        //Up
        if (j > 0)
        {
            if (Tile2D[i, j - 1] == TileType.Moriarty)
            {
                threat++;
            }
        }
        //Bottom
        if (j < 3)
        {
            if (Tile2D[i, j + 1] == TileType.Moriarty)
            {
                threat++;
            }
        }
        return threat;
    }

    int CheckDiagonal2Threat(int i, int j)
    {
        int threat = 0;
        //UpRight
        if (i < 3 && j > 0)
        {
            if (Tile2D[i + 1, j - 1] == TileType.Moriarty)
            {
                threat++;
            }
        }

        //BottomLeft
        if (i > 0 && j < 3)
        {
            if (Tile2D[i - 1, j + 1] == TileType.Moriarty)
            {
                threat++;
            }
        }
        return threat;
    }

    int CheckHorizontalThreat(int i, int j)
    {
        int threat = 0;
        //right
        if (i < 3)
        {
            if (Tile2D[i + 1, j] == TileType.Moriarty)
            {
                threat++;
            }
        }
        //Left
        if (i > 0)
        {
            if (Tile2D[i - 1, j] == TileType.Moriarty)
            {
                threat++;
            }
        }
        return threat;
    }



    int CheckAsEnd(int i, int j)
    {
        int ThreatSeen = -2;
        int UpLeftThreat = CheckUpLeftThreat(i,j);
        int UpThreat = CheckUpThreat(i, j);
        int UpRightThreat = CheckUpRightThreat(i, j);
        int RightThreat = CheckRightThreat(i, j);
        int BottomRightThreat = CheckBottomRightThreat(i, j);
        int BottomThreat = CheckBottomThreat(i, j);
        int BottomLeftThreat = CheckBottomLeftThreat(i, j);
        int LeftThreat = CheckLeftthreat(i, j);

        if (ThreatSeen < UpLeftThreat) { ThreatSeen = UpLeftThreat; }
        if (ThreatSeen < UpThreat) { ThreatSeen = UpThreat; }
        if (ThreatSeen < UpRightThreat) { ThreatSeen = UpRightThreat; }
        if (ThreatSeen < RightThreat) { ThreatSeen = RightThreat; }
        if (ThreatSeen < BottomRightThreat) { ThreatSeen = BottomRightThreat; }
        if (ThreatSeen < BottomThreat) { ThreatSeen = BottomThreat; }
        if (ThreatSeen < BottomLeftThreat) { ThreatSeen = BottomLeftThreat; }
        if (ThreatSeen < LeftThreat) { ThreatSeen = LeftThreat; }

        return ThreatSeen;
    }

    int CheckUpLeftThreat(int i, int j)
    {
        //UpLeft
        if (i > 0 && j > 0)
        {
            if (Tile2D[i - 1, j - 1] == TileType.Moriarty)
            {
                i--;
                j--;
                //UpLeft
                if (i > 0 && j > 0)
                {
                    if (Tile2D[i - 1, j - 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if ((Tile2D[i - 1, j - 1] == TileType.None))
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i - 1, j - 1] == TileType.None)
            {
                i--;
                j--;
                //UpLeft
                if (i > 0 && j > 0)
                {
                    if (Tile2D[i - 1, j - 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if ((Tile2D[i - 1, j - 1] == TileType.None))
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }


    int CheckUpThreat(int i, int j)
    {
        //Up
        if (j > 0)
        {
            if (Tile2D[i, j - 1] == TileType.Moriarty)
            {
                j--;
                if (j > 0)
                {
                    if (Tile2D[i, j - 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if  (Tile2D[i, j - 1] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i, j - 1] == TileType.None)
            {
                j--;
                if (j > 0)
                {
                    if (Tile2D[i, j - 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i, j - 1] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }

    int CheckUpRightThreat(int i, int j)
    {
        //UpRight
        if (i < 3 && j > 0)
        {
            if (Tile2D[i + 1, j - 1] == TileType.Moriarty)
            {
                i++;
                j--;
                //UpRight
                if (i < 3 && j > 0)
                {
                    if (Tile2D[i + 1, j - 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i + 1, j - 1] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i + 1, j - 1] == TileType.None)
            {
                i++;
                j--;
                //UpRight
                if (i < 3 && j > 0)
                {
                    if (Tile2D[i + 1, j - 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i + 1, j - 1] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }

    int CheckRightThreat(int i, int j)
    {
        //right
        if (i < 3)
        {
            if (Tile2D[i + 1, j] == TileType.Moriarty)
            {
                i++;
                if (i < 3)
                {
                    if (Tile2D[i + 1, j] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i + 1, j] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i + 1, j] == TileType.None)
            {
                i++;
                if (i < 3)
                {
                    if (Tile2D[i + 1, j] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i + 1, j] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }

    int CheckBottomRightThreat(int i, int j)
    {
        //BottomRight
        if (i < 3 && j < 3)
        {
            if (Tile2D[i + 1, j + 1] == TileType.Moriarty)
            {
                i++;
                j++;
                //BottomRight
                if (i < 3 && j < 3)
                {
                    if (Tile2D[i + 1, j + 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i + 1, j + 1] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i + 1, j + 1] == TileType.None)
            {
                i++;
                j++;
                //BottomRight
                if (i < 3 && j < 3)
                {
                    if (Tile2D[i + 1, j + 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i + 1, j + 1] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }

    int CheckBottomThreat(int i, int j)
    {
        //Bottom
        if (j < 3)
        {
            if (Tile2D[i, j + 1] == TileType.Moriarty)
            {
                j++;
                //Bottom
                if (j < 3)
                {
                    if (Tile2D[i, j + 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i, j + 1] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i, j + 1] == TileType.None)
            {
                j++;
                //Bottom
                if (j < 3)
                {
                    if (Tile2D[i, j + 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i, j + 1] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
       
        return -1;
    }
    

    int CheckBottomLeftThreat(int i, int j)
    {
        //BottomLeft
        if (i > 0 && j < 3)
        {
            if (Tile2D[i - 1, j + 1] == TileType.Moriarty)
            {
                i--;
                j++;
                //BottomLeft
                if (i > 0 && j < 3)
                {
                    if (Tile2D[i - 1, j + 1] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i - 1, j + 1] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i - 1, j + 1] == TileType.None)
            {
                i--;
                j++;
                //BottomLeft
                if (i > 0 && j < 3)
                {
                    if (Tile2D[i - 1, j + 1] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i - 1, j + 1] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }

    int CheckLeftthreat(int i, int j)
    {
        //Left
        if (i > 0)
        {
            if (Tile2D[i - 1, j ] == TileType.Moriarty)
            {
                i--;
                //Left
                if (i > 0)
                {
                    if (Tile2D[i - 1, j] == TileType.Moriarty)
                    {
                        return 2;
                    }
                    else if (Tile2D[i - 1, j] == TileType.None)
                    {
                        return 1;
                    }
                }
            }
            else if (Tile2D[i - 1, j] == TileType.None)
            {
                i--;
                //Left
                if (i > 0)
                {
                    if (Tile2D[i - 1, j] == TileType.Moriarty)
                    {
                        return 1;
                    }
                    else if (Tile2D[i - 1, j] == TileType.None)
                    {
                        return 0;
                    }
                }
            }
        }
        return -1;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
