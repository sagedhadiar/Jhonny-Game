using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class AddedUserDTO
{
    public bool addedUser;
    public string response;
}

public class Register : MonoBehaviour {

    public GameObject firstname;
    public GameObject lastName;
    public GameObject password;
    public GameObject confirmPassword;
    public GameObject location;
    public GameObject nationality;
    public GameObject gender;
    public GameObject birthdate;

    string userName;
    string userLastName;
    string userPassword;
    string userconfirmPassword;
    string userLocation;
    string userNationality;
    string userGender;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (firstname.GetComponent<InputField>().isFocused)
            {
                lastName.GetComponent<InputField>().Select();
            }

            if (lastName.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }

            if (password.GetComponent<InputField>().isFocused)
            {
                confirmPassword.GetComponent<InputField>().Select();
            }
            if (confirmPassword.GetComponent<InputField>().isFocused)
            {
                nationality.GetComponent<InputField>().Select();
            }
            if (nationality.GetComponent<InputField>().isFocused)
            {
                location.GetComponent<InputField>().Select();
            }
            if (location.GetComponent<InputField>().isFocused)
            {
                gender.GetComponent<InputField>().Select();
            }
            if (gender.GetComponent<InputField>().isFocused)
            {
                birthdate.GetComponent<InputField>().Select();
            }
            if (birthdate.GetComponent<InputField>().isFocused)
            {
                firstname.GetComponent<InputField>().Select();
            }
        }
    }

    public void backToMain()
    {
        SceneManager.LoadScene("Login");
    }
    public void register()
    {
        userName = firstname.GetComponent<InputField>().text;
        userLastName = lastName.GetComponent<InputField>().text;
        userPassword = password.GetComponent<InputField>().text;
        userconfirmPassword = confirmPassword.GetComponent<InputField>().text;
        userNationality = nationality.GetComponent<InputField>().text;
        userGender = gender.GetComponent<InputField>().text;
        userLocation = location.GetComponent<InputField>().text;

        if(userName == null && userLastName == null && userPassword == null && userconfirmPassword == null && userNationality == null && userGender == null && userLocation == null)
        {
            return;
        }

        if(userPassword != userconfirmPassword)
        {
            Debug.Log("Does not match");
            return;
        }

        StartCoroutine(checkUsernameRegisterApi(userName, userLastName, userPassword, userNationality, userGender, userLocation));
    }

    IEnumerator checkUsernameRegisterApi(string username, string userLastName, string password, string userNationality, string userGender, string userLocation)
    {

        WWWForm formData = new WWWForm();
        formData.AddField("userName", username);
        formData.AddField("userLastName", userLastName);
        formData.AddField("userPassword", password);
        formData.AddField("userGender", userGender);
        formData.AddField("userNationality", userNationality);
        formData.AddField("userLocation", userLocation);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:99/api/Users/Checkuser", formData);
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //checking if user exists
            CheckUserDTO checkUser = JsonUtility.FromJson<CheckUserDTO>(www.downloadHandler.text);
            //User exists 
            if (checkUser.userFound)
            {
                
                Debug.Log("User Exists");
                
            }
            //Add user
            else
            {
                Debug.Log("formData ");
                www = UnityWebRequest.Post("http://localhost:99/api/Users/AddUser", formData);
                yield return www.Send();

                if (www.isError)
                {
                    Debug.Log("Error");
                    Debug.Log(www.error);
                }
                else
                {
                    AddedUserDTO user = JsonUtility.FromJson<AddedUserDTO>(www.downloadHandler.text);
                    if (user.addedUser)
                    {
                        Debug.Log("Added Succesfully");
                        SceneManager.LoadScene("Login");

                    }
                    else
                    {
                        Debug.Log("Wrong");
                    }
                }
            }
        }
    }

}
