// Shared/UI/InventorySlot.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Desktop.Inventory;

namespace Desktop.UI.Inventory
{
    public class InventorySlot
    {
        public Rectangle Bounds;
        public InventoryItem Item;
        public Texture2D IconTexture;

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D slotTexture)
        {
            // Draw slot background
            spriteBatch.Draw(slotTexture, Bounds, Color.White);

            if (Item != null && IconTexture != null)
            {
                // Draw item icon centered in slot
                var iconPos = new Vector2(
                    Bounds.Center.X - IconTexture.Width / 2,
                    Bounds.Center.Y - IconTexture.Height / 2
                );

                spriteBatch.Draw(IconTexture, iconPos, Color.White);

                // Draw quantity
                if (Item.Quantity > 1)
                {
                    var quantityPos = new Vector2(Bounds.Right - 20, Bounds.Bottom - 25);
                    spriteBatch.DrawString(font, Item.Quantity.ToString(), quantityPos, Color.White);
                }
            }
        }
    }
}