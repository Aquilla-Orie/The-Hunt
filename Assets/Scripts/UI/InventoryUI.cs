using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Dictionary<Item, int> _inventoryUIItems = new Dictionary<Item, int>();
    [SerializeField] private RectTransform _inventoryUIPanel;
    [SerializeField] private ItemUI _itemUIPrefab;

    public void AddItemToUI(Item item)
    {
        if (_inventoryUIItems.ContainsKey(item))
        {
            //Only update count
            _inventoryUIItems[item] = _inventoryUIItems[item]++;
            item.ItemUI.UpdateItemCount(_inventoryUIItems[item]);
            return;
        }
        ItemUI itemUI = Instantiate(_itemUIPrefab, _inventoryUIPanel);
        item.ItemUI = itemUI;
        itemUI.UpdateItemCount(1);
        //Set item properties
    }

    public void RemoveItemFromUI(Item item)
    {
        if (_inventoryUIItems.ContainsKey(item))
        {
            //Only update count
            return;
        }
    }
}
