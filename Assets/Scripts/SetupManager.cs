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


                DataTrackerObject.GetComponent<LSLEventMarker>().PushData("SETUP_END",5);

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

    
    Rect windowRect = new Rect(20, 20, 300 , 300);
    Rect playerParametersWindow = new Rect(Screen.width - 300 - 20, 20, 300 , 300);


    void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.skin.toggle.fontSize = 30;
        GUI.skin.button.fontSize = 30;

        // Register the window. Notice the 3rd parameter
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "Params");
        playerParametersWindow = GUI.Window(1, playerParametersWindow, DoPlayerParamWindow, "Player Params");

        
        int left = Screen.width / 2 - 100/ 2;
        int top = Screen.height - 20 - 100;

        if (GUI.Button(new Rect(left, top, 100, 30), "Start"))
        {
            setupComplete = true;
        }
        if (setupComplete)
        {
            GUI.Label(new Rect(left, Screen.height / 2 - 10, 100, 100), ((int)timer).ToString());
        }
    }


    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        skipTutorial = GUILayout.Toggle(skipTutorial, "Skip Tuto");
        if (GUI.Button(new Rect(10, 100, 200, 100), "Calibration"))
        {
            startCalibration = true;
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        

    }

    void DoPlayerParamWindow(int windowID)
    {
        Player.GetComponent<PlayerParameters>().rightHanded= GUILayout.Toggle(Player.GetComponent<PlayerParameters>().rightHanded, "Right Handed?");
        Player.GetComponent<MovementManager>().xTranslation = GUILayout.Toggle(Player.GetComponent<MovementManager>().xTranslation, "X Translation");
        Player.GetComponent<MovementManager>().zTranslation = GUILayout.Toggle(Player.GetComponent<MovementManager>().zTranslation, "Z Translation");
        Player.GetComponent<MovementManager>().yRotation = GUILayout.Toggle(Player.GetComponent<MovementManager>().yRotation, "Y Rotation");
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));


    }
}
