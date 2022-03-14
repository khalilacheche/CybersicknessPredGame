using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private int score;

    private GameObject[] obstacles;
    private  Fire [] fires;
    private Fire currentFire;
    private Fire previousFire;
    private int numActiveFires = 0;


    private GameObject player;

    private bool started = false;
    void Start(){
        score = 0;
        
        currentFire = null;
        previousFire = null;
        player = GameObject.FindGameObjectWithTag("Player");
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] firesAsGO = GameObject.FindGameObjectsWithTag("Fire");
        fires = new Fire [firesAsGO.Length];
        //numActiveFires = fires.Length;
        for (int i = 0; i < firesAsGO.Length; i++)
        {
            fires[i] = firesAsGO[i].GetComponent<Fire>();
        }
    }

    void Update(){
        if(player.GetComponent<movement_manager>().zTranslation){
            setAllObstacles(true);
        }else{
            setAllObstacles(false);

        }
        /*
        if(!started){
            started = true;
            if(fires.Length>0)
            {fires[0].reset();}

        }*/
        
    }
    private void setAllObstacles(bool value){
        foreach(GameObject obstacle in obstacles){
            obstacle.SetActive(value);
        }
    }
    public void notifyDeadFire(){
        score++;
        numActiveFires --;
    }
    public void notifyRebirthFire(){
        numActiveFires++;
    }
    /*
    public void MoveToNext(){
        score++;
        //look for closest fire to start, according to player direction
        float minDistance = float.MaxValue;
        Fire next = null;
        foreach( Fire fire in fires){
            float distance = Vector3.Distance(fire.gameObject.transform.position, player.transform.position);
            if( distance < minDistance && fire != currentFire && fire != previousFire){
                next = fire;
                minDistance = distance;   
            }  
        }
        previousFire = currentFire;
        currentFire = next;
        next.reset();
    }*/
    public int getScore(){
        return score;
    }
    public float getFireIntensity(){
        return ((float)numActiveFires)/fires.Length;
    }
}
