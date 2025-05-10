using System;
using System.Collections.Generic;

namespace Shared.Pathfinding
{
    public static class AStar
    {
        // 4-направленная сетка (можно расширить до 8-направленной, добавив диагонали)
        private static readonly GridPoint[] Directions = {
            new GridPoint(1, 0), new GridPoint(-1, 0),
            new GridPoint(0, 1), new GridPoint(0, -1),
            new GridPoint(1, 1), new GridPoint(1, -1),
            new GridPoint(-1, 1), new GridPoint(-1, -1)
        };

        public static List<GridPoint> SmoothPath(List<GridPoint> path, bool[,] walkable)
        {
            if (path.Count < 3) return path;

            var smoothed = new List<GridPoint> { path[0] };
            int lastValid = 0;

            for (int i = 2; i < path.Count; i++)
            {
                if (!IsWalkableBetween(smoothed.Last(), path[i], walkable))
                {
                    smoothed.Add(path[i - 1]);
                    lastValid = i - 1;
                }
            }

            smoothed.Add(path.Last());
            return smoothed;
        }

        private static bool IsWalkableBetween(GridPoint a, GridPoint b, bool[,] walkable)
        {
            // Алгоритма Брезенхема для проверки прямой видимости
            int dx = Math.Abs(b.X - a.X);
            int dy = Math.Abs(b.Y - a.Y);
            int sx = a.X < b.X ? 1 : -1;
            int sy = a.Y < b.Y ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                if (a.X == b.X && a.Y == b.Y) break;
                if (!walkable[a.X, a.Y]) return false;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    a.X += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    a.Y += sy;
                }
            }

            return true;
        }

        /// <summary>
        /// Находит путь от start до goal по булевой карте проходимости.
        /// </summary>
        public static List<GridPoint> FindPath(
            bool[,] walkable,
            GridPoint start,
            GridPoint goal)
        {
            int width = walkable.GetLength(0);
            int height = walkable.GetLength(1);

            var openSet = new PriorityQueue<Node, float>();
            var allNodes = new Dictionary<(int, int), Node>();
            var closedSet = new HashSet<(int, int)>();

            Node GetNode(GridPoint p)
            {
                var key = (p.X, p.Y);
                if (!allNodes.TryGetValue(key, out var node))
                {
                    node = new Node(p);
                    allNodes[key] = node;
                }
                return node;
            }

            // Инициализация старта
            var startNode = GetNode(start);
            startNode.G = 0;
            startNode.H = Heuristic(start, goal);
            openSet.Enqueue(startNode, startNode.F);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                if (current.Position.X == goal.X && current.Position.Y == goal.Y)
                    return ReconstructPath(current);

                closedSet.Add((current.Position.X, current.Position.Y));

                foreach (var dir in Directions)
                {
                    var neighborPos = new GridPoint(current.Position.X + dir.X, current.Position.Y + dir.Y);

                    // Проверка границ
                    if (neighborPos.X < 0 || neighborPos.X >= width ||
                        neighborPos.Y < 0 || neighborPos.Y >= height)
                        continue;

                    // Нельзя пройти
                    if (!walkable[neighborPos.X, neighborPos.Y])
                        continue;

                    // Диагональное движение возможно только если соседние клетки свободны
                    if (Math.Abs(dir.X) + Math.Abs(dir.Y) == 2)
                    {
                        // Проверяем, не блокированы ли соседние клетки
                        bool isBlockedX = !walkable[current.Position.X + dir.X, current.Position.Y];
                        bool isBlockedY = !walkable[current.Position.X, current.Position.Y + dir.Y];
                        if (isBlockedX || isBlockedY) continue;
                    }

                    // Уже обработан
                    if (closedSet.Contains((neighborPos.X, neighborPos.Y)))
                        continue;

                    //float tentativeG = current.G + 1; 
                    // стоимость движения — 1 за шаг
                    float moveCost = (Math.Abs(dir.X) + Math.Abs(dir.Y) == 2) ? 1.414f : 1f;
                    float tentativeG = current.G + moveCost;

                    var neighbor = GetNode(neighborPos);

                    if (tentativeG < neighbor.G)
                    {
                        neighbor.G = tentativeG;
                        neighbor.H = Heuristic(neighborPos, goal);
                        neighbor.Parent = current;
                        openSet.Enqueue(neighbor, neighbor.F);  // .NET PriorityQueue O(log n) :contentReference[oaicite:5]{index=5}
                    }
                }
            }

            // Путь не найден
            return new List<GridPoint>();
        }

        // Манхэттенская эвристика для 4-направленной сетки
        //private static float Heuristic(GridPoint a, GridPoint b)
        //    => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);  // :contentReference[oaicite:6]{index=6}

        // Эвклидова эвристика для 8-направленной сетки
        private static float Heuristic(GridPoint a, GridPoint b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        // Восстановление пути по ссылкам Parent
        private static List<GridPoint> ReconstructPath(Node? node)
        {
            var path = new List<GridPoint>();
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}
