using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Inventory contains the list of items (and count) the player has
    public Dictionary<Item, int> InventoryItems = new Dictionary<Item, int>();
    public Item EquippedItem {  get; private set; }

    [SerializeField] private InventoryUI _inventoryUI;
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
            Debug.Log($"Item :: {item} already exists! Increasing count to {InventoryItems[item]}");
            _inventoryUI.AddItemToUI(item);
            return;
        }
        InventoryItems.Add(item, 1);
        _inventoryUI.AddItemToUI(item);

        Debug.Log($"Inventory Count :: {InventoryItems.Count}");
    }

    public void RemoveItem(Item item)
    {
        if (InventoryItems.ContainsKey(item))
        {
            if (InventoryItems[item] > 0)
            {
                InventoryItems[item] = InventoryItems[item]--;
                _inventoryUI.RemoveItemFromUI(item);
                return;
            }
        }
    }

    public void SelectItem(int index)
    {
        index = Mathf.Clamp(index, 0, InventoryItems.Count);
        EquippedItem = InventoryItems.Keys.ToList()[index];
    }
}
