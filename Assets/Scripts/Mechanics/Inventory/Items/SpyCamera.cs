using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyCamera : Item
{
    private void Start()
    {
        Name = "Spy Camera";
        Description = $"Can be placed on walls and observed from the large screen.";
        Type = $"Recon";
    }

    public override void Use(InteractionManager interactor = null)
    {
        base.Use();
        Debug.Log("Using Spy Camera");
        interactor.GetComponent<ThirdPersonShooterController>().PlaceItem(Name);
    }
}