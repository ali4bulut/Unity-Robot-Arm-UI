using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public InputField J1input;
    public Slider J1slider;

    public InputField J2input;
    public Slider J2slider;

    public InputField J3input;
    public Slider J3slider;

    public InputField J4input;
    public Slider J4slider;

    public InputField J5input;
    public Slider J5slider;

    public InputField J6input;
    public Slider J6slider;

    public Text tX, tY, tZ, tz1, ty, tz2;

    public InputField X_PosInput;
    public InputField Y_PosInput;
    public InputField Z_PosInput;

    public InputField Z_EulerInput1;
    public InputField Y_EulerInput;
    public InputField Z_EulerInput2;

    public Button reseetBut, runBut;

    public float X_position, Y_position, Z_position, z1Euler, yEuler, z2Euler;

    public float j1, j2, j3, j4, j5, j6;

    public bool isOk = false, rstBut = false, isRun = false, isFollow = false, isRepeat = false;

    public RobotKol robot;

    public List<float> savedPositions = new List<float>();
    public int savedCnt = 0;

    private void Start()
    {
        J1input.text = J1slider.value.ToString();
        J2input.text = J2slider.value.ToString();
        J3input.text = J3slider.value.ToString();
        J4input.text = J4slider.value.ToString();
        J5input.text = J5slider.value.ToString();
        J6input.text = J6slider.value.ToString();
        X_position = 0;
        Y_position = 980;
        Z_position = 478.5f;
    }

    private void Update()
    {
        j1 = J1slider.value;
        j2 = J2slider.value;
        j3 = J3slider.value;
        j4 = J4slider.value;
        j5 = J5slider.value;
        j6 = J6slider.value;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SavePos();
        }
    }

    public void FollowBut()
    {
        if (!isFollow) isFollow = true;
        else isFollow = false;
    }

    public void Run()
    {
        isRun = true;
    }

    public void Reset_But()
    {
        J1input.text = "0";
        J2input.text = "0";
        J3input.text = "0";
        J4input.text = "0";
        J5input.text = "0";
        J6input.text = "0";
        Z_EulerInput1.text = "0";
        Y_EulerInput.text = "0";
        Z_EulerInput2.text = "0";
        X_PosInput.text = "478.5";
        Y_PosInput.text = "0";
        Z_PosInput.text = "980";

        j1 = J1slider.value = 0f;
        j2 = J2slider.value = 0f;
        j3 = J3slider.value = 0f;
        j4 = J4slider.value = 0f;
        j5 = J5slider.value = 0f;
        j6 = J6slider.value = 0f;
        rstBut = true;
    }

    public void X_pos()
    {
        Z_position = float.Parse(X_PosInput.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void Y_pos()
    {
        X_position = -float.Parse(Y_PosInput.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void Z_pos()
    {
        Y_position = float.Parse(Z_PosInput.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void z1EulerAngle()
    {
        z1Euler = float.Parse(Z_EulerInput1.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void yEulerAngle()
    {
        yEuler = float.Parse(Y_EulerInput.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void z2EulerAngle()
    {
        z2Euler = float.Parse(Z_EulerInput2.text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        isOk = true;
    }

    public void _j1SliderValueChange()
    {
        J1input.text = J1slider.value.ToString();
    }

    public void _j1InputValueChange()
    {
        J1slider.value = float.Parse(J1input.text);
    }

    public void _j2SliderValueChange()
    {
        J2input.text = J2slider.value.ToString();
    }

    public void _j2InputValueChange()
    {
        J2slider.value = float.Parse(J2input.text);
    }

    public void _j3SliderValueChange()
    {
        J3input.text = J3slider.value.ToString();
    }

    public void _j3InputValueChange()
    {
        J3slider.value = float.Parse(J3input.text);
    }

    public void _j4SliderValueChange()
    {
        J4input.text = J4slider.value.ToString();
    }

    public void _j4InputValueChange()
    {
        J4slider.value = float.Parse(J4input.text);
    }

    public void _j5SliderValueChange()
    {
        J5input.text = J5slider.value.ToString();
    }

    public void _j5InputValueChange()
    {
        J5slider.value = float.Parse(J5input.text);
    }

    public void _j6SliderValueChange()
    {
        J6input.text = J6slider.value.ToString();
    }

    public void _j6InputValueChange()
    {
        J6slider.value = float.Parse(J6input.text);
    }

    public void SavePos()
    {
        savedPositions.Add(J1slider.value);
        savedPositions.Add(J2slider.value);
        savedPositions.Add(J3slider.value);
        savedPositions.Add(J4slider.value);
        savedPositions.Add(J5slider.value);
        savedPositions.Add(J6slider.value);
        savedPositions.Add(robot.comm.transform.position.x);
        savedPositions.Add(robot.comm.transform.position.y);
        savedPositions.Add(robot.comm.transform.position.z);

        savedCnt++;
        Debug.Log(savedCnt);
    }
    public void ClearSaved()
    {
        savedPositions.Clear();
        savedCnt = 0;
    }

    public void RepeatIt()
    {
        isRepeat = true;
    }

}
