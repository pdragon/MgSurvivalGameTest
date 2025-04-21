using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Inventory
{
    /// <summary>
    /// Класс, описывающий предмет в инвентаре.
    /// </summary>
    public class InventoryItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string TextureId { get; private set; }

        public InventoryItem(string id, string name, string textureId = "error_error", int quantity = 1)
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            TextureId = textureId;
        }
    }
}
