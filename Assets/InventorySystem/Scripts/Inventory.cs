using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] List<Item> itemList;
        public List<Item> ItemList
        {
            get
            {
                return itemList;
            }
        }

        public Inventory()
        {
            itemList = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Item exists = itemList.Where(x => x.itemSO.name == item.itemSO.name).FirstOrDefault();

            if (exists != null)
                exists.amount += item.amount;
            else
                itemList.Add(item);
        }

        public void RemoveItem(ItemSO itemSO, int amount = -1)
        {
            Item containItem = itemList.Where(x => x.itemSO == itemSO).FirstOrDefault();
            if (containItem == null)
            {
                Debug.LogError($"Trying to remove an item from an inventory that does not contain it. \nIventory = {this} \nItem = {itemSO}");
                return;
            }

            if (amount == -1)
            {
                itemList.Remove(containItem);
                return;
            }

            if (containItem.amount == amount)
            {
                Debug.LogWarning($"Trying to remove item with amount {containItem.amount} by amount, but is not calling to remove the item completely");
                itemList.Remove(containItem);
                return;
            }

            if (containItem.amount < amount)
            {
                Debug.LogError($"Trying to remove item with amount {containItem.amount} by amount {amount} which is greater");
                return;
            }

            containItem.amount -= amount;
        }
    }

    [System.Serializable] 
    public class Item
    {
        public ItemSO itemSO;
        public int amount;
    }
}