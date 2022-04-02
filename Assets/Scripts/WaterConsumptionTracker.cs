using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterConsumptionTracker : MonoBehaviour
{
    private WaterHoseManager hose;
    public Text numConsumedTanksText;

    private const float TANK_CAPACITY = 30;  
    private int consumedTanks;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject hoseGO = GameObject.FindGameObjectWithTag("WaterHose");
        if(hoseGO != null){
            hose = hoseGO.GetComponent<WaterHoseManager>();
        } 
        consumedTanks = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hose != null){
            consumedTanks =(int) (hose.getWaterConsumption() / TANK_CAPACITY);
            GetComponent<Slider>().value = 1 - ((hose.getWaterConsumption() - consumedTanks * TANK_CAPACITY)/TANK_CAPACITY);
            numConsumedTanksText.text = "x"+ consumedTanks;
        }
    }
}
