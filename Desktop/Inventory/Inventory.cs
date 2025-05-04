using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Inventory
{
    /// <summary>
    /// Общий класс-хранилище предметов.
    /// </summary>
    public class Inventory
    {
        public int Capacity { get; protected set; }
        public List<InventoryItem> Items { get; protected set; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
            Items = new List<InventoryItem>();
        }

        public virtual bool AddItem(InventoryItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ID == item.ID);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                return true;
            }
            else
            {
                if (Items.Count < Capacity)
                {
                    Items.Add(item);
                    return true;
                }
            }
            return false;
        }

        public virtual bool RemoveItem(string id, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ID == id);
            if (existingItem != null && existingItem.Quantity >= quantity)
            {
                existingItem.Quantity -= quantity;
                if (existingItem.Quantity == 0)
                    Items.Remove(existingItem);
                return true;
            }
            return false;
        }
    }
}
