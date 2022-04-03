using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFire : MonoBehaviour
{
    private float initialScale;
    public float maxHealth = 100;
    public float healthThreshhold = 10;
    public float initialFireRate = 50;
    public float initialSmokeRate = 40;


    private  ParticleSystem firePS;
    private ParticleSystem smokePS;

    public float health;
    private bool dead;
    private bool fireEnabled;

    void Start(){
        firePS = GetComponent<ParticleSystem>();
        smokePS = gameObject.GetComponentsInChildren<ParticleSystem>()[1];
        var fireEmission =firePS.emission;
        fireEmission.rateOverTime = 0;
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = 0;
        health= 0;
        fireEnabled = false;
    }
    void Update(){
        
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
        fireEnabled = true;
        
    }
    private float map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    void die(){
        //this.gameObject.SetActive(false);
        dead = true;
        health = 0;
    }
    public bool isDead(){
        return dead;
    }
    void OnTriggerStay(Collider other){
        if(!fireEnabled){
            return;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Water")){
            health -= 0.2f;
            if(health <= healthThreshhold){
                die();
            }
        }

    }
}
