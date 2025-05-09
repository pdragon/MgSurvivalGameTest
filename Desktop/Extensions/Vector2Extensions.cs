using Shared.Utils;
//using System.Numerics; // для Vector2
using XnaPoint = Microsoft.Xna.Framework.Point;    // для Point
using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace Desktop.Extensions
{
    public static class Vector2Extensions
    {
        ///// <summary>
        ///// Преобразует вектор Microsoft.Xna.Framework.Vector2 в System.Numerics.Vector2.
        ///// </summary>
        ////public static Vector2 ToNumerics(this XnaVector2 v)
        ////{
        ////    return new Vector2(v.X, v.Y);
        ////}

        ///// <summary>
        ///// Преобразует вектор System.Numerics.Vector2 в Microsoft.Xna.Framework.Vector2.
        ///// </summary>
        //public static XnaVector2 ToMonoGame(this Vector2 v)
        //{
        //    return new XnaVector2(v.X, v.Y);
        //}

        //public static Vector2i ToIntVector(this Vector2 v)
        //{
        //    return new Vector2i(v);
        //}

        /// <summary>
        /// Переводит мировую позицию (в пикселях) в координаты клетки (по размеру тайла).
        /// </summary>
        public static XnaPoint ToGridCoordinates(this XnaVector2 worldPos, int tileSize)
        {
            return new XnaPoint(
                (int)(worldPos.X / tileSize),
                (int)(worldPos.Y / tileSize)
            );
        }
    }
}
