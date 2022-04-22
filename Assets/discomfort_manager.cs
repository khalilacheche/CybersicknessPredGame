using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
public class discomfort_manager : MonoBehaviour
{
    public delegate void Callback();
    private Callback callback;
    public float disableTime = 7;
    void OnEnable()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void enableDiscomfortGUI(Callback callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);
        StartCoroutine(disableAfterTime(disableTime));
    }
    public void enableDiscomfortGUI()
    {
        enableDiscomfortGUI(null);
    }

    IEnumerator disableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (callback != null) {
            callback();
        }
        gameObject.SetActive(false);
    }


}
