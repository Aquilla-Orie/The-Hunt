using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Interactable
{
    public float restoreAmount = 20f;

    public override void Interact()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null && playerStats.transform.parent.name == "PlayerAssassin")
        {
            playerStats.RestoreHealth(restoreAmount);
            Debug.Log("Player health restored by healthpack");
            Destroy(gameObject);
        }
    }
}
