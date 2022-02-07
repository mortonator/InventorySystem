using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory System/Item SO")]
    [System.Serializable]
    public class ItemSO : ScriptableObject
    {
        public enum ItemType
        {
            Food,
            Weapon,
            Armour,
            Resource,
            Decor
        }

        public ItemType itemType;
        public Sprite icon;
        public ItemHolder prefab;
        public float weight;
    }
}