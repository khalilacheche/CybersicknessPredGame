using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private float initialScale;
    public float maxHealth = 100;
    public float initialFireRate = 50;
    public float initialSmokeRate = 40;


    private  ParticleSystem firePS;
    private ParticleSystem smokePS;

    private GameManager gm;

    private float health;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        firePS = GetComponent<ParticleSystem>();
        smokePS = gameObject.GetComponentsInChildren<ParticleSystem>()[1];
        health = 0;
        var fireEmission =firePS.emission;
        fireEmission.rateOverTime = 0;
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = 0;
        
        
    }

    // Update is called once per frame
    void Update(){
        
        float fireRate = map(health, 0,maxHealth, 0, initialFireRate);
        var fireEmission =firePS.emission;
        fireEmission.rateOverTime = fireRate;
        float smokeRate = map(health, 0, maxHealth, 0 , initialSmokeRate);
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = smokeRate;
        
    }
    public void Reset (){
        health = maxHealth;
        dead = false;
    }
    private float map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    void OnTriggerStay(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Water")){
            health -= 1f;
            if(health < 0){
                if(!dead){
                    gm.MoveToNext();
                }
                dead = true;
                health = 0;
            }
        }

    }
}
