using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private int currentAction;
    public List<Action> actions;

    public bool startTutorial;

    private bool hasStarted;
    private LSLEventMarker eventMarker;
    // Start is called before the first frame update
    void Start()
    {
        hasStarted=false;
        GameObject.FindGameObjectWithTag("Player")?.GetComponent<MovementManager>().setPosition(new Vector3(-160.5f,-1.7f,181));
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().localRotation = Quaternion.Euler(0,90,0);
        eventMarker = GameObject.FindGameObjectWithTag("DataTracker").GetComponent<LSLEventMarker>();

    }

    // Update is called once per frame
    void Update()
    {
        if(startTutorial && !hasStarted){
            hasStarted= true;
            actions[0].startAction();

            eventMarker.PushData("TUTO_START");
        }
        
    }
    public void actionEnded(){
        if(++currentAction<actions.Count){
            actions[currentAction].startAction();
        }else{
            eventMarker.PushData("TUTO_END");
            SceneManager.LoadScene(2);
        }

    }
    void OnGUI()
    {

        int left = Screen.width / 2 - 50 / 2;
        int top = Screen.height - 20 - 50;

        if (GUI.Button(new Rect(left, top, 50, 20), "Start"))
        {
            startTutorial = true;
        }
        if (GUI.Button(new Rect(left, 20, 100, 20), "End tutorial"))
        {
            currentAction = actions.Count;
            actionEnded();
        }

    }
}
