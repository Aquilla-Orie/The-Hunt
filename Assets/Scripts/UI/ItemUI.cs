using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemCountText;
    [SerializeField] private RawImage _itemBackground;
    [SerializeField] private RawImage _itemImage;
    public void UpdateItemCount(int count)
    {
        if (_itemCountText != null)
        {
            _itemCountText.text = count.ToString();
        }
    }

    public void SetItemImage(Texture2D image)
    {
        if (image == null) return;

        _itemImage.texture = image;
    }

    public void Selected()
    {
        if (_itemBackground != null)
        {
            _itemBackground.color = Color.black;
        }
    }

    public void Unselected()
    {
        if (_itemBackground != null)
        {
            _itemBackground.color = Color.white;
        }
    }
}
