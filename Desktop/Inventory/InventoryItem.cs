using Desktop.Classes;

namespace Desktop.Inventory
{
    /// <summary>
    /// Класс, описывающий предмет в инвентаре.
    /// </summary>
    public class InventoryItem
    {
        public enum ActionsType
        {
            None = 0,
            Mine,
            Plant,
            Build,
            Chop
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string TextureId { get; private set; }
        public ActionsType ActionType { get; set; } = ActionsType.None; // Например, "mine", "plant", "build"

        public InventoryItem(Item item, int quantity)
        {
            ID = item.Id.ToString();
            Name = item.Name;
            Quantity = quantity;
            TextureId = item.IconId;
        }
    }
}
