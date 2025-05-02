namespace Shared.Inventory
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

        public InventoryItem(string id, string name, string textureId = "error_error", int quantity = 1)
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            TextureId = textureId;
        }
    }
}
