using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MainMenu : MonoBehaviour {

    private bool loadScene = false;
    public string LoadingSceneName;
    public Text loadingText;
    public Slider sliderBar;

    public GameObject play;
    public GameObject options;
    public GameObject quit;

    string url; 

    public AudioMixer audioMixer;
	// Use this for initialization
	void Start () {
        //Hide Slider Progress Bar in start
        sliderBar.gameObject.SetActive(false);

        play.SetActive(true);
        options.SetActive(false);
        quit.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame() {
        play.SetActive(false);
        options.SetActive(false);
        quit.SetActive(false);
        updateLoadScene();
        //StartCoroutine(Upload());
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator Upload()
    {
    
        WWWForm formData = new WWWForm();
        formData.AddField("userName", "ahmad");
        formData.AddField("userLastName", "haidar");
        formData.AddField("userGender", "10");
        formData.AddField("userLocation", "beirut");
        formData.AddField("userNationality", "lebanese");
        formData.AddField("userPassword", "123");

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:99/api/Users/AddUser", formData);
        yield return www.Send();

        if (www.isError )
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    public void SetVolume(float volume) {

        //audioMixer.SetFloat("volume", volume);
    }

    public void updateLoadScene()
    {
        // If the player has pressed the space bar and a new scene is not loading yet...
        if (loadScene == false)
        {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            //Visible Slider Progress bar
            sliderBar.gameObject.SetActive(true);

            // ...change the instruction text to read "Loading..."
            loadingText.text = "Loading...";

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene(LoadingSceneName));

        }
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene(string sceneName)
    {

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            sliderBar.value = progress;
            //int value = (int)Mathf.Round(progress * 100f);
            loadingText.text = (int)Mathf.Round(progress * 100f) + "%";
            yield return null;

        }

    }

}
