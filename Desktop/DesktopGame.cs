using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;
using Shared.Utils;
using Shared;
using Desktop.Extensions;
using Shared.Inventory;
using Shared.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Desktop
{
    public class DesktopGame : Game
    {
        private bool IsMiddleButtonPressed = false;
        private System.TimeSpan _lastToggleTime;
        private const double ToggleCooldown = 0.3; // Задержка в секундах

        private readonly GameCore core = new GameCore();
        private GraphicsDeviceManager gfx;
        private SpriteBatch sb;
        private Texture2D tileTex, playerTex, TileSet;
        private GameTileSet TileManager = new GameTileSet();
        public static TextureManager TextureManager;

        private SpriteFont UIFont;
        private Texture2D SlotTexture;
        private Texture2D PanelTexture;
        private InventoryPanel InventoryPanel;
        private CharacterInventory CharacterInventory;
        //public IconManager Icons { get; private set; }
        public const string MAIN_ATHLAS_NAME = "main_atlas";

        private bool IsMiddleButtonPreviouslyPressed = false;

        public DesktopGame()
        {
            gfx = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            InventoryPanel = new InventoryPanel();
            //Icons = new IconManager();
            //CharacterInventory = new CharacterInventory(40);
            core.Initialize();
            base.Initialize();
        }

        private void LoadInventory()
        {
            //Icons.LoadIcons(Content);
            UIFont = Content.Load<SpriteFont>("UI/Font");
            SlotTexture = Content.Load<Texture2D>("UI/slot");
            PanelTexture = Content.Load<Texture2D>("UI/panel");

            CharacterInventory = new CharacterInventory(40);
            CharacterInventory.AddItem(new InventoryItem("sword", "Steel Sword", MAIN_ATHLAS_NAME + "_sword", 1));
            CharacterInventory.AddItem(new InventoryItem("potion", "Health Potion", "potion_potion", 5));
            CharacterInventory.AddItem(new InventoryItem("arrow", "Arrows", MAIN_ATHLAS_NAME + "_arrow", 50));
            InventoryPanel.Initialize(CharacterInventory, PanelTexture, SlotTexture, UIFont);
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            //tileTex = Content.Load<Texture2D>("tile");
            TileSet = Content.Load<Texture2D>("terrain/output_tileset");
            TileManager.LoadTileset(new Vector2i(TileSet.Width, TileSet.Height));
            //tileTex = Content.Load<Texture2D>("terrain/output_tileset");
            playerTex = Content.Load<Texture2D>("player");

            TextureManager = new TextureManager(Content);

            // Загрузка отдельных текстур
            TextureManager.LoadTexture("player", "player");
            TextureManager.LoadTexture("ui_slot", "UI/slot");

            // Загрузка атласа с регионами
            var tileRegions = new Dictionary<string, Rectangle>
            {
                { "grass",  new Rectangle(0, 0, 32, 32)    },
                { "water",  new Rectangle(32, 0, 32, 32)   },
                { "sword",  new Rectangle(64, 0, 32, 32)   },
                //{ "potion", new Rectangle(64, 0, 20, 20)   },
                { "arrow",  new Rectangle(100, 0, 120, 20) }

            };

            var potionRegions = new Dictionary<string, Rectangle>
            {
                { "potion",  new Rectangle(32, 0, 16, 16)    },
            };

            var errorRegion = new Dictionary<string, Rectangle>
            {
                { "error",  new Rectangle(0, 0, 16, 16)},
            };

            TextureManager.LoadAtlas($"{MAIN_ATHLAS_NAME}", "terrain/output_tileset", tileRegions);
            TextureManager.LoadAtlas("potion", "UI/icons/potion", potionRegions);
            TextureManager.LoadAtlas("error", "UI/icons/error", errorRegion);
            LoadInventory();
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mouse = Mouse.GetState();
            Point? click = null;

            // Обработка открытия с задержкой
            if (mouse.MiddleButton == ButtonState.Pressed && !IsMiddleButtonPressed)
            {
                if ((gameTime.TotalGameTime - _lastToggleTime).TotalSeconds >= ToggleCooldown)
                {
                    InventoryPanel.IsVisible = !InventoryPanel.IsVisible;
                    _lastToggleTime = gameTime.TotalGameTime;
                }
                IsMiddleButtonPressed = true;
            }
            else if (mouse.MiddleButton == ButtonState.Released)
            {
                IsMiddleButtonPressed = false;
            }

            // Закрытие при клике вне панели или по кнопке закрытия
            if (InventoryPanel.IsVisible && mouse.LeftButton == ButtonState.Pressed)
            {
                if (InventoryPanel.CheckCloseButtonClick(mouse.Position))
                {
                    InventoryPanel.IsVisible = false;
                }
                else if (!InventoryPanel.Bounds.Contains(mouse.Position))
                {
                    InventoryPanel.IsVisible = false;
                }
            }

            // Проверяем:
            // 1. Нажата левая кнопка мыши
            // 2. Окно активно (IsActive)
            // 3. Курсор находится в пределах видимой области окна (клиентские координаты)
            if (mouse.RightButton == ButtonState.Pressed && IsActive)
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
            InventoryPanel.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();
            // рендерим тайлы
            for (int x = 0; x < TileManager.TileMap.Width; x++)
                for (int y = 0; y < TileManager.TileMap.Height; y++)
                {
                    //var rect = TileManager.TileMap.GetTileBounds(x, y);
                    //// Получаем тип плитки (например, 0, 1, 2...)
                    //int tileType = 66;
                    //// Получаем регион из атласа
                    //Rectangle sourceRect = TileManager.TileRegions[tileType].ToMonoGame();
                    // Позиция на экране
                    Rectangle destRect = TileManager.TileMap.GetTileBounds(x, y).ToMonoGame();
                    //// Отрисовка
                    //sb.Draw(TileSet, destRect, sourceRect, Color.White);

                    int tileType = TileManager.TileMap.GetTileType(x, y);
                    string textureId = tileType switch
                    {
                        0 =>  "main_atlas_grass",
                        1 =>  "main_atlas_water",
                        66 => "main_atlas_grass",
                        _ =>  "main_atlas_error"
                    };

                    Texture2D texture = TextureManager.GetTexture(textureId);
                    sb.Draw(texture, destRect, Color.White);
                }
            // рендерим игрока
            sb.Draw(playerTex, core.Player.Position, Color.White);
            InventoryPanel.Draw(sb);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
