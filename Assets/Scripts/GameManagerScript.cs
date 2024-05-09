using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerDeadUI;
    public GameObject gameOverUI;
    public GameObject gameWinUI;

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

    public void LoadGameLevelScene()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
