using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Inventory
{
    // Интерфейс для объектов, имеющих инвентарь
    public interface IInventoryHolder
    {
        Inventory MainInventory { get; set; }
        List<Inventory> AdditionalInventories { get; set; }
    }
}
