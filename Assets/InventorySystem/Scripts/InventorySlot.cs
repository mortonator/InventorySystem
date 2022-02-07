using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Image backgroundImage;
        [SerializeField] UnityEngine.UI.Image image;
        [SerializeField] TMPro.TextMeshProUGUI text;

        public Item item { get; private set; }

        public void UpdateSlot(Item _item)
        {
            item = _item;

            image.sprite = item.itemSO.icon;
            text.text = $"{item.amount}\nx {item.itemSO.weight}";
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
                backgroundImage.color = new Color(1, 0.5f, 0);
            else
                backgroundImage.color = Color.white;
        }
    }
}