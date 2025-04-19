using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;
using Shared.Utils;
using Shared;
using Desktop.Extensions;

namespace Desktop
{
    public class DesktopGame : Game
    {
        private readonly GameCore core = new GameCore();
        private GraphicsDeviceManager gfx;
        private SpriteBatch sb;
        private Texture2D tileTex, playerTex, TileSet;
        private GameTileSet TileManager = new GameTileSet();

        public DesktopGame()
        {
            gfx = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            core.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            //tileTex = Content.Load<Texture2D>("tile");
            TileSet = Content.Load<Texture2D>("terrain/output_tileset");
            TileManager.LoadTileset(new Vector2i(TileSet.Width, TileSet.Height));
            //tileTex = Content.Load<Texture2D>("terrain/output_tileset");
            playerTex = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mouse = Mouse.GetState();
            Point? click = null;
            if (mouse.LeftButton == ButtonState.Pressed && IsActive)
            {
                // Используем клиентские координаты окна
                if (mouse.X >= 0 && mouse.X < GraphicsDevice.Viewport.Width &&
                    mouse.Y >= 0 && mouse.Y < GraphicsDevice.Viewport.Height)
                {
                    click = new Point(mouse.X, mouse.Y);
                }
            }
            core.Update(dt, click);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();
            // рендерим тайлы
            for (int x = 0; x < TileManager.TileMap.Width; x++)
                for (int y = 0; y < TileManager.TileMap.Height; y++)
                {
                    var rect = TileManager.TileMap.GetTileBounds(x, y);
                    //sb.Draw(tileTex, rect.ToMonoGame(), Color.White);
                    // Получаем тип плитки (например, 0, 1, 2...)
                    //int tileType = core.Map.GetTileType(x, y);
                    //int tileType = TileManager.TileMap.GetTileType(x, y);
                    int tileType = 66;
                    // Получаем регион из атласа
                    Rectangle sourceRect = TileManager.TileRegions[tileType].ToMonoGame();
                    // Позиция на экране
                    Rectangle destRect = TileManager.TileMap.GetTileBounds(x, y).ToMonoGame();
                    // Отрисовка
                    sb.Draw(TileSet, destRect, sourceRect, Color.White);
                }
            // рендерим игрока
            sb.Draw(playerTex, core.Player.Position, Color.White);
            sb.End();
            base.Draw(gameTime);
        }
    }


}
