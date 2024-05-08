using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private List<Interactable> _interactables;
    [SerializeField] private Inventory _inventory;

    private void Start()
    {
        _interactables = new List<Interactable>();
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_starterAssetsInputs.interact)
        {
            if(_interactables.Count > 0)
            {
                var closest = _interactables[0];
                foreach (var interactable in _interactables)
                {
                    float dist = Vector3.Distance(transform.position, interactable.transform.position);
                    if (dist > Vector3.Distance(transform.position, closest.transform.position))
                    {
                        closest = interactable;
                    }
                }

                closest.Interact(this);
            }
            _starterAssetsInputs.interact = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            if(interactable.CanInteract)
                _interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            _interactables.Remove(interactable);
        }
    }

    public void AddItemToInventory(Item item)
    {
        _inventory.AddItem(item);
    }
}
