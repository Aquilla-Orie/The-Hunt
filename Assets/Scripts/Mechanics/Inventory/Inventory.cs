using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Inventory contains the list of items (and count) the player has
    public Dictionary<Item, int> InventoryItems = new Dictionary<Item, int>();
    public Item EquippedItem {  get; private set; }
    public void InitializeInventory()
    {
        //Primes up the inventory for use
    }

    public void AddItem(Item item)
    {
        Debug.Log($"{item.Name} added to inventory");
        if (InventoryItems.ContainsKey(item))
        {
            InventoryItems[item] = InventoryItems[item]++;
            return;
        }
        InventoryItems.Add(item, 1);
    }

    public void RemoveItem(Item item)
    {
        if (InventoryItems.ContainsKey(item))
        {
            if (InventoryItems[item] > 0)
            {
                InventoryItems[item] = InventoryItems[item]--;
                return;
            }
        }
    }

    public void CycleInventory()
    {
        
    }
}
