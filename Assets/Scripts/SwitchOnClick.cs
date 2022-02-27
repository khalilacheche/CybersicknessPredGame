using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchOnClick : MonoBehaviour
{
    private bool state;
    public SteamVR_Input_Sources hand;
    public GameObject toDisable;
    
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        toDisable.SetActive(state);
        SteamVR_Actions._default.GrabPinch.AddOnStateDownListener(switchMapState, hand);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void switchMapState(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        state = !state;
        toDisable.SetActive(state);
    }
}
