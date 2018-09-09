using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[System.Serializable]
public class AddedScore
{
    public bool updated;
    public string response;
}

public class Score : MonoBehaviour {


    private static Score instance;

    public Text TimeGame;
    public Text Distance;
    public Text Coins;
    public Text Lives;
    public Text CurrentHealth;
    public Text EnemiesKilled;

    string timeGame { get; set; }
    string distance { get; set; }
    string coins { get; set; }
    string lives { get; set; }
    string currentHealth { get; set; }
    string enemiesKilled { get; set; }

    public static Score Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Score>();
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int  calculateScore()
    {
        timeGame = formatTime(TimeGame.text);
        distance = PlayerController.getDistanceToScore();
        coins = Coins.text;
        lives = Lives.text;
        currentHealth = CurrentHealth.text;
        enemiesKilled = EnemiesKilled.text;

        return 10;
    }

    public void sendSore()
    {
        StartCoroutine(uploadScore());
    }

    IEnumerator uploadScore()
    {

        WWWForm formData = new WWWForm();
        formData.AddField("timeGame", timeGame);
        formData.AddField("distance", distance);
        formData.AddField("coins", coins);
        formData.AddField("lives", lives);
        formData.AddField("currentHealth", currentHealth);
        formData.AddField("enemiesKilled", enemiesKilled);
        formData.AddField("userID", LoginToRegister.getUserIDUse());
        formData.AddField("newScore", calculateScore().ToString());

        UnityWebRequest addScore = UnityWebRequest.Post("http://localhost:99/api/Users/EnterScore", formData);
        yield return addScore.Send();

        if (addScore.isError)
        {
            Debug.Log(addScore.error);
        }
        else
        {
            AddedScore checkUser = JsonUtility.FromJson<AddedScore>(addScore.downloadHandler.text);
            Debug.Log(checkUser.response);
        }
    }

    public string formatTime(string time)
    {
        float count;
        string[] splitTime = time.Split(new[] { ":" }, StringSplitOptions.None);
        count = float.Parse(splitTime[0]) * 60 + float.Parse(splitTime[1]);
        return count.ToString(); 
    }
}
