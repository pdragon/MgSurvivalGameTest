using Shared;
using System.Xml.Linq;
using Desktop.Extensions;
using Desktop.Interactables;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Desktop
{
    public class GameCore
    {
        public Player Player { get; private set; }
        public TileMap Map { get; private set; }
        public List<IInteractable> Interactables { get; } = new List<IInteractable>();

        public GameCore()
        {
            // Инициализация игрока и карты
            Player = new Player(5, 5, 200f);
            Map = new TileMap(100, 100, 32);

            // Добавление тестового объекта
            AddTree(new Rectangle(200, 200, 32, 32));
        }

        public void Update(float deltaTime, Point? click)
        {
            // Обновление игрока
            if (click.HasValue)
            {
                Player.SetDestination(new Point(click.Value.X, click.Value.Y).ToSystemDrawing());
            }
            Player.Update(deltaTime, Map);
        }

        private void AddTree(Rectangle bounds)
        {
            var tree = new Tree(bounds);
            tree.OnTreeRemoved += RemoveTree;
            Interactables.Add(tree);
        }

        private void RemoveTree(Tree tree)
        {
            Interactables.Remove(tree);
        }
    }
}