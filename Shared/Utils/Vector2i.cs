using System.Numerics;

namespace Shared.Utils
{
    public class Vector2i
    {
        public int X;
        public int Y;

        public Vector2i(){}
        public Vector2i(Vector2 vector) { 
            X = (int)vector.X;
            Y = (int)vector.Y;
        }

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        //public ToVector()
        //{

        //}
    }
}
