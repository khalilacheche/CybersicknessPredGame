using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public bool rightHanded;
    public static bool isRightHanded = true;
    public discomfort_manager discomfort_Manager;
    public SwitchOnClick hoseSwitcher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isRightHanded = rightHanded;
        
    }
}
