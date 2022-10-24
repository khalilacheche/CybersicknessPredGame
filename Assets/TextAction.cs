using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAction : Action
{
    [TextArea]
    public string text;
    public float duration = 5;
    private float timer;
    public bool append_timer;
    private bool hasStarted;
    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        gameObject.SetActive(false);
        manager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0){
            if (append_timer)
            {
                gameObject.GetComponent<TextMesh>().text = text + " "+ (int)timer + " seconds";
            }
            timer -= Time.deltaTime;
        } else {
            endAction();
            gameObject.SetActive(false);
        }
    }
    void OnGUI()
    {
        if (hasStarted && append_timer)
        {
            GUI.skin.label.fontSize = 50;
            int left = Screen.width / 2 - 100 / 2;
            GUI.Label(new Rect(left, Screen.height / 2 - 10, 100, 100), ((int)timer).ToString());
        }
    }
    override public void startAction(){
        gameObject.SetActive(true);
        gameObject.GetComponent<TextMesh>().text = text;
        timer = duration;
        hasStarted = true;
    }

}
