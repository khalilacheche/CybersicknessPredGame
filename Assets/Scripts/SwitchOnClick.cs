using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchOnClick : MonoBehaviour
{
    public enum SwitchType {
        Click, Hold
    }
    public bool initialState;
    public SwitchType switchType; 
    private bool state;
    public SteamVR_Input_Sources hand;
    public GameObject toSwitch;
    
    // Start is called before the first frame update
    void Start()
    {
        state = initialState;
        toSwitch.SetActive(state);
        SteamVR_Actions._default.GrabPinch.AddOnStateDownListener(handleDown, hand);
        SteamVR_Actions._default.GrabPinch.AddOnStateUpListener(handleUp,hand);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void handleDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        state = !state;
        toSwitch.SetActive(state);
    }
    public void handleUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        if(switchType == SwitchType.Click){
            return;
        }
        state =!state;
        toSwitch.SetActive(state);

    }
}
