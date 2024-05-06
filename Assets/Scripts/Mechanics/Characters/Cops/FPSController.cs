using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections.Specialized;
using Photon.Pun;
using LootLocker.Requests;

public class FPSController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject crosshairs;
    [SerializeField] private GameObject pingMarker;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private LayerMask pingLayerMask;
    //[SerializeField] private Transform debugTransform;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Animator animator;

    public ParticleSystem muzzleFlash;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Leaderboard leaderboard;

    private int kills = 0;
    private int damage = 0;

    void Awake()
    {
        pingLayerMask = LayerMask.GetMask("Default", "PingMarker");
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        leaderboard = FindObjectOfType<Leaderboard>();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
                //debugTransform.position = raycastHit.point;
            }

            if (starterAssetsInputs.aim)
            {
                crosshairs.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);

                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

                if (starterAssetsInputs.shoot)
                {
                    photonView.RPC("RPC_Shoot", RpcTarget.All);
                }
            }
            else
            {
                crosshairs.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);

                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * 10f));
                starterAssetsInputs.shoot = false;
            }

            if (starterAssetsInputs.ping)
            {
                photonView.RPC("RPC_Ping", RpcTarget.All);
                starterAssetsInputs.ping = false;
            }
        }
    }

    [PunRPC]
    void RPC_Shoot()
    {
        muzzleFlash.Play();
        Ray ray = new Ray(spawnBulletPosition.position, spawnBulletPosition.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.tag == "Assassin")
            {
                var enemyPlayerStats = hit.collider.GetComponent<PlayerStats>();

                if (enemyPlayerStats != null)
                {
                    enemyPlayerStats.TakeDamage(10);

                    if (photonView.IsMine)
                    {
                        //leaderboard.SubmitDamage(10);

                        damage++;
                        //leaderboard.SubmitScoreRoutine(globalDamageLeaderboardKey, damage);
                    }

                    if (enemyPlayerStats.currentHealth <= 0)
                    {
                        if (photonView.IsMine)
                        {
                            //leaderboard.SubmitKill();

                            kills++;
                            leaderboard.SubmitScoreRoutine(kills);
                        }

                        enemyPlayerStats.Die();
                    }
                }
            }
        }

        starterAssetsInputs.shoot = false;
    }
        
    [PunRPC]
    void RPC_Ping()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, pingLayerMask))
        {
            if (raycastHit.collider.gameObject.CompareTag("PingMarker"))
            {
                Destroy(raycastHit.collider.gameObject);
            }
            else
            {
                GameObject pingGO = Instantiate(pingMarker, raycastHit.point, Quaternion.identity);
                Destroy(pingGO, 10f);
            }
        }
    }
}