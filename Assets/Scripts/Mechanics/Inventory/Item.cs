using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    //An item is any object the player can hold physically
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string Type { get; protected set; }

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

    public virtual void Use()
    {

    }
}