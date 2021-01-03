using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class Comm : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    string receivedData;
    bool running;

    public float[] Acilar = { 0, 0, 0, 0, 0, 0 };

    public MoveObj move;

    private string poz;

    // private  float[] PreAcilar = {0,0,0,0,0,0};
    // public GameObject Axis1;
    // public GameObject Axis2;
    // public GameObject Axis3;
    // public GameObject Axis4;
    // public GameObject Axis5;
    // public GameObject Axis6;


    private void Start()
    {
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }
    private void Update()
    {
        poz = move.EndEfector.transform.position[2].ToString("F2").Replace(",", ".") + "|" + (-move.EndEfector.transform.position[0]).ToString("F2").Replace(",", ".") + "|" +
            move.EndEfector.transform.position[1].ToString("F2").Replace(",", ".") + "|" + move.alfa.ToString("F2").Replace(",", ".") + "|" + move.beta.ToString("F2").Replace(",", ".") + "|" +
            move.gama.ToString("F2").Replace(",", ".");


        //string a = Acilar[0].ToString() + " / " + Acilar[1].ToString()+ " / " + Acilar[2].ToString()+ " / " + Acilar[3].ToString()+ " / "+ Acilar[4].ToString()+ " / " + Acilar[5].ToString();
        //string s = PreAcilar[0].ToString() + " / " + PreAcilar[1].ToString()+ " / " + PreAcilar[2].ToString()+ " / " + PreAcilar[3].ToString()+ " / "+ PreAcilar[4].ToString()+ " / " + PreAcilar[5].ToString();
        //Debug.Log(poz);

        // if (move.panel.isRun){
        //     Axis1.transform.Rotate(0, PreAcilar[0], 0);
        //     Axis2.transform.Rotate(PreAcilar[1], 0, 0);
        //     Axis3.transform.Rotate(PreAcilar[2], 0, 0);
        //     Axis4.transform.Rotate(0, 0, PreAcilar[3]);
        //     Axis5.transform.Rotate(PreAcilar[4], 0, 0);
        //     Axis6.transform.Rotate(0, 0, PreAcilar[5]);

        //     Axis1.transform.Rotate(0, -Acilar[0], 0);
        //     Axis2.transform.Rotate(-Acilar[1], 0, 0);
        //     Axis3.transform.Rotate(-Acilar[2], 0, 0);
        //     Axis4.transform.Rotate(0, 0, -Acilar[3]);
        //     Axis5.transform.Rotate(-Acilar[4], 0, 0);
        //     Axis6.transform.Rotate(0, 0, -Acilar[5]);

        //     for (int i = 0; i < 6; i++){
        //         PreAcilar[i] = Acilar[i];
        //     } 
        //     move.panel.isRun = false; 
        // }

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---Sending Data to Host----
        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(poz); //Converting string to byte data
        nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        if (dataReceived != null)
        {

            //---Using received data---
            receivedData = dataReceived; //<-- assigning receivedPos value from Python
            var Gelen = receivedData.Split(","[0]);

            for (int i = 0; i < Gelen.Length; i++)
            {
                Acilar[i] = float.Parse(Gelen[i], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            }

        }
    }

}
