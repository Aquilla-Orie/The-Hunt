using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerDeadUI;
    public GameObject gameOverUI;
    public GameObject gameWinUI;

    private static GameManagerScript _instance;

    public static GameManagerScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManagerScript>();
                if (_instance == null)
                {
                    Debug.LogError("GameManagerScript instance is not found in the scene!");
                }
            }
            return _instance;
        }
    }

    public void PlayerDead(string playerTag)
    {
        playerDeadUI.SetActive(true);

        if (playerTag == "Assassin")
        {
            Invoke("GameOver", 5f);
        }
        else if (playerTag == "Cop")
        {
            int aliveCopCount = 0;
            GameObject[] cops = GameObject.FindGameObjectsWithTag("Cop");
            foreach (GameObject cop in cops)
            {
                if (cop.activeInHierarchy) aliveCopCount++;
            }

            if (aliveCopCount == 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);

        Invoke("LoadLobbyScene", 10f);
    }

    public void Win()
    {
        gameWinUI.SetActive(true);

        Invoke("LoadLobbyScene", 10f);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void LoadLobbyScene()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadAssassinLoadoutScene()
    {
        //SceneManager.LoadScene("Loadoutstat");
    }

    public void LoadCopsLoadoutScene()
    {
        SceneManager.LoadScene("Loadoutstat");
    }

    public void LoadAssassinScene()
    {
        SceneManager.LoadScene("assasin");
    }

    public void LoadCopsSelectionScene()
    {
        SceneManager.LoadScene("cop selection");
    }

    public void LoadCopsScene()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;

            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player == localPlayer)
                {
                    PhotonNetwork.LoadLevel("hacker ability");
                    break;
                }
            }
        }
    }

    public void LoadGameLevelScene()
    {
        SceneManager.LoadScene("GameLevel");
        NetworkManager.Instance.SpawnPlayers();
    }
}
