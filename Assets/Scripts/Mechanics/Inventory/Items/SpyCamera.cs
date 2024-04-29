using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyCamera : Item
{
    private void Start()
    {
        Name = gameObject.name;
        Description = $"Can be placed on walls and observed from the large screen.";
        Type = $"Recon";
    }
}