﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    private int score;

    private GameObject[] obstacles;
    private GameObject[] turns;
    private GameObject[] turnarounds;

    private bool experimentEnded;

    private  Fire [] fires;
    private Fire currentFire;
    private Fire previousFire;
    private int numActiveFires = 0;
    private discomfort_manager discommforGUIManager;
    public bool forceQuit;

    public float timeScale = 1;
    public Transform [] leftLanes = new Transform[4];
    public Transform [] rightLanes = new Transform[4];

    public static float EXPERIMENT_DURATION_TIME = 20 * 60; //Duration in seconds
    public float timer;

    private LSLEventMarker eventMarker;

    private Text playerText;

    private GameObject player;

    private bool started = false;
    void Start(){
        experimentEnded = false;
        eventMarker = GameObject.FindGameObjectWithTag("DataTracker").GetComponent<LSLEventMarker>();
        eventMarker.PushData("EXP_START",1);
        timer = 0;
        score = 0;
        InvokeRepeating("PromptUser", 60, 60);
        currentFire = null;
        previousFire = null;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MovementManager>().initializeExperimentPosition();
        player.GetComponent<MovementManager>().startMoving();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        turnarounds = GameObject.FindGameObjectsWithTag("turnaround");
        turns = GameObject.FindGameObjectsWithTag("turn");
        discommforGUIManager = player.GetComponent<PlayerParameters>().discomfort_Manager;
        playerText = player.GetComponent<PlayerParameters>().text;
        playerText.text = "Start!";
        playerText.gameObject.SetActive(true);
        StartCoroutine(disableAfterTime(1.5f, playerText.gameObject));

        GameObject[] firesAsGO = GameObject.FindGameObjectsWithTag("Fire");
        fires = new Fire [firesAsGO.Length];
        //numActiveFires = fires.Length;
        for (int i = 0; i < firesAsGO.Length; i++)
        {
            fires[i] = firesAsGO[i].GetComponent<Fire>();
        }
        switch (player.GetComponent<MovementManager>().getExperience())
        {
            case "XZo":
            case "XZR":
                {
                    setTurns(true);
                    setTurnarounds(false);
                    setAllObstacles(true);
                    break;
                }
            default:
                {
                    setTurns(false);
                    setTurnarounds(true);
                    setAllObstacles(false);
                    break;
                }
        }
    }
    IEnumerator disableAfterTime(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        
        go.SetActive(false);
    }

    private void PromptUser(){
        discommforGUIManager.enableDiscomfortGUI();
    }

    void Update(){
        if (experimentEnded)
        {
            Time.timeScale = 0;
            if (!playerText.gameObject.activeSelf){
                playerText.text = "Thank you for your time!";
                playerText.gameObject.SetActive(true);
            }
            return;
        }
        if (forceQuit)
        {
            eventMarker.PushData("EXP_END_FORCEQUIT",2);
            experimentEnded = true;
        }

        timer += Time.deltaTime;
        if(timer>= EXPERIMENT_DURATION_TIME)
        {
            Debug.Log("Experiment Ended");
            eventMarker.PushData("EXP_END_TIME",3);
            experimentEnded = true;


        }
        Time.timeScale = timeScale;

    }
    private void setAllObstacles(bool value){
        foreach(GameObject obstacle in obstacles){
            obstacle.SetActive(value);
        }
    }
    private void setTurns(bool value){
        foreach(GameObject turn in turns){
            turn.SetActive(value);
        }
    }
    private void setTurnarounds(bool value){
        foreach(GameObject turnaround in turnarounds){
            turnaround.SetActive(value);
        }
    }
    public void notifyDeadFire(){
        score++;
        numActiveFires --;
    }
    public void notifyRebirthFire(){
        numActiveFires++;
    }
    public int getScore(){
        return score;
    }
    public float getFireIntensity(){
        return ((float)numActiveFires)/fires.Length;
    }
    void OnGUI()
    {

        int left = Screen.width / 2 - 50 / 2;
        int top = Screen.height - 20 - 50;

        if (GUI.Button(new Rect(left, top, 200, 100), "Force Quit"))
        {
            forceQuit = true;
        }
        if (GUI.Button(new Rect(left, 20, 200, 100), "End Game"))
        {
            timer = EXPERIMENT_DURATION_TIME;
        }
        GUI.Label(new Rect(left, Screen.height / 2 - 10, 100, 100), ((int)(timer/60)).ToString()+":"+(int)(timer % 60));

    }
}
