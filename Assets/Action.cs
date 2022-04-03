using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    protected TutorialManager manager;

    // Update is called once per frame
    public abstract void startAction();


    protected void endAction(){
        manager.actionEnded();
    }
    
}
