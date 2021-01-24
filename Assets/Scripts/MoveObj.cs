using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord = 0.0f;

    public Transform EndEfector;

    public bool onDrag = false;

    public float beta;
    public float alfa;
    public float gama;

    public Panel panel;

    private void Start()
    {
        gameObject.transform.position = new Vector3(0, 812.95f, 642.75f);
        gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
        panel.tX.text = EndEfector.transform.position[2].ToString("F2").Replace(",", ".");
        panel.tY.text = (-EndEfector.transform.position[0]).ToString("F2").Replace(",", ".");
        panel.tZ.text = EndEfector.transform.position[1].ToString("F2").Replace(",", ".");
    }

    // public void LateUpdate()
    // {


    //     // Debug.Log(EndEfector.localToWorldMatrix);
    //     // [ 0 4 8 ]
    //     // [ 1 5 9 ]    
    //     // [ 2 6 10 ]    
    //     // Debug.Log(Mathf.Acos(EndEfector.localToWorldMatrix[0])* (180 / Mathf.PI) + " / " + Mathf.Acos(EndEfector.localToWorldMatrix[1])* (180 / Mathf.PI) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[2])* (180 / Mathf.PI) + " / " +
    //     //         Mathf.Acos(EndEfector.localToWorldMatrix[4]) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[5])* (180 / Mathf.PI) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[6])* (180 / Mathf.PI) + " / " +
    //     //         Mathf.Acos(EndEfector.localToWorldMatrix[8])* (180 / Mathf.PI) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[9])* (180 / Mathf.PI) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[10])* (180 / Mathf.PI));
    //     //Debug.Log( Mathf.Acos(EndEfector.localToWorldMatrix[0])* (180 / Mathf.PI) + " / " +Mathf.Acos(EndEfector.localToWorldMatrix[5])* (180 / Mathf.PI) + " / " + Mathf.Acos(EndEfector.localToWorldMatrix[10])* (180 / Mathf.PI));
    //     //Debug.Log(EndEfector.localToWorldMatrix[6]);


    // }

    private void Update()
    {
        #region Euler Açılarını Hesaplama
        if (EndEfector.localToWorldMatrix[6] > 0.0f)
        {
            beta = Mathf.Atan2(Mathf.Sqrt(EndEfector.localToWorldMatrix[1] * EndEfector.localToWorldMatrix[1] + EndEfector.localToWorldMatrix[9] * EndEfector.localToWorldMatrix[9]),
                                            EndEfector.localToWorldMatrix[5]) * Mathf.Rad2Deg;
        }

        else
        {
            beta = -Mathf.Atan2(Mathf.Sqrt(EndEfector.localToWorldMatrix[1] * EndEfector.localToWorldMatrix[1] + EndEfector.localToWorldMatrix[9] * EndEfector.localToWorldMatrix[9]),
                                EndEfector.localToWorldMatrix[5]) * Mathf.Rad2Deg;
        }

        if (beta == 0) beta = .000001f;

        alfa = -Mathf.Atan2(EndEfector.localToWorldMatrix[4] / Mathf.Abs(Mathf.Sin(beta)), EndEfector.localToWorldMatrix[6] / Mathf.Abs(Mathf.Sin(beta))) * Mathf.Rad2Deg;
        gama = -Mathf.Atan2(EndEfector.localToWorldMatrix[1] / Mathf.Abs(Mathf.Sin(beta)), -(EndEfector.localToWorldMatrix[9] / Mathf.Abs(Mathf.Sin(beta)))) * Mathf.Rad2Deg;

        if (EndEfector.localToWorldMatrix[6] < 0.0f)
        {
            if (alfa < 0)
            {
                alfa = 180 + alfa;
                gama = 180 + gama;
            }
            else
            {
                alfa = alfa - 180;
                gama = gama - 180;
            }
        }
        //Debug.Log(alfa + " / " + beta + " / " + gama);
        #endregion

        if (panel.rstBut)
        {
            gameObject.transform.position = new Vector3(0, 812.95f, 642.75f);
            gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            panel.rstBut = false;
        }

        panel.tX.text = EndEfector.transform.position[2].ToString("F2").Replace(",", ".");
        panel.tY.text = (-EndEfector.transform.position[0]).ToString("F2").Replace(",", ".");
        panel.tZ.text = EndEfector.transform.position[1].ToString("F2").Replace(",", ".");


        if (panel.isOk)
        {
            transform.position = new Vector3(panel.X_position, panel.Y_position, panel.Z_position);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(0.00001f, 0, -panel.z1Euler);
            transform.Rotate(panel.yEuler, 0, 0);
            transform.Rotate(0.00001f, -panel.z2Euler, 0);
            panel.isOk = false;
        }
        panel.tz1.text = alfa.ToString("F2").Replace(",", ".");
        panel.ty.text = beta.ToString("F2").Replace(",", ".");
        panel.tz2.text = gama.ToString("F2").Replace(",", ".");


        //Debug.Log(alfa + " / " + beta + " / " + gama);
    }


    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDown()
    {
        onDrag = true;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        panel.X_PosInput.text = panel.tX.text;
        panel.Y_PosInput.text = panel.tY.text;
        panel.Z_PosInput.text = panel.tZ.text;

        transform.position = GetMouseWorldPos() + mOffset;
        panel.X_position = (GetMouseWorldPos() + mOffset).x;
        panel.Y_position = (GetMouseWorldPos() + mOffset).y;
        panel.Z_position = (GetMouseWorldPos() + mOffset).z;

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, .5f, 0, Space.Self);

        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -.5f, 0, Space.Self);

        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(-.5f, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(.5f, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, .5f, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, -.5f, Space.Self);
        }
    }

    private void OnMouseUp()
    {
        onDrag = false;
    }
}
