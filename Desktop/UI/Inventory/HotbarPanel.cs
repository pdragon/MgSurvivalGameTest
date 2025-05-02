using System.Drawing;
using Shared.Inventory;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ColorXNA = Microsoft.Xna.Framework.Color;
using static System.Reflection.Metadata.BlobBuilder;
using RectangleXNA = Microsoft.Xna.Framework.Rectangle;
using static System.Net.Mime.MediaTypeNames;



namespace Desktop.UI.Inventory
{
    public class HotbarPanel : InventoryPanel
    {
        public int SelectedSlotIndex { get; private set; }
        private Texture2D SelectionTexture;
        private GraphicsDevice GraphicsDevice;

        // Конструктор с передачей GraphicsDevice
        public HotbarPanel(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void Initialize(
            CharacterInventory inventory,
            Texture2D slotTex,
            Texture2D selectionTex,
            SpriteFont font
        )
        //public void Initialize(CharacterInventory inventory, Texture2D slotTex, Texture2D selectionTex, SpriteFont font)
        {
            //base.Initialize(inventory, slotTex, font);
            base.Initialize(inventory, null, slotTex, font, GraphicsDevice);
            SelectionTexture = selectionTex;

            // Инициализируем слоты
            int startX = 400;
            int startY = GraphicsDevice.Viewport.Height - 80;

            for (int i = 0; i < 9; i++)
            {
                Slots.Add(new InventorySlot
                {
                    Bounds = new RectangleXNA(startX + i * 70, startY, 64, 64)
                });
            }

            //// Пример: 9 слотов для горячей панели
            //Slots = new List<InventorySlot>();
            //int startX = 400; // Центр экрана по горизонтали
            //int startY = GraphicsDevice.Viewport.Height - 80; // Низ экрана

            //for (int i = 0; i < 9; i++)
            //{
            //    Slots.Add(new InventorySlot
            //    {
            //        Bounds = new RectangleXNA(startX + i * 70, startY, 64, 64)
            //    });
            //}
        }

        public void SelectSlot(int index)
        {
            SelectedSlotIndex = MathHelper.Clamp(index, 0, Slots.Count - 1);
        }

        public InventoryItem GetSelectedItem()
        {
            return Slots[SelectedSlotIndex].Item;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Отрисовка слотов
            //base.Draw(spriteBatch);
            // Отрисовка слотов
            foreach (var slot in Slots)
            {
                slot.Draw(spriteBatch, this.Font, SlotTexture); // Передаем явные параметры
            }

            // Отрисовка выделения
            var selectedSlotRect = Slots[SelectedSlotIndex].Bounds;
            spriteBatch.Draw(SelectionTexture, selectedSlotRect, ColorXNA.White * 0.5f);
        }
    }
}
