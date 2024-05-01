using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemCountText;
    public void UpdateItemCount(int count)
    {
        if (_itemCountText != null)
        {
            _itemCountText.text = count.ToString();
        }
    }
}
