using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletProjectile : MonoBehaviourPunCallbacks
{
    [SerializeField] private float damageAmount = 10f;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float speed = 40f;
        bulletRigidbody.velocity = transform.forward * speed;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Time.deltaTime * bulletRigidbody.velocity.magnitude))
        {
            var enemyPlayerHealth = hit.collider.GetComponent<PlayerStats>();
            if (enemyPlayerHealth != null)
            {
                enemyPlayerHealth.TakeDamage(damageAmount);
            }

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}