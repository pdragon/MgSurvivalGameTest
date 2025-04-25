// Shared/UI/InventoryPanel.cs
using Desktop;
using Desktop.Icons;
using Desktop.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.UI
{
    public class InventoryPanel
    {
        public bool IsVisible;
        private readonly List<InventorySlot> slots = new List<InventorySlot>();
        private Texture2D PanelTexture;
        private Texture2D slotTexture;
        private SpriteFont font;

        public Rectangle Bounds;
        private Rectangle CloseButtonRect;
        private Texture2D CloseButtonTexture;
        //private IconManager Icons;

        //Для плавности анимации закрытия/открытия
        private float OpenProgress; // 0..1
        public bool IsAnimating => OpenProgress < 1f;


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
            // После создания слотов вычисляем общие границы
            Bounds = new Rectangle(
                slots.Min(s => s.Bounds.Left),
                slots.Min(s => s.Bounds.Top),
                slots.Max(s => s.Bounds.Right) - slots.Min(s => s.Bounds.Left),
                slots.Max(s => s.Bounds.Bottom) - slots.Min(s => s.Bounds.Top)
            );
            // Создаём кнопку закрытия в правом верхнем углу
            CloseButtonRect = new Rectangle(Bounds.Right + 40, Bounds.Top + 10, 30, 30);
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

        public void Update(GameTime gameTime)
        {
            if (IsVisible && IsAnimating)
            {
                OpenProgress = Math.Min(OpenProgress + (float)gameTime.ElapsedGameTime.TotalSeconds * 2f, 1f);
            }
            else if (!IsVisible && IsAnimating)
            {
                OpenProgress = Math.Max(OpenProgress - (float)gameTime.ElapsedGameTime.TotalSeconds * 2f, 0f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            // Анимированная позиция/прозрачность
            float scale = 0.8f + 0.2f * OpenProgress;
            Color color = Color.White * OpenProgress;
            spriteBatch.Draw(PanelTexture, Bounds.Center.ToVector2(), null, color, 0f,
                new Vector2(PanelTexture.Width / 2, PanelTexture.Height / 2), scale, SpriteEffects.None, 0);

            if (CloseButtonTexture != null)
            {
                // Отрисовка кнопки закрытия с эффектом наведения
                Color closeButtonColor = CloseButtonRect.Contains(Mouse.GetState().Position)
                    ? Color.Red : Color.White;
                spriteBatch.Draw(CloseButtonTexture, CloseButtonRect, closeButtonColor);
            }

            // Draw panel background
            spriteBatch.Draw(PanelTexture, new Rectangle(50, 50, 800, 500), Color.White * 0.9f);

            // Draw all slots
            foreach (var slot in slots)
            {
                slot.Draw(spriteBatch, font, slotTexture);
            }
            CloseButtonTexture = DesktopGame.TextureManager.GetTexture("error_error");
            spriteBatch.Draw(CloseButtonTexture, CloseButtonRect, Color.White);
        }

        public bool CheckCloseButtonClick(Point mousePos)
        {
            return CloseButtonRect.Contains(mousePos);
        }
    }
}