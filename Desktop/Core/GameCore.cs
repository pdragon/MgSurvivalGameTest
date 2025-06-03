using Shared;
using System.Xml.Linq;
using Desktop.Extensions;
using Desktop.Interactables;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Desktop.Classes;
using System;

namespace Desktop
{
    public class GameCore
    {
        public Player Player { get; private set; }
        public static TileMap Map { get; private set; }
        public List<IInteractable> Interactables { get; } = new List<IInteractable>();

        public GameCore(ushort[] tiles)
        {
            // Инициализация игрока и карты
            Player = new Player(5, 5, 200f);
            Map = new TileMap(tiles, 20, 20, 20);

            // Добавление тестового объекта
            AddTree(new Rectangle(20, 20, 20, 20));
        }

        public void Update(float deltaTime, Point? click)
        {
            // Обновление игрока
            if (click.HasValue)
            {
                Player.SetDestination(new Point(click.Value.X, click.Value.Y).ToSystemDrawing());
            }
            //Player.Update(deltaTime, Map);
            Player.Update(deltaTime);
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

        ///// <summary>
        ///// Матрица проходимости: true — если клетка не занята препятствием.
        ///// </summary>
        //public bool[,] WalkableMap
        //{
        //    get
        //    {

        //        int w = Map.Width, h = Map.Height;
        //        var result = new bool[w, h];
        //        for (int x = 0; x < w; x++)
        //            for (int y = 0; y < h; y++)
        //            {
        //                // Здесь можно проверять свои объекты: если в ячейке есть препятствие — false
        //                // Пока просто считаем все плитки свободными:
        //                result[x, y] = true;    
        //            }
        //        return result;
        //    }
        //}

        /// <summary>
        /// Строит булеву карту проходимости: false там,
        /// где любой IInteractable перекрывает клетку.
        /// </summary>
        //public bool[,] BuildWalkableMap()
        //{
        //    int w = Map.Width, h = Map.Height, ts = Map.TileSize;
        //    var walkable = new bool[w, h];

        //    // Изначально всё проходимо
        //    for (int x = 0; x < w; x++)
        //        for (int y = 0; y < h; y++)
        //        {
        //            walkable[x, y] = true;
        //            ushort tileType = Map.Tiles[x, y];
        //            if (DesktopGame.Config.Assets.Terrain.Tiles.ContainsKey(tileType.ToString()))
        //            {
        //                walkable[x, y] = DesktopGame.Config.Assets.Terrain.Tiles[tileType.ToString()].Walkable;
        //                //walkable[x, y] = Map.Tiles[i].Walkable;
        //            }
        //        }


        //    // Для каждого объекта, блокируем клетки
        //    foreach (var obj in Interactables)
        //    {
        //        var rect = obj.Bounds;
        //        int minX = rect.X / ts, minY = rect.Y / ts;
        //        int maxX = (rect.Right - 1) / ts, maxY = (rect.Bottom - 1) / ts;

        //        for (int gx = minX; gx <= maxX; gx++)
        //            for (int gy = minY; gy <= maxY; gy++)
        //                if (gx >= 0 && gx < w && gy >= 0 && gy < h)
        //                    walkable[gx, gy] = false;
        //    }

        //    return walkable;
        //}

        /// <summary>
        /// Строит булеву карту проходимости: false там,
        /// где любой IInteractable перекрывает клетку.
        /// </summary>
        public bool[,] BuildWalkableMap()
        {
            int w = Map.Width, h = Map.Height, ts = Map.TileSize;
            var walkable = new bool[w, h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    walkable[x, y] = true;
                    ushort tileType = Map.Tiles[x, y];
                    if (DesktopGame.Config.Assets.Terrain.Tiles.TryGetValue(tileType.ToString(), out var tile))
                    {
                        walkable[x, y] = tile.Walkable;
                    }
                }

            foreach (var obj in Interactables)
            {
                var rect = obj.Bounds;
                int minX = Math.Clamp(rect.X / ts, 0, w - 1);
                int minY = Math.Clamp(rect.Y / ts, 0, h - 1);
                int maxX = Math.Clamp((rect.Right - 1) / ts, 0, w - 1);
                int maxY = Math.Clamp((rect.Bottom - 1) / ts, 0, h - 1);

                for (int gx = minX; gx <= maxX; gx++)
                    for (int gy = minY; gy <= maxY; gy++)
                        walkable[gx, gy] = false;
            }
            return walkable;
        }
    }
}