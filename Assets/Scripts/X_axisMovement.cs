using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_axisMovement : MonoBehaviour
{
    private float maxspeed = 15.0f; //The max speed at which the player moves
    private float speed; //The max speed at which the player moves
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x <= -180) || (transform.position.x >= 50)){
            direction *= -1;
            if (speed < maxspeed)
                speed *= 1.5f;
        }
        transform.position += Vector3.left * Time.deltaTime * speed * direction;
    }
}
