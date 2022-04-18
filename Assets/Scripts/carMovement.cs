using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMovement : MonoBehaviour
{
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector3 axisSelector;
    private Vector3 targetLane;
    public int turnNumber;
    public Transform[] rightLanesT = new Transform[4];

    private float translationThreshold=0.1f;
    public float carSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        forwardDirection = transform.right;
        targetLane = rightLanesT[turnNumber].position;

    }

    // Update is called once per frame
    void Update()
    {
        
        rightDirection = new Vector3(-forwardDirection.z, -forwardDirection.y, forwardDirection.x);
        moveForward();
        moveSideways();
        transform.rotation = Quaternion.LookRotation(rightDirection, Vector3.up);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("turn"))
        {
            rotateAxis();
            turnNumber = (turnNumber + 1) % 4;
            targetLane = rightLanesT[turnNumber].position;
        }
    }
    private void rotateAxis()
    {
        forwardDirection = Quaternion.Euler(0, -90, 0) * forwardDirection;
    }
    private void moveForward()
    {
        transform.position += forwardDirection * Time.deltaTime * carSpeed ;
    }
    private void moveSideways()
    {
        axisSelector = new Vector3(Mathf.Abs(rightDirection.x), Mathf.Abs(rightDirection.y), Mathf.Abs(rightDirection.z));


        if (Mathf.Abs(Vector3.Dot(transform.position, axisSelector) - Vector3.Dot(axisSelector, targetLane)) > translationThreshold)
        {
            transform.position += axisSelector * Time.deltaTime * carSpeed  * Mathf.Sign(Vector3.Dot(axisSelector, targetLane) - Vector3.Dot(axisSelector, transform.position));
        }
    }
}
