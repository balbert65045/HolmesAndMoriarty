using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort {

    public int[] Arr;
    // Use this for initialization
    public int[] Sort(int[] InitArr)
    {
        Arr = InitArr;
        QSort(Arr, 0, Arr.Length - 1);
        return Arr;
    }


    void QSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = partition(Arr, low, high);

            QSort(Arr, low, pi - 1);
            QSort(Arr, pi + 1, high);
        }
    }



    int partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < (high); j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                // Swap
                Swap(arr, j, i);
            }
        }
        Swap(Arr, i + 1, high);
        return (i + 1);

    }

    void Swap(int[] arr, int index1, int index2)
    {
        int temp = arr[index1];
        arr[index1] = arr[index2];
        arr[index2] = temp;
    }
}
