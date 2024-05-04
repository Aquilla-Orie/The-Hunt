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
}
