using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private float initialScale;
    public float maxHealth = 100;
    private float health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        initialScale = transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        float scale = map(health, 0,maxHealth, 0, initialScale);
        transform.localScale =Vector3.one * scale;
        
    }
    private float map (float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
        void OnTriggerStay(Collider other)
    {
        health -= 1;
    }
}
