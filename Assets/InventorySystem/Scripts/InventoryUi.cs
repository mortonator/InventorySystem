using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InventorySystem
{
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] InventorySlot slotTemplate;
        [SerializeField] Transform slotContainer;
        [Space]
        [SerializeField] GameObject actionsScreen;

        Inventory myInventory;
        InventorySlot slotSelected;

        #region External
        public void SetInventory(Inventory newInventory)
        {
            myInventory = newInventory;
            RefreshInventorySlots();
        }
        public void AddItem(Item newItem)
        {
            myInventory.AddItem(newItem);
            RefreshInventorySlots();
        }
        #endregion

        void RefreshInventorySlots()
        {
            InventorySlot newItem;

            //less items than slots
            // go through slots, if there is an item for it -> update
            //     otherwise destroy slot
            if (myInventory.ItemList.Count < slotContainer.childCount)
            {
                for (int i = 0; i < slotContainer.childCount; i++)
                {
                    if (i < myInventory.ItemList.Count)
                    {
                        newItem = slotContainer.transform.GetChild(i).GetComponent<InventorySlot>();
                        newItem.UpdateSlot(myInventory.ItemList[i]);

                        continue;
                    }

                    Destroy(slotContainer.transform.GetChild(i).gameObject);
                }

                return;
            }


            //more items than slots
            // go through items, if there is a slot for it -> update
            //     otherwise instantiate
            for (int i = 0; i < myInventory.ItemList.Count; i++)
            {
                if (i < slotContainer.childCount)
                {
                    newItem = slotContainer.transform.GetChild(i).GetComponent<InventorySlot>();
                    newItem.UpdateSlot(myInventory.ItemList[i]);

                    continue;
                }

                newItem = Instantiate(slotTemplate, slotContainer);
                newItem.UpdateSlot(myInventory.ItemList[i]);
                newItem.gameObject.SetActive(true);
            }
        }

        public void OnSlotClicked(InventorySlot slot)
        {
            if (slotSelected == null)
            {
                slotSelected = slot;
                slotSelected.SetSelected(true);
                actionsScreen.SetActive(true);
                return;
            }

            if (slotSelected == slot)
            {
                slotSelected.SetSelected(false);
                slotSelected = null;
                actionsScreen.SetActive(false);
                return;
            }


        }

        public void DropSlot()
        {
            ItemHolder holder = Instantiate(slotSelected.item.itemSO.prefab, transform.position + (Vector3.forward * 2), Quaternion.identity);
            holder.item.amount = slotSelected.item.amount;

            myInventory.RemoveItem(slotSelected.item.itemSO);

            actionsScreen.SetActive(false);
            slotSelected.SetSelected(false);
            slotSelected = null;

            RefreshInventorySlots();
        }
    }
}