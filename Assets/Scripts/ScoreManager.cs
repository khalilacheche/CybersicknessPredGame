using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private GameManager gm;
    void Update()
    {
        if(gm != null){
            scoreText.text = "score: " + gm.getScore().ToString();
        }else{
            GameObject gmGO =GameObject.Find("GameManager");
            if(gmGO != null){
                gm = gmGO.GetComponent<GameManager>();
            } 

        }
    }
}
