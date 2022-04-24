using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Triggerer : MonoBehaviour
{
    SerialPort sp;
    float next_time; int ii = 0;
    public string PortName;
    public bool enabled = false;
    // Use this for initialization
    void Start()
    {
        if (!enabled)
        {
            return;
        }
        string the_com = "";
        next_time = Time.time;
        foreach (string mysps in SerialPort.GetPortNames())
        {
            print(mysps);
            if (mysps == PortName) { the_com = mysps; break; }
        }
        sp = new SerialPort("\\\\.\\" + the_com, 9600);
        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 9600");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
        {
            return;
        }
        if (!sp.IsOpen){
            sp.Open();
            print("opened sp");
        }
        
    }
    public void SendTrigger(byte trigger)
    {
        if (!enabled)
        {
            return;
        }
        if (sp.IsOpen)
        {
            print("Writing " + trigger);
            sp.Write(trigger.ToString());
        }

    }

}
