using Shared.Utils;
using System.Numerics;
using System.Drawing;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Desktop.Extensions
{
    public static class PointExtensions
    {
        /// <summary>
        /// Преобразует System.Drawing.Point в Microsoft.Xna.Framework.Point.
        /// </summary>
        public static XnaPoint ToMonoGame(this Point v)
        {
            return new XnaPoint(v.X, v.Y);
        }

        public static Point ToSystemDrawing(this XnaPoint v)
        {
            return new Point(v.X, v.Y);
        }
    }
}
