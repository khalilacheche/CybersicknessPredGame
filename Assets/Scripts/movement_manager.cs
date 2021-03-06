/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_manager : MonoBehaviour
{

    public Vector3 translationAcceleration;
    public float rotationAcceleration;
    private Vector3 lastPosition;
    private Vector3 lastTranlsationSpeed;
    
    private Vector3 forwardDirection;
    
    private float lastAngle;
    private float lastRotationSpeed;

    public bool xTranslation = false;
    public bool zTranslation = false;
    public bool yRotation = false;
    
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
    public float timeBetweenRotations= 5;

    public float[] angles = {300,240,120,60};

    private float rotationSwitchTime = 0;

    private int targetAngleIndex;

    float degreesPerSecond = 20;
    public float rotationSpeed = 5;
    private float yThreshold = 1f;


    private GameManager gm;
    // Start is called before the first frame update
    void Start(){
        forwardDirection = transform.forward;
        
        gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        if (yRotation && !xTranslation && !zTranslation){
            transform.position = new Vector3(-108,-0.89f, 179.4f);
        }

        targetLane = rightLane;
        targetAngleIndex = 0;
        //Acceleration code 
        lastPosition = transform.position;
        lastAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void FixedUpdate(){

        if(xTranslation){
            if (transform.position.x < gm.laneStart){
                xDirection = -1;
                gm.randomizeObstaclePositions();

            } 
            if(transform.position.x > gm.laneEnd){
                xDirection = 1;
                gm.randomizeObstaclePositions();
            }
            transform.position += Vector3.left * Time.deltaTime * xSpeed * xDirection;
        }
        if(zTranslation){
            if(Mathf.Abs(transform.position.z - targetLane) > zThreshold){
                transform.position += Vector3.forward * Time.deltaTime * zSpeed * Mathf.Sign(targetLane - transform.position.z) ; 
            }
        }
        if(yRotation){
            rotationSwitchTime += Time.deltaTime;
            if(rotationSwitchTime> timeBetweenRotations){
                switchAngle();
                rotationSwitchTime = 0;
            }
            float diff = angles[targetAngleIndex] - transform.localEulerAngles.y;
            if(Mathf.Abs(diff) > yThreshold){
                
                float rotationValue =  rotationSpeed * Mathf.Sign(diff)  * Time.deltaTime * ( diff > 180 ? -1 : 1);
                transform.Rotate(Vector3.up *rotationValue);
            }
        }

        //acceleration code 
        Vector3 deltaP = transform.position - lastPosition;
        Vector3 deltaV = (deltaP/Time.deltaTime) - lastTranlsationSpeed;
        translationAcceleration = (deltaV / Time.deltaTime);
        lastPosition = transform.position;
        lastTranlsationSpeed = deltaP/Time.deltaTime;

        float deltaT = transform.localEulerAngles.y - lastAngle;
        float deltaTP = (deltaT/Time.deltaTime) - lastRotationSpeed;
        rotationAcceleration = (deltaTP / Time.deltaTime);
        lastAngle = transform.localEulerAngles.y;
        lastRotationSpeed = deltaT/Time.deltaTime;

        if(translationAcceleration.x>1f){
            Debug.Log("x acc: "+ translationAcceleration.x);
        }
        if(translationAcceleration.z> 1f){
            
            Debug.Log("z acc: "+ translationAcceleration.z);

        }
        if(Mathf.Abs(rotationAcceleration)>1f){
            
            Debug.Log("y rot: "+ rotationAcceleration);

        }
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
            switchLane(); 
        }
        if(col.gameObject.layer== LayerMask.NameToLayer("turn")){
            rotateAxis();    
        }
    }
    private void rotateAxis(){
        forwardDirection = Quaternion.Euler(0,-90.0f,0)* forwardDirection;
    }
    private void switchLane(){
        //value of targetLane is gonna be only the valueof rightLane or leftLane, so nit's okayto perform float equality
        if(targetLane == rightLane){
            targetLane = leftLane;
        }else{
            targetLane = rightLane;

        }
    }

    private void switchAngle(){
        targetAngleIndex = (targetAngleIndex + Random.Range(1,angles.Length)) % angles.Length;
    
    }
}
*/