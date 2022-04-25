using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : Action
{
    private TutorialFire fire;
    public MeshRenderer text;
    public GameObject image;
    // Start is called before the first frame update
    void  Start(){
        text.enabled = false;
        image.SetActive(false);
        manager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
        fire = GetComponent<TutorialFire>();
        fire.reset();
    }

    // Update is called once per frame
    void Update()
    {
        if(fire.isDead()){
            image.SetActive(false);
            endAction();
            enabled = false;
            text.enabled = false;

        }
    }
    public override void startAction()
    {
        image.SetActive(true);
        text.enabled = true;
        fire.reset();
    }
}
