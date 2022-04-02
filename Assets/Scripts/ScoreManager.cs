using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Slider fireSlider;
    private GameManager gm;
    // Start is called before the first frame update
    void OnEnable(){
    }

    // Update is called once per frame
    void Update()
    {
        if(gm != null){
            scoreText.text = "score: " + gm.getScore().ToString();
            fireSlider.value = gm.getFireIntensity();
        }else{
            GameObject gmGO =GameObject.Find("GameManager");
            if(gmGO != null){
                gm = gmGO.GetComponent<GameManager>();
            } 

        }
    }
}
