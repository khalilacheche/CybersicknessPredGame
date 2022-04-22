/* Author: Phil Lopes on 28 of August 2019
    Modification: Khalil Acheche on 28 ofmarch 2022
  */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLPlayerMovementLogger : MonoBehaviour
{
    
    private float[] PlayerInfoData;

    // LabStreamingLayer Integration -- Gaze and Pupil Streamer
    private const string unique_source_id = "3469CD84-E3A4-4C1C-84F1-8003485B5200";

    public string lslStreamName = "Player_Game_Movement";
    public string lslStreamType = "Player_Data";

    private liblsl.StreamInfo lslStreamInfo;
    private liblsl.StreamOutlet lslOutlet;
    private const int lslChannelCount = 6;
    private MovementManager movementManager;
    private double nominal_srate = 60;
    private const liblsl.channel_format_t lslChannelFormat = liblsl.channel_format_t.cf_float32;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = GameObject.FindGameObjectWithTag("Player")?.GetComponent<MovementManager>();
        PlayerInfoData = new float[lslChannelCount];
        lslOutlet = KickStartPlayerLSLStream();
    }

    private void FixedUpdate()
    {
        Vector3 translation = movementManager.translationAcceleration;
        Vector3 rotation = movementManager.rotationAcceleration;
        // Get Player Data
        PlayerInfoData[0] = translation.x;
        PlayerInfoData[1] = translation.y;
        PlayerInfoData[2] = translation.z;
        PlayerInfoData[3] = rotation.x;
        PlayerInfoData[4] = rotation.y;
        PlayerInfoData[5] = rotation.z;
        lslOutlet.push_sample(PlayerInfoData);
        /*if(translation.magnitude > 1)
            Debug.Log(translation);
        */

        PlayerInfoData = new float[lslChannelCount];
    }

    liblsl.StreamOutlet KickStartPlayerLSLStream()
    {
        lslStreamInfo = new liblsl.StreamInfo(
            lslStreamName,
            lslStreamType,
            lslChannelCount,
            nominal_srate,
            lslChannelFormat,
            unique_source_id);
        return new liblsl.StreamOutlet(lslStreamInfo);
    }

}