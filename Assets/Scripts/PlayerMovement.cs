using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
 public class PlayerMovement : MonoBehaviour
 {
    private Vector2 trackpad;
    private Queue<Vector3> rollingAverage;

    public int windowLength = 30;
    public SteamVR_Input_Sources Hand;//Set Hand To Get Input From
    public float speed; //The max speed at which the player moves
    public float deadzone; //the Deadzone of the trackpad. used to prevent unwanted walking.
    public GameObject playerCamera;

    void Start(){
        rollingAverage = new Queue<Vector3>();
        for(int i = 0;  i< windowLength; ++i)
            rollingAverage.Enqueue(Vector3.zero);
    }
    void Update()
    {
    trackpad = SteamVR_Actions._default.Move.GetAxis(Hand);
    Vector3 speedDirection = Vector3.zero; 
    if(trackpad.magnitude>deadzone){
        speedDirection = Quaternion.Euler(0,playerCamera.transform.localEulerAngles.y,0)* new Vector3(trackpad.x ,0 , trackpad.y) * speed;
    }
    rollingAverage.Dequeue();
    rollingAverage.Enqueue(speedDirection); 
    GetComponent<Rigidbody>().velocity = getMean();
    //Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);
    }

    private Vector3 getMean(){
        float x = 0;
        float y = 0;
        float z = 0;
        foreach (Vector3 vec in rollingAverage){
            x += vec.x;
            y += vec.y;
            z += vec.z;
        }
        if(windowLength != 0) {

            x = x/windowLength;
            y = y/ windowLength;
            z = z/windowLength;
        }

        return new Vector3 (x,y,z);
    }
 }