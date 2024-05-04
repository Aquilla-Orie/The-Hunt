using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Item
{
    private void Start()
    {
        Name = "Health Pack";
        Description = $"Restores a portion of health to the player";
        Type = $"Healing";
    }

    public override void Use()
    {
        base.Use();
        Debug.Log("Using Health pack");
    }

    public float restoreAmount = 20f;

    public override void Interact(InteractionManager manager = null)
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
