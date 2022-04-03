using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchOnClick : MonoBehaviour
{
    public enum SwitchType {
        Click, Hold
    }
    public SwitchType switchType; 
    private bool state;
    public SteamVR_Input_Sources hand;
    public GameObject toSwitch;
    public bool inverse;
    private bool switchEnabled;
    private bool buttonState =false;
    // Start is called before the first frame update
    void Start()
    {
        
        state = false;
        toSwitch.SetActive(state);
        switchEnabled = true;
        SteamVR_Actions._default.GrabPinch.AddOnStateDownListener(handleDown, SteamVR_Input_Sources.LeftHand);
        SteamVR_Actions._default.GrabPinch.AddOnStateUpListener(handleUp,SteamVR_Input_Sources.LeftHand);
        SteamVR_Actions._default.GrabPinch.AddOnStateDownListener(handleDown, SteamVR_Input_Sources.RightHand);
        SteamVR_Actions._default.GrabPinch.AddOnStateUpListener(handleUp,SteamVR_Input_Sources.RightHand);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(switchType == SwitchType.Hold){
            bool newValue = switchEnabled & (buttonState ^ inverse);
            if(newValue != toSwitch.activeSelf){
                toSwitch.SetActive(newValue);
            }
        }
        
    }
    public void handleDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        
        hand = PlayerParameters.isRightHanded ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand; 

        if(fromSource != hand){
            return;
        }
        buttonState = true;
        if(!switchEnabled){
            return;
        }
        state = !state;
        if(switchType == SwitchType.Click){
            toSwitch.SetActive(state);

        }
    
    }
    public void handleUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        hand = PlayerParameters.isRightHanded ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand; 
        if(fromSource != hand){
            return;
        }
        buttonState = false;
        

    }
    public void enableSwitch(){
        switchEnabled = true;
    }
    public void disableSwitch(){
        switchEnabled = false;
    }
}
