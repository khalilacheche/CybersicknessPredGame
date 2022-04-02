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
    
    public Vector3 forwardDirection;
    public Vector3 leftDirection;
    private Vector3 leftLane;
    private Vector3 rightLane;
    private int turnNumber;

    private float lastAngle;
    private float lastRotationSpeed;
    private float switchTime = 0;
    public float timeBetweenAccelerations= 5;
    public Vector3 axisSelector;


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
    public Transform[] leftLanesT = new Transform[4];
    public Transform[] rightLanesT = new Transform[4];

    Vector3[] leftLanes = new Vector3[4];
    Vector3[] rightLanes = new Vector3[4];
    public Vector3 targetLane;
    private float zThreshold = 0.1f;

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
        leftDirection = new Vector3(forwardDirection.z, forwardDirection.y, -forwardDirection.x);
        initializeLanes();
        determineExperience();
        initializePos();        
        targetLane = rightLane;
        targetAngleIndex = 0;
        canMove = false;
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
        }
        
        if(col.gameObject.layer== LayerMask.NameToLayer("carFront")){
            switchLane();
            accelerate(-10);
        }
    }
    private void switchLane(){
        //value of targetLane is gonna be only the valueof rightLane or leftLane, so it's okay to perform float equality
        if(targetLane == rightLane){
            targetLane = leftLane;
        }else{
            targetLane = rightLane;

        }
    }
    private void rotateAxis(){
        forwardDirection = Quaternion.Euler(0,-90,0) * forwardDirection;
        leftDirection = Quaternion.Euler(0, -90, 0) * leftDirection;
        turnNumber= (turnNumber+1) % 4;
        leftLane = leftLanesT[turnNumber].position;
        rightLane = rightLanesT[turnNumber].position;
        targetLane = rightLane;
    }


    private void switchAngle(){
        targetAngleIndex = (targetAngleIndex + Random.Range(1,angles.Length)) % angles.Length;

    
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
    private void moveSideways()
    {
        axisSelector = new Vector3(Mathf.Abs(leftDirection.x), Mathf.Abs(leftDirection.y), Mathf.Abs(leftDirection.z));


        if (Mathf.Abs(Vector3.Dot(transform.position, axisSelector) - Vector3.Dot(axisSelector, targetLane)) > zThreshold)
        {
            Debug.Log(Mathf.Abs(Vector3.Dot(transform.position, axisSelector) - Vector3.Dot(axisSelector, targetLane)));
            Debug.Log(turnNumber);
            Debug.Log(targetLane);

            transform.position += axisSelector * Time.deltaTime * zSpeed * Mathf.Sign(Vector3.Dot(axisSelector, targetLane) - Vector3.Dot(axisSelector,transform.position));
        }
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
                moveSideways();
                break; 
            }
            case "XZR":{
                moveForward();
                moveSideways();    
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
    private void initializeLanes()
    {
        leftLanes[0]= new Vector3(0, 0, 179.14f);
        rightLanes[0] = new Vector3(0, 0, 187.43f);
        leftLanes[1] = new Vector3(-125, 0, 0);
        rightLanes[1] = new Vector3(-138, 0, 0);
        leftLanes[2] = new Vector3(0, 0, -275);
        rightLanes[2] = new Vector3(0, 0, -262);
        leftLanes[3] = new Vector3(143, 0, 0);
        rightLanes[3] = new Vector3(156, 0, 0);
        leftLane = leftLanes[0];
        rightLane = rightLanes[0];
        turnNumber = 0;
    }
    public string getExperience(){
        return experience;
    }
    public void startMoving(){
        canMove = true;
    }
}
