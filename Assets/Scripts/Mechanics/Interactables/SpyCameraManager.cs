using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyCameraManager : Interactable
{
    [SerializeField] private GameObject _spyCamera;
    [SerializeField] private GameObject _playerCamera;
    public override void Interact(InteractionManager interactor = null)
    {
        base.Interact();
        _spyCamera.SetActive(true);
        _playerCamera.SetActive(false);
    }
}
