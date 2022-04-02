using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
public class Manager : MonoBehaviour
{
   private bool calibrationSuccess = false;
   public bool startCalibration = false;
   public bool setupComplete = false;
   public float timer = 10;
   public TextMesh text;

   public GameObject SRAnipalObject;
   public GameObject DataTrackerObject;
   public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        startCalibration = false;
        Application.targetFrameRate = 60;
        while (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
        {
            // Do Nothing
        }

        calibrationSuccess = false;
    }


    private void Update()
    {
        if (!calibrationSuccess && startCalibration)
        {
            calibrationSuccess = SRanipal_Eye.LaunchEyeCalibration();
        }
        else if(startCalibration)
        {
            Debug.Log("Eye calibration complete");
        }
        if (setupComplete)
        {


            text.text = "The experiment will start in\n" + (int)timer + " Seconds";
            timer -= Time.deltaTime;

            if(timer <= 0f)
            {
                text.gameObject.SetActive( false);
                
                

                KickStartExperiment();
            }
        }
    }
    void KickStartExperiment()
    {
        // Make Sure Objects are not Destroyed
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(SRAnipalObject);
        DontDestroyOnLoad(DataTrackerObject);

        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = Color.clear;
        // Load New Scene
        SceneManager.LoadScene(1);
    }
}
