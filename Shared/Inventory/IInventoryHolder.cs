using System.Collections.Generic;
using System.Text;

namespace Shared.Inventory
{
    // Интерфейс для объектов, имеющих инвентарь
    public interface IInventoryHolder
    {
        Inventory MainInventory { get; set; }
        List<Inventory> AdditionalInventories { get; set; }
    }
}
