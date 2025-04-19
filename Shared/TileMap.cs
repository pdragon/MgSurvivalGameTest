using System.Drawing;
using Shared.Utils;

namespace Shared
{
    public class TileMap
    {
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
            int j = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    //tiles[x, y] = 0; // 0 — базовый тип тайла
                    tiles[x, y] = j++;
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
