using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    bool b_Move = false;
    public Vector3 MovePoint = Vector3.zero;


    private void Start()
    {
        b_Move = false;
    }

    public void Move(Vector3 MovePosition)
    {
        b_Move = true;
        MovePoint = MovePosition;
    }

    void Update()
    {
        if (b_Move)
        {
            if ((transform.localPosition - MovePoint).magnitude > .002f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, MovePoint, Time.deltaTime * 2f);
            }
            else
            {
                b_Move = false;
            }

        }
    }

}
