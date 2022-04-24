using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : Action
{
    private TutorialFire fire;
    public MeshRenderer text;
    // Start is called before the first frame update
    void  Start(){
        text.enabled = false;
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
            text.enabled = false;

        }
    }
    public override void startAction()
    {
        text.enabled = true;
        fire.reset();
    }
}
