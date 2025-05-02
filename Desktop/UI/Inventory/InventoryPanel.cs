// Shared/UI/InventoryPanel.cs
using Desktop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.UI.Inventory
{
    public class InventoryPanel
    {
        public bool IsVisible;
        //protected readonly List<InventorySlot> Slots = new List<InventorySlot>();
        protected List<InventorySlot> Slots = new List<InventorySlot>();
        private Texture2D PanelTexture;
        protected Texture2D SlotTexture;
        protected SpriteFont Font;

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

        public void Initialize(
       CharacterInventory inventory,
       Texture2D panelTex,
       Texture2D slotTex,
       SpriteFont font,
       GraphicsDevice graphicsDevice // Добавляем GraphicsDevice
   )
        //public void Initialize(CharacterInventory inventory, Texture2D panelTex, Texture2D slotTex, SpriteFont font)
        {
            PanelTexture = panelTex;
            SlotTexture = slotTex;
            this.Font = font;

            // Create slots grid (example: 5x8)
            int slotSize = 64;
            int padding = 10;
            int startX = 100;
            int startY = 100;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Slots.Add(new InventorySlot
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
                Slots.Min(s => s.Bounds.Left),
                Slots.Min(s => s.Bounds.Top),
                Slots.Max(s => s.Bounds.Right) - Slots.Min(s => s.Bounds.Left),
                Slots.Max(s => s.Bounds.Bottom) - Slots.Min(s => s.Bounds.Top)
            );
            // Создаём кнопку закрытия в правом верхнем углу
            CloseButtonRect = new Rectangle(Bounds.Right + 40, Bounds.Top + 10, 30, 30);
        }

        public void UpdateSlots(CharacterInventory inventory)
        {
            if (inventory.Items.Count > 0)
            {
                for (int i = 0; i < Slots.Count; i++)
                {
                    Slots[i].Item = i < inventory.Items.Count ? inventory.Items[i] : null;
                    if (Slots[i].Item != null)
                    {
                        Slots[i].IconTexture = DesktopGame.TextureManager.GetTexture(Slots[i].Item.TextureId);
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

        public virtual void Draw(SpriteBatch spriteBatch)
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
            foreach (var slot in Slots)
            {
                slot.Draw(spriteBatch, Font, SlotTexture);
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