using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    [Header("EXPERIMENT")]
    public bool xTranslation = false;
    public bool zTranslation = false;
    public bool yRotation = false;



    private bool canMove = false;

    private bool switchedLane = true;
    private bool isDepassing = false;
    private GameObject carToDepass;


    private Vector3 lastForward;
    public Vector3 translationAcceleration;
    public Vector3 rotationAcceleration;
    private Vector3 lastPosition;
    private Vector3 lastLocalSpeed;
    private Vector3 lastTranlsationSpeed;
    private Vector3 lastAngles;
    private Vector3 lastRotationSpeed;


    public Vector3 forwardDirection;
    public Vector3 leftDirection;
    private Vector3 leftLane;
    private Vector3 rightLane;
    private int turnNumber;
    private float switchTime = 0;
    public float timeBetweenAccelerations = 5;
    public Vector3 axisSelector;


    

    private string experience;



    [Header("ooo Experiment Parameters")]
    public Vector3 OOOStartPosition = new Vector3(-160.5f, -2.5f, 168.7f);
    public Vector3 OOOStartRotation = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Xoo Experiment Parameters")]
    public Vector3 XOOStartPosition = new Vector3(-57, -1.7f, 180.9f);
    public Vector3 XOOStartRotation = new Vector3(0.0f, 270.0f, 0.0f);

    [Header("oZo Experiment Parameters")]
    public Vector3 OZOStartPosition = new Vector3(-57, -1.7f, 180.9f);
    public Vector3 OZOStartRotation = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("ooR Experiment Parameters")]
    public Vector3 OORStartPosition = new Vector3(-112, -0.89f, 180.9f);
    public Vector3 OORStartRotation = new Vector3(0.0f, 270.0f, 0.0f);
    public float[] OORAngles = { 290, 250, 110, 70 };

    [Header("XoR Experiment Parameters")]
    public Vector3 XORStartPosition = new Vector3(-57, -1.7f, 180.9f);
    public Vector3 XORStartRotation = new Vector3(0.0f, 270.0f, 0.0f);
    public float[] XORAngles = { 200, 170, 20, -20};

    [Header("oZR Experiment Parameters")]
    public Vector3 OZRStartPosition = new Vector3(-57, -1.7f, 180.9f);
    public Vector3 OZRStartRotation = new Vector3(0.0f, 270.0f, 0.0f);
    public float[] OZRAngles = { 290, 250, 110, 70 };

    [Header("XZo Experiment Parameters")]
    public Vector3 XZOStartPosition = new Vector3(57, -1.7f, 186.5f);
    public Vector3 XZOStartRotation = new Vector3(0.0f, 270.0f, 0.0f);

    [Header("XZR Experiment Parameters")]
    public Vector3 XZRStartPosition = new Vector3(57, -1.7f, 186.5f);
    public Vector3 XZRStartRotation = new Vector3(0.0f, 270.0f, 0.0f);
    public float[] XZRAngles = { 290, 250, 110, 70 };





    


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
    public float translationThreshold = 0.05f;

    [Header("Y rotation parameters")]

     public float[] angles = new float[4];//= { 300, 240, 120, 60 };



    private int targetAngleIndex;

    float degreesPerSecond = 20;
    public float rotationSpeed = 5;
    public float yThreshold = 1f;


    private GameManager gm;
    void Start(){
        //initializePos();        
        determineExperience();
        gm = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void FixedUpdate() {
        if (gm == null) {
            gm = GameObject.Find("GameManager")?.GetComponent<GameManager>();
            
        }

        if (canMove) {
            updateMov();
        }
        calculateAccelerations();

    }

    private void calculateAccelerations()
    {
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 currentSpeed = (currentPosition - lastPosition)/Time.fixedDeltaTime;
        translationAcceleration = (currentSpeed - lastTranlsationSpeed) / Time.fixedDeltaTime;



        Vector3 currLocalPosition = transform.localToWorldMatrix * transform.position;
        Vector3 lastLocalPosition = transform.localToWorldMatrix * lastPosition;

        Vector3 currLocalSpeed = (currLocalPosition - lastLocalPosition) / Time.fixedDeltaTime;
        Vector3 localTranslationAccleration = (currLocalSpeed - lastLocalSpeed) / Time.fixedDeltaTime;
        translationAcceleration = localTranslationAccleration;
        lastPosition = currentPosition;
        lastTranlsationSpeed = currentSpeed;
        lastLocalSpeed = currLocalSpeed;



        Vector3 currForProjXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up); //Normal is Y axis
        Vector3 lastForProjXZ = Vector3.ProjectOnPlane(lastForward, Vector3.up); //Normal is Y axis
        float rotationOnYAxis = Vector3.SignedAngle(lastForProjXZ, currForProjXZ, Vector3.up);
        lastForward = transform.forward;

        Vector3 currentAngles = gameObject.transform.localRotation.eulerAngles;
        Vector3 currentRotationSpeed = new Vector3(0,rotationOnYAxis,0) / Time.fixedDeltaTime;

        //Debug.Log(currentRotationSpeed);
        rotationAcceleration = (currentRotationSpeed - lastRotationSpeed) / Time.fixedDeltaTime;
        lastAngles = currentAngles;
        lastRotationSpeed = currentRotationSpeed;
   

    }


    private void OnTriggerEnter(Collider col) {
        Debug.Log(col.name);
        if (col.gameObject.layer == LayerMask.NameToLayer("turn")) {
            rotateAxis();
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("turnaround")) {
            xDirection *= -1;
        }
    }
    private void switchLane() {
        //value of targetLane is gonna be only the value of rightLane or leftLane, so it's okay to perform float equality
        if (targetLane == rightLane) {
            targetLane = leftLane;
        } else {
            targetLane = rightLane;

        }
    }
    private void rotateAxis() {
        forwardDirection = Quaternion.Euler(0, -90, 0) * forwardDirection;
        leftDirection = Quaternion.Euler(0, -90, 0) * leftDirection;
        turnNumber = (turnNumber + 1) % 4;
        leftLane = leftLanesT[turnNumber].position;
        rightLane = rightLanesT[turnNumber].position;
        if (isDepassing)
            targetLane = leftLane;
        else
            targetLane = rightLane;
        
        switchAngle();

    }


    private void switchAngle() {
        targetAngleIndex = (targetAngleIndex + Random.Range(1, angles.Length)) % angles.Length;


    }
    private void accelerate(int speed) {
        xSpeed += speed;
    }
    private void switchSpeed() {/*
        if (xSpeed <= 10)
            accelerate(15);
        else
            accelerate(5);*/
    }
    private void moveForward() {
        transform.position += forwardDirection * Time.deltaTime * xSpeed * xDirection;
    }
    private void moveSideways()
    {
        axisSelector = new Vector3(Mathf.Abs(leftDirection.x), Mathf.Abs(leftDirection.y), Mathf.Abs(leftDirection.z));


        if (Mathf.Abs(Vector3.Dot(transform.position, axisSelector) - Vector3.Dot(axisSelector, targetLane)) > translationThreshold)
        {
            transform.position += axisSelector * Time.deltaTime * zSpeed * Mathf.Sign(Vector3.Dot(axisSelector, targetLane) - Vector3.Dot(axisSelector, transform.position));
            switchedLane = false;
        }
        else
            switchedLane = true;
    }
    private void determineExperience() {

        if (xTranslation) {
            experience = "X";
        }
        else {
            experience = "o";
        }

        if (zTranslation) {
            experience += "Z";
        }
        else {
            experience += "o";
        }

        if (yRotation) {
            experience += "R";
        }
        else {
            experience += "o";
        }
    }
    private void rotationMovement() {
        float diff = angles[targetAngleIndex] - transform.localEulerAngles.y;
        if (Mathf.Abs(diff) > yThreshold) {
            float rotationValue = rotationSpeed * Mathf.Sign(diff) * Time.deltaTime * (diff > 180 ? -1 : 1);
            transform.Rotate(Vector3.up * rotationValue);
        }
    }
    private void ChangeOfAccelerationByFrequency() {
        switchTime += Time.deltaTime;
        if (switchTime > timeBetweenAccelerations) {
            switch (experience) {
                case "Xoo":
                case "oZo": {
                        switchSpeed();
                        break;
                    }
                case "ooR": {
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
    private void updateMov() {
        switch (experience) {
            case "ooo": {
                    break;
                }
            case "Xoo":
            case "oZo": {
                    ChangeOfAccelerationByFrequency();
                    moveForward();
                    break;
                }
            case "ooR": {
                    //TODO: check time stamp before starting rotation
                    ChangeOfAccelerationByFrequency();
                    rotationMovement();
                    break;
                }
            case "XoR":
            case "oZR": {
                    ChangeOfAccelerationByFrequency();
                    rotationMovement();
                    moveForward();
                    break;
                }
            case "XZo": {
                    depassingMovement();
                    moveForward();
                    moveSideways();
                    break;
                }
            case "XZR": {
                    depassingMovement();
                    moveForward();
                    moveSideways();
                    rotationMovement();
                    break;
                }
            default:
                break;
        }
    }

    private void initializeExperiment() {
      
        switch (experience) {
            case "ooo": {
                    transform.localRotation = Quaternion.Euler(OOOStartRotation);
                    transform.position = OOOStartPosition;
                    break;
                }
            case "Xoo": {

                    transform.localRotation = Quaternion.Euler(XOOStartRotation);
                    transform.position = XOOStartPosition; 
                    break;
                }
            case "oZo": {

                    transform.localRotation = Quaternion.Euler(OZOStartRotation);
                    transform.position = OZOStartPosition;
                    break;
                }
            case "ooR": {

                    transform.localRotation = Quaternion.Euler(OORStartRotation);
                    transform.position = OORStartPosition;
                    break;
                }
            case "XoR": {

                    transform.localRotation = Quaternion.Euler(XORStartRotation);
                    transform.position = XORStartPosition;
                    XORAngles.CopyTo(angles, 0);
                    break;
                }

            case "oZR":{

                    transform.localRotation = Quaternion.Euler(OZRStartRotation);
                    transform.position = OZRStartPosition;
                    OZRAngles.CopyTo(angles, 0);
                    break;
                }

            case "XZo": {

                    transform.localRotation = Quaternion.Euler(XZOStartRotation);
                    transform.position = XZOStartPosition;
                    break;
                }
            case "XZR":{
                    transform.localRotation = Quaternion.Euler(XZRStartRotation);
                    transform.position = XZRStartPosition;
                    XZRAngles.CopyTo(angles, 0);
                    break;
                }
        }
        forwardDirection = transform.forward;
        leftDirection = new Vector3(forwardDirection.z, forwardDirection.y, -forwardDirection.x);
    }
    public void initializeExperimentPosition(){
        determineExperience();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        initializeExperiment();
        leftLanesT = gm.leftLanes;
        rightLanesT = gm.rightLanes;
        initializeLanes();
        targetLane = rightLane;
        targetAngleIndex = 0;
    }
    public void setPosition(Vector3 position){
        transform.position = position;
    }
    private void initializeLanes()
    {
        leftLane = leftLanesT[0].position;
        rightLane = rightLanesT[0].position;
        turnNumber = 0;
    }
    public string getExperience() {
        return experience;
    }
    public void startMoving() {
        canMove = true;
    }
    public void depassingMovement()
    {
        RaycastHit carHit;

        if (!isDepassing && Physics.Raycast(transform.position, forwardDirection, out carHit, 10.0f, LayerMask.GetMask("Obstacle")))
        {
            startDepassing();
            carToDepass = carHit.collider.gameObject;
        }

        if (isDepassing)
            xSpeed = 20;
        else
            xSpeed = 10;

        if ((switchedLane) && (isDepassing) && (Vector3.Distance(transform.position, carToDepass.transform.position) > 30.0f))
        {
            switchLane();
            isDepassing = false;
        }
    }
    public void startDepassing()
    {
        isDepassing = true;
        switchedLane = false;
        switchLane();
        switchAngle();
    }
}
