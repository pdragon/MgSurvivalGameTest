using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Inventory
{
    /// <summary>
    /// Специализированный класс инвентаря для персонажа.
    /// Здесь можно добавить дополнительные методы,
    /// специфичные для персонажа (например, методы для экипировки).
    /// </summary>
    public class CharacterInventory : Inventory
    {
        public CharacterInventory(int capacity) : base(capacity)
        {
        }

        // Пример дополнительного метода:
        public bool EquipItem(string id)
        {
            // Логика экипировки предмета
            // Например, если предмет найден и доступен для экипировки, возвращаем true.
            return Items.Exists(i => i.ID == id);
        }
    }
}
