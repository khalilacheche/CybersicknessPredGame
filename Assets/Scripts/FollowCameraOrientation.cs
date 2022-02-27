using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraOrientation : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3( transform.localEulerAngles.x, -45 + camera.transform.localEulerAngles.y,transform.localEulerAngles.z);
    }
}
