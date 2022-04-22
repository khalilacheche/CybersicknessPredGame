﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    private GameObject player;

    private bool started = false;
    void Start(){
        experimentEnded = false;
        eventMarker = GameObject.FindGameObjectWithTag("DataTracker").GetComponent<LSLEventMarker>();
        eventMarker.PushData("EXP_START");
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

    private void PromptUser(){
        discommforGUIManager.enableDiscomfortGUI();
    }

    void Update(){
        if (experimentEnded)
        {
            Time.timeScale = 0;
            return;
        }
        if (forceQuit)
        {
            eventMarker.PushData("EXP_END_FORCEQUIT");
            experimentEnded = true;
        }

        timer += Time.deltaTime;
        if(timer>= EXPERIMENT_DURATION_TIME)
        {
            Debug.Log("Experiment Ended");
            eventMarker.PushData("EXP_END_TIME");
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

        if (GUI.Button(new Rect(left, top, 200, 50), "Force Quit"))
        {
            forceQuit = true;
        }
        GUI.Label(new Rect(left, Screen.height / 2 - 10, 100, 100), ((int)(timer/60)).ToString()+":"+(int)(timer % 60));

    }
}
