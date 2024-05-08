using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpyCameraManager : Interactable
{
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<SpyCamera> _spyCameras;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _playerCameraCanvas;

    private int _cameraIndex = 0;

    private void Start()
    {
        _spyCameras = new List<SpyCamera>();
    }

    public override void Interact(InteractionManager interactor = null)
    {
        base.Interact();
        //_spyCamera.SetActive(true);
        _starterAssetsInputs = interactor.GetComponent<StarterAssetsInputs>();
        _playerInput = interactor.GetComponent<PlayerInput>();
        _playerInput.SwitchCurrentActionMap("Camera");
        _playerCameraCanvas.SetActive(true);
    }

    public void AddSpyCamera(SpyCamera camera)
    {
        _spyCameras.Add(camera);
    }

    private void Update()
    {
        if (_starterAssetsInputs == null) return;
        if (_starterAssetsInputs.exit)
        {
            _playerInput.SwitchCurrentActionMap("Player");
            _playerCameraCanvas.SetActive(false);
            _starterAssetsInputs.exit = false;
            _cameraIndex = 0;
        }
        if (_starterAssetsInputs.browse)
        {
            BrowseCameras();
            _starterAssetsInputs.browse = false;
        }
    }

    private void BrowseCameras()
    {
        if(_spyCameras.Count <= 0) return;
        foreach (SpyCamera camera in _spyCameras)
        {
            camera.TurnOff();
        }
        _cameraIndex++;

        _spyCameras[_cameraIndex % _spyCameras.Count].TurnOn();
    }
}
