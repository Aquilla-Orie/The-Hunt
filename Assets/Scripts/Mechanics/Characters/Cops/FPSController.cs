using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections.Specialized;
using Photon.Pun;

public class FPSController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    //[SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnBulletPosition;
    //[SerializeField] private Transform vfxHitGreen;
    //[SerializeField] private Transform vfxHitRed;
    [SerializeField] private Animator animator;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Vector3 mouseWorldPosition;

    void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
            }

            if (starterAssetsInputs.aim)
            {
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);

                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

                //Only allow player to shoot when aiming
                if (starterAssetsInputs.shoot)
                {
                    // Call RPC_Shoot with the mouseWorldPosition as a parameter
                    photonView.RPC("RPC_Shoot", RpcTarget.All, mouseWorldPosition);
                }
            }
            else
            {
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);

                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * 10f));
                starterAssetsInputs.shoot = false;

            }


        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 mousePosition)
    {
        Vector3 aimDirection = (mousePosition - spawnBulletPosition.position).normalized;
        PhotonNetwork.Instantiate(bulletPrefab.name, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        starterAssetsInputs.shoot = false;
    }
}
