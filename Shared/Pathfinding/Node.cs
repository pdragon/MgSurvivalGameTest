using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Pathfinding
{
    /// <summary>
    /// Поле Parent позволяет, дойдя до цели, восстановить путь в обратном порядке (https://www.redblobgames.com/pathfinding/a-star/implementation.html)
    /// Эвристика:
    /// Манхэттен (для 4-направленной сетки): H = |x1 - x2| + |y1 - y2| (https://www.redblobgames.com/pathfinding/grids/algorithms.html)
    /// Евклидово (если разрешены диагонали): H = √((x1 - x2)² + (y1 - y2)²) (https://www.redblobgames.com/pathfinding/a-star/introduction.html)
    /// </summary>
    public class Node
    {
        public GridPoint Position;          // координаты в сетке
        public float G, H;              // g = стоимость до узла, h = эвристика до цели
        public Node? Parent;             // предыдущий узел в пути

        public float F => G + H;        // общий приоритет
        public Node(GridPoint pos) { Position = pos; G = float.MaxValue; H = 0; Parent = null; }
    }
}
