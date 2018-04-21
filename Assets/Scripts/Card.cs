using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    bool Moving = false;
    public Vector3 MovePoint = Vector3.zero;


    public void Move(Vector3 MovePosition)
    {
        Moving = true;
        MovePoint = MovePosition;
    }

    void Update()
    {
        if (Moving)
        {
            if ((transform.localPosition - MovePoint).magnitude > .002f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, MovePoint, Time.deltaTime * 2f);
            }
            else
            {
                Moving = false;
            }

        }
    }

}
