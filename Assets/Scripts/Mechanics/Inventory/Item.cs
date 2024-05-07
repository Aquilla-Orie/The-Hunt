using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    //An item is any object the player can hold physically
    public string Name;
    public string Description;
    public string Type;

    public Texture2D Image;

    public ItemUI ItemUI;

    public override void Interact(InteractionManager interactor = null)
    {
        base.Interact();
        interactor.AddItemToInventory(this);
        transform.parent = interactor.transform;
        transform.position = Vector3.zero;
        //Debug.Log($"{Name} {Description} :: was interacted by {interactor.name}");
    }

    public virtual void Use(InteractionManager interactor = null)
    {

    }
}