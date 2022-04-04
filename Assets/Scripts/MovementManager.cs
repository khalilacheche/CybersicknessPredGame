using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    private bool canMove = false;
    private bool switchedLane = true;
    private bool isDepassing = false;
    public Vector3 translationAcceleration;
    public float rotationAcceleration;
    private Vector3 lastPosition;
    private Vector3 lastTranlsationSpeed;
    private GameObject carToDepass;
    public Vector3 forwardDirection;
    public Vector3 leftDirection;
    private Vector3 leftLane;
    private Vector3 rightLane;
    private int turnNumber;
    private float lastAngle;
    private float lastRotationSpeed;
    private float switchTime = 0;
    public float timeBetweenAccelerations = 5;
    public Vector3 axisSelector;


    public bool xTranslation = false;
    public bool zTranslation = false;
    public bool yRotation = false;

    private string experience;

    [Header("stale Experience Parameters")]
    public Vector3 stalePosition = new Vector3(-160.5f, -2.5f, 168.7f);
    [Header("Rotation Experience Parameters")]
    public Vector3 crossingPosition = new Vector3(-112, -0.89f, 180.9f);
    [Header("X or Z  w/o R Experiences Parameters")]
    public Vector3 startPosition = new Vector3(-57, -1.7f, 180.9f);

    [Header("X and Z  w/o R Experiences Parameters")]
    public Vector3 startPositionXZ = new Vector3(57, -1.7f, 186.5f);


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

    public float[] angles = { 300, 240, 120, 60 };



    private int targetAngleIndex;

    float degreesPerSecond = 20;
    public float rotationSpeed = 5;
    private float yThreshold = 1f;


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
    }

    private void OnTriggerEnter(Collider col) {
        /*if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            switchLane();
        }
        */
        if (col.gameObject.layer == LayerMask.NameToLayer("turn")) {
            rotateAxis();
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("turnaround")) {
            xDirection *= -1;
        }
        /*if(col.gameObject.layer== LayerMask.NameToLayer("carBehind")){
            switchLane();
            accelerate(10);
        }*/
        /*
        if(col.gameObject.layer== LayerMask.NameToLayer("carFront")){
            switchLane();
            accelerate(-10);
        }
        */
    }
    private void switchLane() {
        //value of targetLane is gonna be only the valueof rightLane or leftLane, so it's okay to perform float equality
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
    private void switchSpeed() {
        if (xSpeed <= 10)
            accelerate(15);
        else
            accelerate(5);
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

    private void initializePos() {
        transform.localRotation= Quaternion.Euler(0,270,0);
        forwardDirection = transform.forward;
        leftDirection = new Vector3(forwardDirection.z, forwardDirection.y, -forwardDirection.x);
    

        switch (experience) {
            case "ooo": {
                    transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                    transform.position = stalePosition;
                    break;
                }
            case "ooR": {
                    transform.position = crossingPosition;
                    break;
                }
            case "XoR":
            case "Xoo": {
                    transform.position = startPosition;
                    break;
                }
            case "oZR":
            case "oZo": {
                    transform.position = startPosition;
                    transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                    break;
                }
            default: {
                    transform.position = startPositionXZ;
                    break;
                }
        }
    }
    public void initializeExperimentPosition(){
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        initializePos();
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
            Debug.Log("car found ___________");
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
        /*RaycastHit turnHit;
        if (Physics.Raycast(transform.position, forwardDirection, out turnHit, 40.0f, LayerMask.GetMask("turn")))
        {
            if (isDepassing)
            {
                xSpeed = 7;
            }
        }*/

        /*public void canDepass()
        {
            RaycastHit carHit;
            if (Physics.Raycast(transform.position, forwardDirection, out carHit, 20.0f, LayerMask.GetMask("Obstacle")))
            {

                RaycastHit turnHit;
                if (Physics.Raycast(transform.position, forwardDirection, out turnHit, 100.0f, LayerMask.GetMask("noDepassingZone")))
                {
                    Debug.Log(turnHit.distance + "-------------" + carHit.distance);
                    //if (2 * carHit.distance > turnHit.distance)
                    {
                        xSpeed = 7;
                        Debug.Log(xSpeed);
                        return;
                    }
                }
            }
            if (xSpeed <= 10)
            {
                xSpeed = 10;
            }
            Debug.Log(xSpeed);
        }
        */
    }
    public void startDepassing()
    {
        isDepassing = true;
        switchedLane = false;
        switchLane();
        switchAngle();
    }
}
