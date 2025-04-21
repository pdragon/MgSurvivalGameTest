
using Microsoft.Xna.Framework.Graphics;

namespace Desktop.Classes
{
    /// <summary>
    /// Свойства этого класса надо будет потом сделать, чтобы грузились из json файла.
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IconId { get; set; }
    }

    
}
