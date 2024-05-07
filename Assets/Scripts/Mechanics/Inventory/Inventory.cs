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
    [SerializeField] private InteractionManager _interactionManager;

    [SerializeField] private List<Item> _items;

    private int _inventoryIndex;

    private void Start()
    {
        _interactionManager = GetComponent<InteractionManager>();
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        //Primes up the inventory for use
        _inventoryIndex = 0;
        SelectItem(_inventoryIndex);

        if (_items.Count > 0)
        {
            foreach (Item item in _items)
            {
                AddItem(item);
            }
        }
    }

    private void Update()
    {
        if (_starterAssetsInputs.inventoryRight)
        {
            _inventoryIndex++;
            SelectItem(_inventoryIndex);
            
        }
        if (_starterAssetsInputs.inventoryLeft)
        {
            _inventoryIndex--;
            SelectItem(_inventoryIndex);
        }

        if (_starterAssetsInputs.inventoryUse)
        {
            if (EquippedItem != null)
            {
                UseItem(EquippedItem);
            }
        }

        _starterAssetsInputs.inventoryLeft = false;
        _starterAssetsInputs.inventoryRight = false;
        _starterAssetsInputs.inventoryUse = false;
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

    public void RemoveItem(Item item)
    {
        string name = item.Name;
        int count = 0;

        if (InventoryItems.ContainsKey(name))
        {
            count = InventoryItems[name].Value;

            if (count > 0)
            {
                count--; 
                InventoryItems[name] = new KeyValuePair<Item, int>(item, count);
                _inventoryUI.RemoveItemFromUI(item);
                return;
            }
        }
    }

    public void SelectItem(int index)
    {
        Debug.Log($"index before clamp {index}");
        index = Mathf.Clamp(index, 0, InventoryItems.Count);
        if (InventoryItems.Count <= 0 || index >= InventoryItems.Count) return;
        Debug.Log($"Index after clamp {index}");
        EquippedItem = InventoryItems.Values.ToList()[index].Key;
        Debug.Log($"Equipped item is {EquippedItem.Name}");
    }

    public void UseItem(Item item)
    {
        string name = item.Name;
        int count = 0;

        if (InventoryItems.ContainsKey(name))
        {
            count = InventoryItems[name].Value;

            if (count > 0)
            {
                item.Use(_interactionManager);
                RemoveItem(item);
                return;
            }
        }
    }
}
