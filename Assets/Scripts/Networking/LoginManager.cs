using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using WebSocketSharp;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _emailInput;
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TMP_Text _nicknameText;

    [SerializeField] private GameObject _setNickNamePanel;

    private string _email = "";
    private string _password = "";
    private string _nickname = "";
    private bool rememberMe = true;

    public void SignUp()
    {
        _email = _emailInput.text;
        _password = _passwordInput.text;
        LootLockerSDKManager.WhiteLabelSignUp(_email, _password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");

                return;
            }

            Debug.Log("user created successfully");
        });
    }


    public void Login()
    {
        _email = _emailInput.text;
        _password = _passwordInput.text;
        LootLockerSDKManager.WhiteLabelLoginAndStartSession(_email, _password, rememberMe, response =>
        {
            if (!response.success)
            {
                if (!response.LoginResponse.success)
                {
                    Debug.Log("error while logging in");
                }
                else if (!response.SessionResponse.success)
                {
                    Debug.Log("error while starting session");
                }
                return;
            }
            //Ignore the GetNickName function
            else if(response.statusCode == 200)
            {
                LootLockerSDKManager.GetPlayerName((response) =>
                {
                    if (response.success)
                    {
                        _nickname = response.name;
                        Debug.Log("Successfully retrieved player name: " + response.name);
                        Debug.Log($"Nickname is {_nickname}");

                        if (string.IsNullOrEmpty(_nickname))
                        {
                            //There is no username yet. Send the player to set a username
                            _setNickNamePanel.SetActive(true);
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            StartGame();
                        }
                    }
                    else
                    {
                        Debug.Log("Error getting player name");
                    }
                });
            }
        });
    }


    public void StartGame()
    {
        Debug.Log($"Nickname is {_nickname}");
        PlayerManager.Instance.Nickname = _nickname;
        SceneManager.LoadScene("Matchmaking");
    }

    public void SetNickName()
    {
        _nickname = _nicknameInput.text;
        //Set player's username
        LootLockerSDKManager.SetPlayerName(_nickname, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
            }
            else
            {
                Debug.Log("Error setting player name");
            }
        });
    }
    public string GetNickName()
    {
        string nickName = "";
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                nickName = response.name;
            }
            else
            {
                Debug.Log("Error getting player name");
            }
        });

        return nickName;
    }

    public void ResetPassword()
    {
        _email = _emailInput.text;
        _nicknameText.text = "";

        LootLockerSDKManager.WhiteLabelRequestPassword(_email, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Error requesting password reset. Please confirm your internet connection");
                _nicknameText.text = "Error requesting password reset. Please confirm your internet connection";

                return;
            }
            Debug.Log("requested password reset successfully");
            _nicknameText.text = "A password reset link has been sent to the email provided";
        });
    }
}
