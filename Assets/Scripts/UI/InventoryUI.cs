using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Dictionary<string, KeyValuePair<Item, int>> _inventoryUIItems = new Dictionary<string, KeyValuePair<Item, int>>();
    [SerializeField] private RectTransform _inventoryUIPanel;
    [SerializeField] private ItemUI _itemUIPrefab;

    public void AddItemToUI(Item item)
    {
        string name = item.Name;
        int count = 1;


        if (_inventoryUIItems.ContainsKey(name))
        {
            //Only update count
            var iUI = _inventoryUIItems[name].Key.ItemUI;
            count = _inventoryUIItems[name].Value;
            count++;
            _inventoryUIItems[name] = new KeyValuePair<Item, int>(item, count);
            item.ItemUI = iUI;
            item.ItemUI.UpdateItemCount(_inventoryUIItems[name].Value);
            return;
        }
        ItemUI itemUI = Instantiate(_itemUIPrefab, _inventoryUIPanel);
        item.ItemUI = itemUI;
        itemUI.UpdateItemCount(1);
        _inventoryUIItems.Add(name, new KeyValuePair<Item, int>(item, count));
        //Set item properties
    }

    public void RemoveItemFromUI(Item item)
    {
        string name = item.Name;
        int count = _inventoryUIItems[name].Value;

        if (_inventoryUIItems.ContainsKey(name))
        {
            //Only update count
            count--;
            _inventoryUIItems[name] = new KeyValuePair<Item, int>(item, count);
            item.ItemUI.UpdateItemCount(_inventoryUIItems[name].Value);
            return;
        }
    }
}
