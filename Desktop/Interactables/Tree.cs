using Desktop.Interactables;
using Microsoft.Xna.Framework;
using Shared.Inventory;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Shared
{
    public class Tree : IInteractable
    {
        public Rectangle Bounds { get; set; }

        // Событие для удаления объекта
        public event Action<Tree> OnTreeRemoved;

        public Tree(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public InteractionResult TryInteract(InventoryItem item)
        {
            if (item.ActionType != InventoryItem.ActionsType.Chop && CanInteract(item))
                return null;
            return new InteractionResult(InventoryItem.ActionsType.Chop, this);
        }

        public bool CanInteract(InventoryItem item)
        {
            return item?.ID == "axe";
        }

        public void OnInteract(InventoryItem item, Player player)
        {
            player.PlayAnimation("chopping");
            // Вызываем событие, а не удаляем напрямую
            OnTreeRemoved?.Invoke(this);
        }
    }
}