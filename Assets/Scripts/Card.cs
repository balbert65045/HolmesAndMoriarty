using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    bool b_Move = false;
    bool S_Move = false;
    bool C_Move = false;
    public float MoveSpeed = .2f;
    public Vector3 MovePoint = Vector3.zero;
    Vector3 MiddleMovePos;

    private void Start()
    {
        b_Move = false;
    }

    public virtual void MoveUp(int pos)
    {

    }

    public virtual void MoveBackDown()
    {

    }

    public void Move(Vector3 MovePosition)
    {
        b_Move = true;
        MovePoint = MovePosition;
    }

    public void SwapMove(Transform movePoint)
    {
        Debug.Log(movePoint.parent);
        MovePoint = movePoint.position;
        Vector3 OGPoint = transform.position;

        //transform.SetParent(null);
        //MovePoint = movePoint.TransformPoint(movePoint.parent.position);
        //Vector3 OGPoint = transform.TransformPoint(transform.parent.position);
        //Moving Up
        Debug.Log("move" + MovePoint);
        Debug.Log("start" + OGPoint);
        if (MovePoint.z > OGPoint.z)
        {
            float z = ((MovePoint.z - OGPoint.z) / 2) + OGPoint.z;
            float x = MovePoint.x + 4;
            MiddleMovePos = new Vector3(x, MovePoint.y, z);
        }
        //moving down
        else
        {
            float z = ((MovePoint.z - OGPoint.z) / 2) + OGPoint.z;
            float x = MovePoint.x - 4;
            MiddleMovePos = new Vector3(x, MovePoint.y, z);
        }

        S_Move = true;
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

        else if (S_Move)
        {
            Debug.Log("Smove");
            //Vector3 OGPoint = transform.TransformPoint(transform.position);
            Vector3 OGPoint = transform.position;
            if ((OGPoint - MiddleMovePos).magnitude > .4f)
            {
                Vector3 direction = (MiddleMovePos - OGPoint).normalized;
                transform.position = transform.position + direction*MoveSpeed;
            }
            else
            {
                S_Move = false;
                C_Move = true;
            }
        }

        else if (C_Move)
        {
            Debug.Log("Cmove");
            if ((transform.localPosition).magnitude > .002f)
            {
               // Vector3 direction = (MovePoint - OGPoint).normalized;
               // transform.position = transform.position + direction * MoveSpeed;
                //Vector3 WorldPos = Vector3.Lerp(OGPoint, MovePoint, Time.deltaTime * 2f);
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }
            else
            {
                Debug.Log("Stoped CMove");
                C_Move = false;
            }
        }

    }

}
