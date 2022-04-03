using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
public class discomfort_manager : MonoBehaviour
{
    public delegate void Callback();
    private Slider slider;

    private Text text;

    public float discomfort;

    private float MAX_DISCOMFORT = 20;
    public SteamVR_Input_Sources Hand;

    private float trackpad;

    private bool hasSelected;

    private Callback callback;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentsInChildren<Text>()[1];
        SteamVR_Actions._default.Menu.AddOnStateUpListener(handleUp, SteamVR_Input_Sources.LeftHand);
        SteamVR_Actions._default.Menu.AddOnStateUpListener(handleUp, SteamVR_Input_Sources.RightHand);

        //gameObject.SetActive(false);
        
    }
    public void handleUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        Hand = PlayerParameters.isRightHanded ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        if(fromSource != Hand){
            return;
        }
        if(!hasSelected){
            hasSelected = true;
            player?.GetComponent<PlayerParameters>().hoseSwitcher.enableSwitch();
            if(callback != null){
                callback();
            }
            gameObject.SetActive(false);
            //notify
        }
    }
    

    // Update is called once per frame
    void Update()
    {

        trackpad = SteamVR_Actions._default.Move.GetAxis(Hand).x;
        discomfort += trackpad * 0.1f;
        discomfort = Mathf.Clamp(discomfort,0,MAX_DISCOMFORT);
        slider.value = discomfort/MAX_DISCOMFORT;
        text.text  = ""+(int)discomfort; 
        
    }
    public float getDiscomfort(){
        return discomfort;
    }

    void OnEnable(){
        discomfort = MAX_DISCOMFORT/2;
        player = GameObject.FindGameObjectWithTag("Player");
        player?.GetComponent<PlayerParameters>().hoseSwitcher.disableSwitch();        
        gameObject.GetComponent<AudioSource>().Stop();
        gameObject.GetComponent<AudioSource>().Play();
        hasSelected =false;
    }
    public void enableDiscomfortGUI(Callback callback ){
        this.callback = callback;
        gameObject.SetActive(true);
    }
    public void disableDiscomfortGUI(){
        gameObject.SetActive(false);
        player?.GetComponent<PlayerParameters>().hoseSwitcher.enableSwitch();

    }


}
