using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    // Use this for initialization
    bool Moving = false; 

    public void Move(Vector3 MovePosition)
    {
        Moving = true;
        IEnumerator MoveEnumerator = MoveTo(MovePosition);
        StartCoroutine(MoveEnumerator);
    }

    IEnumerator MoveTo(Vector3 MovePosition)
    {
        while (Moving)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if ((transform.localPosition - MovePosition).magnitude > .5f)
            {
                transform.localPosition = Vector3.Lerp(transform.position, MovePosition, Time.deltaTime * 10f);
            }
            else
            {
                Moving = false;
            }

        }
    }

}
