using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStats : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth = 100f;
    public float currentHealth;

    Renderer[] visuals;

    public HealthBar healthBar;
    //public GameManager gameManager;

    void Start()
    {
        visuals = GetComponentsInChildren<Renderer>();

        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
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

    void Update()
    {
        if (currentHealth <= 0)
        {
            // Game over or respawn - can change in settings??
            //StartCoroutine(Respawn());

            Die();
        }

        // Update health bar UI across the network
       // photonView.RPC("RPC_UpdateHealth", RpcTarget.All, currentHealth);
    }

    void Die()
    {
        VisualiseRenderers(false);
        GetComponent<CharacterController>().enabled = false;
        //gameManager.GameOver();
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

    /*[PunRPC]
    private void RPC_UpdateHealth(float health)
    {
        currentHealth = health;
        healthBar.SetSlider(currentHealth);
    }*/

    // Adding Respawn for testing and optional feature

   /* IEnumerator Respawn()
    {
        VisualiseRenderers(false);
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(0, 10, 0);
        yield return new WaitForSeconds(1);
        GetComponent<CharacterController>().enabled = true;
        VisualiseRenderers(true);
    }*/

    void VisualiseRenderers(bool state)
    {
        foreach(var renderer in visuals)
        {
            renderer.enabled = state;
        }
    }
}
