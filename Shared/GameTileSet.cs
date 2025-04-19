using Shared;
using Shared.Utils;
using System.Drawing;
using System.Numerics;

namespace Desktop
{
    public class GameTileSet
    {
        public Rectangle[] TileRegions { get; private set; }
        private int tileWidth = 20;
        private int tileHeight = 20;
        public TileMap TileMap;
        
        public void LoadTileset(Vector2i textureSize)
        {
            TileMap = new TileMap(textureSize.X / tileWidth, textureSize.Y / tileHeight, tileWidth);
            int tilesPerRow = textureSize.X / tileWidth;
            int tilesPerColumn = textureSize.Y / tileHeight;
            TileRegions = new Rectangle[tilesPerRow * tilesPerColumn];

            for (int x = 0; x < tilesPerColumn; x++)
            {
                for (int y = 0; y < tilesPerRow; y++)
                {
                    int index = x * tilesPerRow + y;
                    TileRegions[index] = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight
                    );
                }
            }
        }
    }
}
