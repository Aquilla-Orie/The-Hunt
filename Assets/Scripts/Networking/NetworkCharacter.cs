using Photon.Pun;
using StarterAssets;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;

public class NetworkCharacter : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    [SerializeField] private ThirdPersonController _thirdPersonController;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log($"Instantiated {name}");

        info.Sender.TagObject = gameObject;
        _thirdPersonController.DisableOtherCameras();
    }
}
