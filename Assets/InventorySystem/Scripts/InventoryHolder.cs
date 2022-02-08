using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryHolder : MonoBehaviour
    {
        Inventory inventory = new Inventory();

        public void Open(InventoryUi ui)
        {
            ui.SetOtherInventory(inventory);
        }
        public void Close(InventoryUi ui)
        {
            ui.SetOtherInventory(null);
        }
    }
}