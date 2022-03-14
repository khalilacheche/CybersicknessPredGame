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
    void Start(){
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "score: " + gm.getScore().ToString();
        fireSlider.value = gm.getFireIntensity();
    }
}
