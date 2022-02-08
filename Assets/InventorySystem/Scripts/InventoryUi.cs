using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace InventorySystem
{
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] InventorySlot slotTemplate;
        [Space]
        [SerializeField] Transform mySlotContainer;
        [SerializeField] Transform otherSlotContainer;
        [Space]
        [SerializeField] GameObject actionsScreen;
        [SerializeField] TextMeshProUGUI countText;

        Inventory myInventory;
        Inventory otherInventory;
        InventorySlot slotSelected;

        List<InventorySlot> mySlotsList;
        List<InventorySlot> otherSlotsList;

        int moveCountMax;
        int moveCount;

        #region Setup
        void Start()
        {
            mySlotsList = new List<InventorySlot>();
            otherSlotsList = new List<InventorySlot>();

            AddFirstSlot(mySlotContainer, mySlotsList, true);
            AddFirstSlot(otherSlotContainer, otherSlotsList, false);
        }
        void AddFirstSlot(Transform container, List<InventorySlot> list, bool isMySlot)
        {
            InventorySlot newSlot;

            newSlot = Instantiate(slotTemplate, container);
            newSlot.UpdateSlot(null);
            newSlot.gameObject.SetActive(true);
            newSlot.SetSlotOwner(isMySlot);

            list.Add(newSlot);
        }
        #endregion

        #region External
        public void SetMyInventory(Inventory newInventory)
        {
            myInventory = newInventory;
            RefreshInventorySlots();
        }
        public void SetOtherInventory(Inventory newInventory)
        {
            otherInventory = newInventory;
            otherSlotContainer.gameObject.SetActive(otherInventory != null);

            RefreshInventorySlots();
        }

        public void AddItem(Item newItem)
        {
            myInventory.AddItem(newItem);
            RefreshInventorySlots();
        }
        #endregion

        #region Refresh Slots
        void RefreshInventorySlots()
        {
            if (otherInventory != null)
                RefreshInventorySlots(otherInventory, otherSlotContainer, otherSlotsList, false);

            RefreshInventorySlots(myInventory, mySlotContainer, mySlotsList, true);
        }
        void RefreshInventorySlots(Inventory inv, Transform container, List<InventorySlot> slotList, bool isMySlot)
        {
            InventorySlot newItem;

            // items equal to slots excluding extra
            // go through slots and update
            if (inv.ItemList.Count+1 == slotList.Count)
            {
                for (int i = 0; i < slotList.Count; i++)
                {
                    if (i == inv.ItemList.Count)
                    {
                        newItem = slotList[i];
                        newItem.UpdateSlot(null);
                        return;
                    }

                    newItem = slotList[i];
                    newItem.UpdateSlot(inv.ItemList[i]);
                }

                return;
            }

            // less items than slots excluding extra
            // go through slots, if there is an item for it -> update
            //     otherwise destroy slot
            if (inv.ItemList.Count+1 < slotList.Count)
            {
                for (int i = 0; i < slotList.Count; i++)
                {
                    if (i < inv.ItemList.Count)
                    {
                        newItem = slotList[i];
                        newItem.UpdateSlot(inv.ItemList[i]);

                        continue;
                    }

                    if (i == inv.ItemList.Count)
                    {
                        newItem = slotList[i];
                        newItem.UpdateSlot(null);

                        continue;
                    }

                    Destroy(slotList[i].gameObject);
                    slotList.RemoveAt(i);
                }

                return;
            }


            // more items than slots exluding extra
            // go through items, if there is a slot for it -> update
            //     otherwise instantiate
            for (int i = 0; i <= inv.ItemList.Count; i++)
            {
                if (i < slotList.Count)
                {
                    newItem = slotList[i];
                    newItem.UpdateSlot(inv.ItemList[i]);

                    continue;
                }

                if (i == inv.ItemList.Count)
                {
                    newItem = Instantiate(slotTemplate, container);
                    newItem.SetSlotOwner(isMySlot);
                    newItem.UpdateSlot(null);
                    newItem.gameObject.SetActive(true);
                    slotList.Add(newItem);

                    continue;
                }

                newItem = Instantiate(slotTemplate, container);
                newItem.SetSlotOwner(isMySlot);
                newItem.UpdateSlot(inv.ItemList[i]);
                newItem.gameObject.SetActive(true);
                slotList.Add(newItem);
            }
        }
        #endregion

        #region Slot Selection
        public void OnSlotClicked(InventorySlot slot)
        {
            slot.ResetButton();

            if (slotSelected == null)
            {
                SelectSlot(slot);
                return;
            }

            if (slotSelected == slot)
            {
                UnselectSlot();
                return;
            }

            if (slot.item == null)
            {
                MoveSlot(slot);
                RefreshInventorySlots();
                UnselectSlot();
                return;
            }

            SwapSlots(slot);
            RefreshInventorySlots();
            UnselectSlot();
        }
        void SelectSlot(InventorySlot slot)
        {
            if (slot.item == null)
                return;

            slotSelected = slot;
            slotSelected.SetSelected(true);
            actionsScreen.SetActive(true);

            moveCountMax = slotSelected.item.amount;
            moveCount = moveCountMax;
            countText.text = moveCount.ToString();
        }
        void UnselectSlot()
        {
            slotSelected.SetSelected(false);
            slotSelected = null;
            actionsScreen.SetActive(false);
        }
        void MoveSlot(InventorySlot slot)
        {
            if (slotSelected.isMySlot)
                myInventory.RemoveItem(slotSelected.item, moveCount);
            else
                otherInventory.RemoveItem(slotSelected.item, moveCount);

            if (slot.isMySlot)
                myInventory.AddItem(slotSelected.item, moveCount);
            else
                otherInventory.AddItem(slotSelected.item, moveCount);
        }
        void SwapSlots(InventorySlot slot)
        {
            if (slotSelected.isMySlot)
                myInventory.ReplaceItem(slotSelected.item, new Item(slot.item));
            else
                otherInventory.ReplaceItem(slotSelected.item, new Item(slot.item));
            

            if (slot.isMySlot)
                myInventory.ReplaceItem(slot.item, new Item(slotSelected.item));
            else
                otherInventory.ReplaceItem(slot.item, new Item(slotSelected.item));
        }
        #endregion

        #region Actions
        public void DropSlot()
        {
            ItemHolder holder = Instantiate(slotSelected.item.itemSO.prefab, transform.position + (Vector3.forward * 2), Quaternion.identity);
            holder.item.amount = moveCount;

            myInventory.RemoveItem(slotSelected.item, moveCount);

            actionsScreen.SetActive(false);
            slotSelected.SetSelected(false);
            slotSelected = null;

            RefreshInventorySlots();
        }

        public void ChangeMoveCount(int inc)
        {
            if (moveCountMax == 1)
                return;

            moveCount += inc;

            moveCount = Mathf.Clamp(moveCount, 1, moveCountMax);

            countText.text = moveCount.ToString();
        }
        public void ChangeMoveCount_Min()
        {
            moveCount = 1;

            countText.text = moveCount.ToString();
        }
        public void ChangeMoveCount_Max()
        {
            moveCount = moveCountMax;

            countText.text = moveCount.ToString();
        }
        #endregion
    }
}