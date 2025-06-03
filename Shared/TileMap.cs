using System.Drawing;
using Shared.Utils;

namespace Shared
{
    public class TileMap
    {
        public static ushort[] TilesTypeMap;
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
        public ushort[,] Tiles { get; private set; }

        public TileMap(ushort[] tilesTypeMap, int width, int height, int tileSize)
        {
            Width = width; Height = height; TileSize = tileSize;
            Tiles = new ushort[width, height];
            TilesTypeMap = tilesTypeMap;
            Generate();
        }

        private void Generate()
        {
            int currentTile = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (currentTile >= TilesTypeMap.Length)
                    {
                        Tiles[x, y] = 0;
                    }
                    else
                    {
                        Tiles[x, y] = TilesTypeMap[currentTile]; //tiles[x, y] = 66; // 0 — базовый тип плитки
                        currentTile++;
                    }
                }
            //tiles[x, y] = j++;
            // Временно
            //tiles[2, 4] = 1;
            //tiles[2, 5] = 1;
            //tiles[2, 6] = 1;
            //tiles[2, 7] = 1;
            //tiles[3, 8] = 1;
            //tiles[4, 8] = 1;
            //tiles[5, 8] = 1;
            //tiles[3, 3] = 1;
            //tiles[4, 3] = 1;
            //tiles[5, 3] = 1;
            //tiles[3,43] = 1;
            //tiles[4,45] = 1;
        }


        public Rectangle GetTileBounds(int x, int y)
            => new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

        public void SetTileType(Vector2i position, ushort value)
        {
            Tiles[position.X, position.Y] = value;
        }

        public int GetTileType(Vector2i position)
        {
            return Tiles[position.X, position.Y];
        }

        public int GetTileType(int x, int y)
        {
            return Tiles[x, y];
        }
    }
}
