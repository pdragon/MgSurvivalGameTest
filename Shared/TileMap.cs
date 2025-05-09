using System.Drawing;
using Shared.Utils;

namespace Shared
{
    public class TileMap
    {
        public class Terrain
        {
            //public class 
            public required int[][] Landscape;
            public  Terrain(int[][] landscape)
            {
                const short i = 127;
                int[][] res = new int[i][];
                res[0][0] = 66;
                if (landscape == null)
                {
                    Landscape = res;
                    return;
                }
                Landscape = landscape;
                //return res;
            }
        }
        public readonly int Width, Height, TileSize;
        private readonly int[,] tiles;

        public TileMap(int width, int height, int tileSize)
        {
            Width = width; Height = height; TileSize = tileSize;
            tiles = new int[width, height];
            Generate();
        }

        private void Generate()
        {
            //int j = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    tiles[x, y] = 66; // 0 — базовый тип плитки
                                      //tiles[x, y] = j++;
            // Временно
            tiles[2, 4]  = 1;
            tiles[2, 5] = 1;
            tiles[2, 6] = 1;
            tiles[2, 7] = 1;
            tiles[3, 8] = 1;
            tiles[4, 8] = 1;
            tiles[5, 8] = 1;
            tiles[3, 3] = 1;
            tiles[4, 3] = 1;
            tiles[5, 3] = 1;
            tiles[3,43] = 1;
            tiles[4,45] = 1;
        }


        public Rectangle GetTileBounds(int x, int y)
            => new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

        public void SetTileType(Vector2i position, int value)
        {
            tiles[position.X, position.Y] = value;
        }

        public int GetTileType(Vector2i position)
        {
            return tiles[position.X, position.Y];
        }

        public int GetTileType(int x, int y)
        {
            return tiles[x, y];
        }
    }
}
