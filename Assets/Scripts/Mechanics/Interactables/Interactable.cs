using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool CanInteract = true;
    public virtual void Interact(InteractionManager interactor = null)
    {

    }
}
