using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrambler : Item
{
    [SerializeField] private float _scambleRadius = 20f;
    [SerializeField] private float _scambleDistance = 20f;
    [SerializeField] private LayerMask _scambleMask;

    private void OnEnable()
    {
        CanInteract = false;
    }

    public override void Use(InteractionManager interactor = null)
    {
        base.Use(interactor);
        Debug.Log("Using Scrambler");
        interactor.GetComponent<FPSController>().PlaceItem(Name);
    }

    private void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _scambleRadius, transform.forward, _scambleDistance, _scambleMask, QueryTriggerInteraction.UseGlobal);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.GetComponent<SpyCamera>())
            {
                hit.collider.GetComponent<SpyCamera>().Scramble();
            }
        }
    }
}
