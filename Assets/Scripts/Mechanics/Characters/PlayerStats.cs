using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using LootLocker.Requests;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerStats : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth = 100f;
    public float currentHealth;

    int score = 0;

    private string globalDeathsLeaderboardKey = "globalDeaths";

    Renderer[] visuals;

    public HealthBar healthBar;
    public GameManagerScript gameManager;
    public Leaderboard leaderboard;

    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        leaderboard = FindObjectOfType<Leaderboard>();
        visuals = GetComponentsInChildren<Renderer>();

        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.SetSlider(currentHealth);
    }

    public void Die()
    {
        VisualiseRenderers(false);
        GetComponent<CharacterController>().enabled = false;

        Transform playerHUDCanvasTransform = transform.Find("PlayerHUDCanvas");
        if (playerHUDCanvasTransform != null)
        {
            GameObject playerHUDCanvas = playerHUDCanvasTransform.gameObject;
            playerHUDCanvas.SetActive(false);
        }

        if (photonView.IsMine)
        {
            leaderboard.SubmitScore(++score, globalDeathsLeaderboardKey);

            gameManager.PlayerDead(gameObject.tag);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }

    void VisualiseRenderers(bool state)
    {
        foreach (var renderer in visuals)
        {
            renderer.enabled = state;
        }
    }
}
