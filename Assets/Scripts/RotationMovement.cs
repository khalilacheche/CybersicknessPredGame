using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{
    private int direction;
    private float angle;
    public float switchTimeInSeconds = 5.0f;

    public float maxSpeed = 60;
    float degreesPerSecond;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        direction = 1;
        degreesPerSecond = 20;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= switchTimeInSeconds)
        {
            direction *= -1;
            time = 0;
            if (degreesPerSecond<maxSpeed)
                degreesPerSecond *= 1.50f;
        }
        angle += degreesPerSecond*direction;
        transform.Rotate(Vector3.up * degreesPerSecond * direction * Time.deltaTime);
    }
}
