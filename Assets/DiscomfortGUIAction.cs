
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
public class DiscomfortGUIAction : Action
{
    private discomfort_manager discomfort_Manager;

    private float trackpad;
    private bool hasEnded;
    private bool hasStarted;
    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        hasEnded = false;
        manager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
        discomfort_Manager = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerParameters>().discomfort_Manager;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !hasEnded && Input.GetKeyDown("space"))
        {
            hasEnded = true;
            userEndedDiscomfortHandler();
        }
    }
    void userEndedDiscomfortHandler(){
        endAction();
        gameObject.SetActive(false);

    }
    
    public override void startAction()
    {
        discomfort_Manager?.enableDiscomfortGUI();
        hasEnded = false;
        hasStarted = true;
    }

}
