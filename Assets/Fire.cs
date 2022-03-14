using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private float initialScale;
    public float maxHealth = 100;
    public float healthThreshhold = 10;
    public float initialFireRate = 50;
    public float initialSmokeRate = 40;


    private  ParticleSystem firePS;
    private ParticleSystem smokePS;
    private float deathTimeCounter =0;
    public float timeDeadSeconds = 30;

    private GameManager gm;

    private float health;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        firePS = GetComponent<ParticleSystem>();
        smokePS = gameObject.GetComponentsInChildren<ParticleSystem>()[1];
        var fireEmission =firePS.emission;
        fireEmission.rateOverTime = 0;
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = 0;
        reset();
        
        
    }

    // Update is called once per frame
    void Update(){
        if(dead){
            deathTimeCounter += Time.deltaTime;
        }
        if(deathTimeCounter> timeDeadSeconds){
            reset();
        }
        
        float fireRate = map(health, 0,maxHealth, 0, initialFireRate);
        var fireEmission =firePS.emission;
        fireEmission.rateOverTime = fireRate;
        float smokeRate = map(health, 0, maxHealth, 0 , initialSmokeRate);
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = smokeRate;
        
    }
    public void reset (){
        //this.gameObject.SetActive(true);
        health = maxHealth;
        dead = false;
        deathTimeCounter = 0;
        gm.notifyRebirthFire();
        
    }
    private float map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    void die(){
        //this.gameObject.SetActive(false);
        dead = true;
        health = 0;
        gm.notifyDeadFire();
    }
    void OnTriggerStay(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Water")){
            health -= 1f;
            if(health < healthThreshhold){
                if(!dead){
                    die();
                }
            }
        }

    }
}

