using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    public float restoreAmount = 20f;
    private void Start()
    {
        Name = "Health Pack";
        Description = $"Restores a portion of health to the player";
        Type = $"Healing";
    }

    public override void Use(InteractionManager interactor = null)
    {
        base.Use();
        Debug.Log("Using Health pack");
        PlayerStats playerStats = interactor.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.RestoreHealth(restoreAmount);
            Debug.Log("Player health restored by healthpack");
        }
    }

}
