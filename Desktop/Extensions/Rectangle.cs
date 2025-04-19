using System.Drawing;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Desktop.Extensions
{
    public static class Rectangle2XNA
    {
        /// <summary>
        /// Преобразует вектор System.Drawing.Rectangle в Microsoft.Xna.Framework.Rectangle.
        /// </summary>
        public static XnaRectangle ToMonoGame(this Rectangle v)
        {
            return new XnaRectangle(v.X, v.Y, v.Width, v.Height);
        }
    }
}
