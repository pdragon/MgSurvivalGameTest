using Shared.Inventory;
using Shared;
using Microsoft.Xna.Framework;

namespace Desktop.Interactables
{
    public class InteractionResult
    {
        public InventoryItem.ActionsType ActionType { get; }
        public IInteractable Source { get; }
        public InteractionResult(InventoryItem.ActionsType actionType, IInteractable source)
        {
            ActionType = actionType; Source = source;
        }
    }

    public interface IInteractable
    {
        /// <summary>
        /// Границы объекта в мировых координатах для hit-теста.
        /// </summary>
        Rectangle Bounds { get; }
        InteractionResult TryInteract(InventoryItem item);
        /// <summary>
        /// Можно ли взаимодействовать с этим объектом данным предметом.
        /// </summary>
        bool CanInteract(InventoryItem item);
        /// <summary>
        /// Обработчик взаимодействия.
        /// </summary>
        void OnInteract(InventoryItem item, Player player);
    }
}
