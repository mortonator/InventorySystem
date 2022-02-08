using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image backgroundImage;
        [SerializeField] Image image;
        [SerializeField] TMPro.TextMeshProUGUI text;

        public bool isMySlot { get; private set; }
        public Item item { get; private set; }

        public void SetSlotOwner(bool _isMySlot) => isMySlot = _isMySlot;

        public void UpdateSlot(Item _item)
        {
            item = _item;

            if (_item != null)
            {
                image.sprite = item.itemSO.icon;
                text.text = $"{item.amount}\nx {item.itemSO.weight}";

                return;
            }

            image.sprite = null;
            text.text = "";
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
                backgroundImage.color = new Color(1, 0.5f, 0);
            else
                backgroundImage.color = Color.white;
        }

        public void ResetButton()
        {
            button.interactable = false;
            button.interactable = true;
        }
    }
}