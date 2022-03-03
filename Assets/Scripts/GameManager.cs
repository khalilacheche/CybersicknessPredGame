using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Fire> fires;
    private Fire currentFire;
    private Fire previousFire;

    private GameObject player;

    private bool started = false;
    void Start(){
        currentFire = null;
        previousFire = null;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        if(!started){
            started = true;
            fires[0].Reset();

        }
        
    }

    public void MoveToNext(){
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
        next.Reset();
    }
}
