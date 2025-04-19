using System.Drawing;
using System.Numerics;

namespace Shared
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        private Vector2 destination;
        private readonly float speed;

        public Player(int startX, int startY, float speed)
        {
            Position = new Vector2(startX, startY);
            destination = Position;
            this.speed = speed;
        }

        public void SetDestination(Point click)
        {
            destination = new Vector2(click.X, click.Y);
        }

        public void Update(float deltaTime, TileMap map)
        {
            var dt = (float)deltaTime;
            var dir = destination - Position;
            if (dir.Length() < 1f) return;

            dir = Vector2.Normalize(dir);            // статический метод с параметром
            Position += dir * speed * deltaTime;
        }
    }
}
