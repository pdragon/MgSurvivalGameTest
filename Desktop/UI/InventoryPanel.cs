// Shared/UI/InventoryPanel.cs
using Desktop;
using Desktop.Icons;
using Desktop.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared.Inventory;
using System.Collections.Generic;

namespace Shared.UI
{
    public class InventoryPanel
    {
        public bool IsVisible;
        private readonly List<InventorySlot> slots = new List<InventorySlot>();
        private Texture2D PanelTexture;
        private Texture2D slotTexture;
        private SpriteFont font;
        //private IconManager Icons;

        public InventoryPanel() { 
            //Icons = new IconManager();
        }

        public void Initialize(CharacterInventory inventory, Texture2D panelTex, Texture2D slotTex, SpriteFont font)
        {
            PanelTexture = panelTex;
            slotTexture = slotTex;
            this.font = font;

            // Create slots grid (example: 5x8)
            int slotSize = 64;
            int padding = 10;
            int startX = 100;
            int startY = 100;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    slots.Add(new InventorySlot
                    {
                        Bounds = new Rectangle(
                            startX + x * (slotSize + padding),
                            startY + y * (slotSize + padding),
                            slotSize,
                            slotSize
                        )
                    });
                }
            }

            UpdateSlots(inventory);
        }

        public void UpdateSlots(CharacterInventory inventory)
        {
            if (inventory.Items.Count > 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    slots[i].Item = i < inventory.Items.Count ? inventory.Items[i] : null;
                    if (slots[i].Item != null)
                    {
                        slots[i].IconTexture = DesktopGame.TextureManager.GetTexture(slots[i].Item.TextureId);
                    }
                }
            }
            //for (int i = 0; i < slots.Count; i++)
            //{
            //    slots[i].Item = i < inventory.Items.Count ? inventory.Items[i] : null;
            //    // Здесь можно добавить загрузку текстур для предметов
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            // Draw panel background
            spriteBatch.Draw(PanelTexture, new Rectangle(50, 50, 800, 500), Color.White * 0.9f);

            // Draw all slots
            foreach (var slot in slots)
            {
                slot.Draw(spriteBatch, font, slotTexture);
            }
        }
    }
}