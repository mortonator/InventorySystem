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
            Item exists = itemList.Where(x => x.itemSO == item.itemSO).FirstOrDefault();

            if (exists != null)
            {
                exists.amount += item.amount;
            }
            else
            {
                itemList.Add(new Item(item));
            }
        }
        public void AddItem(Item item, int amount)
        {
            Item exists = itemList.Where(x => x.itemSO == item.itemSO).FirstOrDefault();

            if (exists != null)
            {
                exists.amount += amount;
            }
            else
            {
                itemList.Add(new Item(item, amount));
            }
        }

        public void RemoveItem(Item item, int amount = -1)
        {
            if (itemList.Contains(item) == false)
            {
                Debug.LogError($"Trying to remove an item from an inventory that does not contain it. \nIventory = {this} \nItem = {item}");
                return;
            }

            if ((amount == -1) || (item.amount == amount))
            {
                itemList.Remove(item);
                return;
            }

            if (item.amount < amount)
            {
                Debug.LogError($"Trying to remove item with amount {item.amount} by amount {amount} which is greater");
                return;
            }

            item.amount -= amount;
        }

        public void ReplaceItem(Item oldItem, Item newItem)
        {
            if (itemList.Contains(oldItem) == false)
            {
                Debug.LogError($"Trying to replace item that does not exist in this inventory. \nOld Item{oldItem.itemSO.name} \nNew Item {newItem.itemSO.name}");
                return;
            }

            int oldIndex = itemList.IndexOf(oldItem);
            itemList.Insert(oldIndex, newItem);

            RemoveItem(oldItem);
        }

        public void DebugList()
        {
            string listString = "";
            foreach (Item item in itemList)
            {
                listString += $"{item.itemSO.name} ({item.amount})\n";
            }
            Debug.Log(listString);
        }
    }
}