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
    // Start is called before the first frame update
    void Start()
    {
        hasStarted=false;
        GameObject.FindGameObjectWithTag("Player")?.GetComponent<MovementManager>().setPosition(new Vector3(-160.5f,-1.7f,180));
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTutorial && !hasStarted){
            hasStarted= true;
            actions[0].startAction();
        }
        
    }
    public void actionEnded(){
        if(++currentAction<actions.Count){
            actions[currentAction].startAction();
        }else{
            SceneManager.LoadScene(2);
        }

    }
}
