using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    //Inventory contains the list of items (and count) the player has
    public Dictionary<string, KeyValuePair<Item, int>> InventoryItems = new Dictionary<string, KeyValuePair<Item, int>>();
    public Item EquippedItem {  get; private set; }

    [SerializeField] private InventoryUI _inventoryUI;

    private int _inventoryIndex;
    public void InitializeInventory()
    {
        //Primes up the inventory for use
        _inventoryIndex = 0;
    }

    private void Update()
    {
        Vector2 inventorySelect = _starterAssetsInputs.inventory;
        if (inventorySelect.x > 0)
        {
            _inventoryIndex++;
            SelectItem(_inventoryIndex);
        }
        if (inventorySelect.x < 0)
        {
            _inventoryIndex--;
            SelectItem(_inventoryIndex);
        }
        
    }

    public void AddItem(Item item)
    {
        string name = item.Name;
        int count = 1;


        if (InventoryItems.ContainsKey(name))
        {
            count = InventoryItems[name].Value;
            count++;
            InventoryItems[name] = new KeyValuePair<Item, int> ( item, count);
            _inventoryUI.AddItemToUI(item);
            return;
        }
        InventoryItems.Add(name, new KeyValuePair<Item, int>(item, count));
        _inventoryUI.AddItemToUI(item);

    }

    //public void RemoveItem(Item item)
    //{
    //    if (InventoryItems.ContainsKey(item))
    //    {
    //        if (InventoryItems[item] > 0)
    //        {
    //            InventoryItems[item] = InventoryItems[item]--;
    //            _inventoryUI.RemoveItemFromUI(item);
    //            return;
    //        }
    //    }
    //}

    public void SelectItem(int index)
    {
        Debug.Log($"index before clamp {index}");
        index = Mathf.Clamp(index, 0, InventoryItems.Count);
        if (InventoryItems.Count <= 0 || index >= InventoryItems.Count) return;
        Debug.Log($"Index after clamp {index}");
        EquippedItem = InventoryItems.Values.ToList()[index].Key;
        Debug.Log($"Equipped item is {EquippedItem.Name}");
    }
}
