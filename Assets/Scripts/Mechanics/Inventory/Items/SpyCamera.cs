using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpyCamera : Item
{
    [SerializeField] private GameObject _swivelHead;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _scrambledVideo;
    private void Start()
    {
        Name = "Spy Camera";
        Description = $"Can be placed on walls and observed from the large screen.";
        Type = $"Recon";
    }

    private void OnEnable()
    {
        transform.forward = Camera.main.transform.forward;
        _swivelHead.transform.right = -Camera.main.transform.forward;
        FindObjectOfType<SpyCameraManager>().AddSpyCamera(this);
        CanInteract = false;
    }

    public override void Use(InteractionManager interactor = null)
    {
        base.Use();
        Debug.Log("Using Spy Camera");
        interactor.GetComponent<ThirdPersonShooterController>().PlaceItem(Name);
    }

    public void TurnOn()
    {
        _camera.SetActive(true);
    }

    public void TurnOff()
    {
        _camera.SetActive(false);
    }

    public void Scramble()
    {
        _scrambledVideo.SetActive(true);
    }

    public void UnScramble()
    {
        _scrambledVideo.SetActive(false);
    }
}