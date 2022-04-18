using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
public class SetupManager : MonoBehaviour
{
   private bool calibrationSuccess = false;
   [Header("Calibration")]
   public bool startCalibration = false;
   
   public float timer = 10;
   public TextMesh text;
   [Header("Don't Destroy On Load")]
   public GameObject SRAnipalObject;
   public GameObject DataTrackerObject;
   public GameObject Player;
    [Header("Skip Tutorial")]
   public bool skipTutorial = false;

   [Header("Setup Complete")]
   public bool setupComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        startCalibration = false;
        Application.targetFrameRate = 60;
        while (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
        {
            // Do Nothing
        }
        Player.GetComponent<MovementManager>().setPosition(Vector3.zero);
        DataTrackerObject.GetComponent<LSLEventMarker>().PushData("SETUP_START");

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


                DataTrackerObject.GetComponent<LSLEventMarker>().PushData("SETUP_END");

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
        if(skipTutorial){
            SceneManager.LoadScene(2);

        }else{
            SceneManager.LoadScene(1);
        }
    }
}
