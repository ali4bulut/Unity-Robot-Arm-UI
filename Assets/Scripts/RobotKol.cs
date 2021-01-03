using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotKol : MonoBehaviour
{
    public GameObject Axis1;
    public GameObject Axis2;
    public GameObject Axis3;
    public GameObject Axis4;
    public GameObject Axis5;
    public GameObject Axis6;

    public Panel panel;
    public MoveObj move;
    public Comm comm;

    private int i = 0;

    private void Update()
    {
        if (panel.isRun)
        {

            Axis1.transform.localRotation = Quaternion.Euler(0, -comm.Acilar[0], 0);
            Axis2.transform.localRotation = Quaternion.Euler(-comm.Acilar[1], 0, 0);
            Axis3.transform.localRotation = Quaternion.Euler(-comm.Acilar[2], 0, 0);
            Axis4.transform.localRotation = Quaternion.Euler(0, 0, -comm.Acilar[3]);
            Axis5.transform.localRotation = Quaternion.Euler(-comm.Acilar[4], 0, 0);
            Axis6.transform.localRotation = Quaternion.Euler(0, 0, -comm.Acilar[5]);

            panel.J1slider.value = comm.Acilar[0];
            panel.J2slider.value = comm.Acilar[1];
            panel.J3slider.value = comm.Acilar[2];
            panel.J4slider.value = comm.Acilar[3];
            panel.J5slider.value = comm.Acilar[4];
            panel.J6slider.value = comm.Acilar[5];

            panel.isRun = false;

        }
        else if (panel.isFollow)
        {
            Axis6.transform.localRotation = Quaternion.Lerp(Axis6.transform.localRotation, Quaternion.Euler(0, 0, -comm.Acilar[5]), Time.deltaTime * 4f);
            Axis5.transform.localRotation = Quaternion.Lerp(Axis5.transform.localRotation, Quaternion.Euler(-comm.Acilar[4], 0, 0), Time.deltaTime * 4f);
            Axis4.transform.localRotation = Quaternion.Lerp(Axis4.transform.localRotation, Quaternion.Euler(0, 0, -comm.Acilar[3]), Time.deltaTime * 4f);
            Axis3.transform.localRotation = Quaternion.Lerp(Axis3.transform.localRotation, Quaternion.Euler(-comm.Acilar[2], 0, 0), Time.deltaTime * 4f);
            Axis2.transform.localRotation = Quaternion.Lerp(Axis2.transform.localRotation, Quaternion.Euler(-comm.Acilar[1], 0, 0), Time.deltaTime * 4f);
            Axis1.transform.localRotation = Quaternion.Lerp(Axis1.transform.localRotation, Quaternion.Euler(0, -comm.Acilar[0], 0), Time.deltaTime * 4f);


            panel.J1slider.value = comm.Acilar[0];
            panel.J2slider.value = comm.Acilar[1];
            panel.J3slider.value = comm.Acilar[2];
            panel.J4slider.value = comm.Acilar[3];
            panel.J5slider.value = comm.Acilar[4];
            panel.J6slider.value = comm.Acilar[5];
        }

        else if (panel.isRepeat)
        {
            Repeat();
        }
        else
        {
            Axis1.transform.localRotation = Quaternion.Euler(0, -panel.j1, 0);
            Axis2.transform.localRotation = Quaternion.Euler(-panel.j2, 0, 0);
            Axis3.transform.localRotation = Quaternion.Euler(-panel.j3, 0, 0);
            Axis4.transform.localRotation = Quaternion.Euler(0, 0, -panel.j4);
            Axis5.transform.localRotation = Quaternion.Euler(-panel.j5, 0, 0);
            Axis6.transform.localRotation = Quaternion.Euler(0, 0, -panel.j6);
        }


    }

    void Repeat()
    {
        if (i < panel.savedCnt)
        {
            Axis6.transform.localRotation = Quaternion.Slerp(Axis6.transform.localRotation, Quaternion.Euler(0, 0, -panel.savedPositions[(i * 9) + 5]), Time.deltaTime * 4f);
            Axis5.transform.localRotation = Quaternion.Slerp(Axis5.transform.localRotation, Quaternion.Euler(-panel.savedPositions[(i * 9) + 4], 0, 0), Time.deltaTime * 4f);
            Axis4.transform.localRotation = Quaternion.Slerp(Axis4.transform.localRotation, Quaternion.Euler(0, 0, -panel.savedPositions[(i * 9) + 3]), Time.deltaTime * 4f);
            Axis3.transform.localRotation = Quaternion.Slerp(Axis3.transform.localRotation, Quaternion.Euler(-panel.savedPositions[(i * 9) + 2], 0, 0), Time.deltaTime * 4f);
            Axis2.transform.localRotation = Quaternion.Slerp(Axis2.transform.localRotation, Quaternion.Euler(-panel.savedPositions[(i * 9) + 1], 0, 0), Time.deltaTime * 4f);
            Axis1.transform.localRotation = Quaternion.Slerp(Axis1.transform.localRotation, Quaternion.Euler(0, -panel.savedPositions[i * 9], 0), Time.deltaTime * 4f);

            if (i == panel.savedCnt - 1)
            {
                panel.J1slider.value = panel.savedPositions[i * 9];
                panel.J2slider.value = panel.savedPositions[(i * 9) + 1];
                panel.J3slider.value = panel.savedPositions[(i * 9) + 2];
                panel.J4slider.value = panel.savedPositions[(i * 9) + 3];
                panel.J5slider.value = panel.savedPositions[(i * 9) + 4];
                panel.J6slider.value = panel.savedPositions[(i * 9) + 5];
            }

            Vector3 pos = new Vector3(panel.savedPositions[(i * 9) + 6], panel.savedPositions[(i * 9) + 7], panel.savedPositions[(i * 9) + 8]);
            if (comm.transform.position.x - pos.x < .5f & comm.transform.position.y - pos.y < .5f & comm.transform.position.z - pos.z < .5f) i++;
        }
        else
        {
            i = 0;
            panel.isRepeat = false;
        }
    }
}
