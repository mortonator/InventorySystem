
namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        public ItemSO itemSO;
        public int amount;

        public Item() { }
        public Item(Item old)
        {
            itemSO = old.itemSO;
            amount = old.amount;
        }
        public Item(Item old, int newAmount)
        {
            itemSO = old.itemSO;
            amount = newAmount;
        }
    }
}
