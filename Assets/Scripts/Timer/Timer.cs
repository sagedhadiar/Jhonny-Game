using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timerText;
    private float startTime;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        float updateTime = Time.time - startTime;
        string minutes = ((int)updateTime / 60).ToString();
        string seconds = (updateTime % 60).ToString("f0");
        timerText.text = minutes + ":" + seconds;

    }
}
