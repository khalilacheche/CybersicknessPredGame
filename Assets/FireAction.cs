using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : Action
{
    private TutorialFire fire;
    // Start is called before the first frame update
    void  Start(){
        manager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
        fire = GetComponent<TutorialFire>();
        fire.reset();
    }

    // Update is called once per frame
    void Update()
    {
        if(fire.isDead()){
            
            endAction();
            enabled = false;
        }
    }
    public override void startAction()
    {
        fire.reset();
    }
}
