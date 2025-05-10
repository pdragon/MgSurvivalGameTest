using Shared.Pathfinding;
using System.Drawing;
using System.IO;
using System.Numerics;

namespace Shared
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        private Vector2 destination;
        private readonly float speed;

        // Размер плитки — можно вынести в константу
        private const int MapTileSize = 20;
        // Путь в виде списка клеток
        private List<GridPoint> Path = new List<GridPoint>();

        public Player(int startX, int startY, float speed)
        {
            Position = new Vector2(startX, startY);
            destination = Position;
            this.speed = speed;
        }

        // Текущее положение в сеточных координатах
        public Point GridPosition
            => new Point((int)(Position.X / MapTileSize), (int)(Position.Y / MapTileSize));

        public void PlayAnimation(string animationName)
        {
            // Логика анимации
        }

        public void SetDestination(Point click)
        {
            destination = new Vector2(click.X, click.Y);
        }

        /// <summary>
        /// Задаёт путь клетками; если null или пуст — путь сбрасывается.
        /// </summary>
        public void SetPath(List<GridPoint> newPath)
        {
            //Path = newPath ?? new List<GridPoint>();
            Path = newPath?.Count > 0 ? newPath : new List<GridPoint>();
        }

        public void Update(float deltaTime, TileMap map)
        {
            //if (Path.Count > 0)
            //{
            //    // Целевая клетка — первая в списке
            //    var gp = Path[0];
            //    var targetPos = new Vector2(
            //        gp.X * MapTileSize + MapTileSize / 2,
            //        gp.Y * MapTileSize + MapTileSize / 2);

            //    var dir = targetPos - Position;
            //    if (dir.Length() < 1f)
            //    {
            //        // Достигли — удаляем клетку
            //        Path.RemoveAt(0);
            //    }
            //    else
            //    {
            //        dir = Vector2.Normalize(dir);
            //        Position += dir * speed * deltaTime;
            //    }
            //}
            //else
            //{
            //    // Ваша прежняя логика перемещения по клику
            //}
            if (Path.Count > 0)
            {
                var targetCell = Path[0];
                var targetPos = new Vector2(
                    targetCell.X * MapTileSize + MapTileSize / 2,
                    targetCell.Y * MapTileSize + MapTileSize / 2
                );

                // Вычисляем направление и расстояние
                Vector2 dir = targetPos - Position;
                float distance = dir.Length();

                // Если персонаж близко к цели, переходим к следующей точке
                if (distance <= speed * deltaTime)
                {
                    Position = targetPos;
                    Path.RemoveAt(0);
                }
                else
                {
                    // Нормализуем направление и двигаемся
                    dir = Vector2.Normalize(dir);
                    Position += dir * speed * deltaTime;
                }
            }

        }
    }
}
