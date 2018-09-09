using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class CheckUserDTO {
    public bool userFound;
    public int userID;
    public string response;
}
public class LoginToRegister : MonoBehaviour {

    public GameObject firstname;
    public GameObject lastName;
    public GameObject password;

    string userName;
    string userLastName;
    string userPassword;

    static string userIDUse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(firstname.GetComponent<InputField>().isFocused)
            {
                lastName.GetComponent<InputField>().Select();
            }

            if (lastName.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }

            if (password.GetComponent<InputField>().isFocused)
            {
                firstname.GetComponent<InputField>().Select();
            }
        }
    }

    
    public void Login()
    {
        userName = firstname.GetComponent<InputField>().text;
        userLastName = lastName.GetComponent<InputField>().text;
        userPassword = password.GetComponent<InputField>().text;
        checkUsername(userName, userLastName, userPassword);
    }

    public void checkUsername(string username, string userLastName, string password)
    {
        if (username == null && password == null)
        {
            //toast
            return;
        }

        if (username == null)
        {
            //toast
            return;
        }
        if(password == null)
        {
            //toast
            return;
        }
        StartCoroutine(checkUsernameApi(username, userLastName, password));

    }

    public void goToRegister()
    {
        SceneManager.LoadScene("Register");
    }

    IEnumerator checkUsernameApi(string username, string userLastName, string password)
    {

        WWWForm formData = new WWWForm();
        formData.AddField("userName", username);
        formData.AddField("userLastName", userLastName);
        formData.AddField("userPassword", password);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:99/api/Users/Checkuser", formData);
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            CheckUserDTO checkUser = JsonUtility.FromJson<CheckUserDTO>(www.downloadHandler.text);
            if (checkUser.userFound)
            {
                Debug.Log("Found");
                userIDUse = checkUser.userID.ToString();
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.Log("Not Found");
            }
        }
    }

    public static string getUserIDUse()
    {
        return userIDUse;
    }

}
