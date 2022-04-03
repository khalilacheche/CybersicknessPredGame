using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAction : Action
{
    public string text;
    public float duration = 5;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        manager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0){
            timer -= Time.deltaTime;
        }else{
            endAction();
            gameObject.SetActive(false);
        }
    }

    override public void startAction(){
        gameObject.SetActive(true);
        gameObject.GetComponent<TextMesh>().text = text;
        timer = duration;
    }

}
