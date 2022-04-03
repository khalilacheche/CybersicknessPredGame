using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    private bool canMove = false;
    public Vector3 translationAcceleration;
    public float rotationAcceleration;
    private Vector3 lastPosition;
    private Vector3 lastTranlsationSpeed;
    
    private Vector3 forwardDirection;
    
    private float lastAngle;
    private float lastRotationSpeed;
    private float switchTime = 0;
    public float timeBetweenAccelerations= 5;



    public bool xTranslation = false;
    public bool zTranslation = false;
    public bool yRotation = false;

    private string experience ;    
   
    [Header("stale Experience Parameters")]
    public Vector3 stalePosition =new Vector3(-160.5f,-2.5f, 168.7f);
    [Header("Rotation Experience Parameters")]
    public Vector3 crossingPosition =new Vector3(-112,-0.89f, 180.9f);
    [Header("X or Z  w/o R Experiences Parameters")]
    public Vector3 startPosition =new Vector3(-57,-1.7f, 180.9f);
    
    [Header("X and Z  w/o R Experiences Parameters")]
    public Vector3 startPositionXZ =new Vector3(57,-1.7f, 186.5f);


    [Header("X Translation Parameters")]
    public float xSpeed = 10;
    public float maxXSpeed = 15.0f; //The max speed at which the player moves
    private int xDirection = 1;


    [Header("Z translation Parameters")]

    public float zSpeed = 10;
    public float leftLane = 179.14f;
    public float rightLane = 187.43f;
    private float targetLane;
    private float zThreshold = 0.05f;

    [Header("Y rotation parameters")]

    public float[] angles = {300,240,120,60};

    

    private int targetAngleIndex;

    float degreesPerSecond = 20;
    public float rotationSpeed = 5;
    private float yThreshold = 1f;


    private GameManager gm;
    // Start is called before the first frame update
    void OnEnable(){
        forwardDirection = transform.forward;
        determineExperience();
        //initializePos();        
        targetLane = rightLane;
        targetAngleIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(gm == null){
            GameObject gmGO =GameObject.Find("GameManager");
            if(gmGO != null){
                gm = gmGO.GetComponent<GameManager>();
            }
        } 

        if(canMove){
            updateMov();
        }
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
            switchLane(); 
        }
        if(col.gameObject.layer== LayerMask.NameToLayer("turn")){
            rotateAxis();
        }
        if(col.gameObject.layer== LayerMask.NameToLayer("turnaround")){
            xDirection*=-1;
        }
        if(col.gameObject.layer== LayerMask.NameToLayer("carBehind")){
            switchLane();
            accelerate(10);
            rotateAxis();
        }
        
        if(col.gameObject.layer== LayerMask.NameToLayer("carFront")){
            switchLane();
            accelerate(-10);
            rotateAxis();
        }
    }
    private void switchLane(){
        //value of targetLane is gonna be only the valueof rightLane or leftLane, so nit's okayto perform float equality
        if(targetLane == rightLane){
            targetLane = leftLane;
        }else{
            targetLane = rightLane;

        }
    }
    private void rotateAxis(){
        forwardDirection = Quaternion.Euler(0,-90,0) * forwardDirection;
    }
    

    private void switchAngle(){
        targetAngleIndex = (targetAngleIndex + Random.Range(1,angles.Length)) % angles.Length;
                        Debug.Log("xspeed =" + xSpeed);

    
    }
    private void accelerate(int speed){
        xSpeed += speed;
    }
    private void switchSpeed(){
        if (xSpeed <=10)
            accelerate(15);
        else 
            accelerate(5);
    }
    private void moveForward(){
        transform.position += forwardDirection * Time.deltaTime * xSpeed * xDirection;
    }
    private void determineExperience(){
        
        if (xTranslation){
            experience ="X";
        }
        else{
            experience ="o";
        }
        
        if (zTranslation){
            experience +="Z";
        }
        else{
            experience +="o";
        }
        
        if (yRotation){
            experience +="R";
        }
        else{
            experience +="o";
        }
    } 
    private void rotationMovement(){
        float diff = angles[targetAngleIndex] - transform.localEulerAngles.y;
            if(Mathf.Abs(diff) > yThreshold){
                float rotationValue =  rotationSpeed * Mathf.Sign(diff)  * Time.deltaTime * ( diff > 180 ? -1 : 1);
                transform.Rotate(Vector3.up *rotationValue);
            }
    }
    private void ChangeOfAccelerationByFrequency(){
        switchTime += Time.deltaTime;
            if(switchTime> timeBetweenAccelerations){
                switch(experience){
                    case "Xoo":
                    case "oZo":{
                        switchSpeed();
                        break;
                    }
                    case "ooR":{
                        switchAngle();
                        break;
                    }
                    case "XoR":
                    case "oZR":
                    {
                        switchSpeed();
                        switchAngle();
                        break;
                    }
                }
                switchTime = 0;
        }
    }
    private void updateMov(){
    switch(experience){
            case "ooo":{
                break;
            } 
            case "Xoo":
            case "oZo":{
                ChangeOfAccelerationByFrequency();
                moveForward();
                break;
            }
            case "ooR":{
                //TODO: check time stamp before starting rotation
                ChangeOfAccelerationByFrequency();                
                rotationMovement();
                break;
            }
            case "XoR":
            case "oZR":{
                ChangeOfAccelerationByFrequency();
                rotationMovement();
                moveForward();
                break;
            }
            case "XZo":{
                moveForward();
                break; 
            }
            case "XZR":{
                moveForward();
                rotationMovement();
                break; 
            }
            default:
                break;
        }
    }
    
    private void initializePos(){
        switch(experience){
            case "ooo":{
                transform.Rotate(0.0f ,90.0f,0.0f, Space.Self);
                transform.position = stalePosition;
                break;
            } 
            case "ooR":{
                transform.position = crossingPosition;
                break;
            }
            case "XoR":
            case "Xoo":{
                transform.position =startPosition;
                break;
            }
            case "oZR":
            case "oZo": {
                transform.position =startPosition;
                transform.Rotate(0.0f ,90.0f,0.0f, Space.Self);
                break;
            }
            default:{
                transform.position = startPositionXZ;
                break;
            }
        }
    }
    public void initializeExperimentPosition(){
        initializePos();
    }
    public void setPosition(Vector3 position){
        transform.position = position;
    }
    public string getExperience(){
        return experience;
    }
    public void startMoving(){
        canMove = true;
    }
}
