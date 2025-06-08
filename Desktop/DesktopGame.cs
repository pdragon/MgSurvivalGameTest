using Shared;
using Shared.Utils;
using Desktop.Inventory;
using Desktop.Extensions;
using Desktop.UI.Inventory;
using Desktop.Interactables;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;
using System.Buffers;
using System;
using Desktop.Core;
using System.IO;
using Desktop.Classes;
using Shared.Pathfinding;

namespace Desktop
{
    public class DesktopGame : Game
    {
        private KeyboardState PrevKeyboard;
        private MouseState PrevMouse;

        private readonly GameCore Core;
        private Camera Camera;

        private bool IsMiddleButtonPressed = false;
        private System.TimeSpan _lastToggleTime;
        private const double ToggleCooldown = 0.3; // Задержка в секундах
        private const float CameraSpeed = 300f;

        private GraphicsDeviceManager gfx;
        private SpriteBatch sb;
        private Texture2D playerTex, TileSet;
        private GameTileSet TileManager = new GameTileSet();
        public static TextureManager TextureManager;

        private SpriteFont UIFont;
        private Texture2D SlotTexture;
        private Texture2D PanelTexture;
        private InventoryPanel InventoryPanel;
        private CharacterInventory CharacterInventory;
        //public IconManager Icons { get; private set; }
        public const string MAIN_ATHLAS_NAME = "main_atlas";
        public static ConfigFile Config;

        private bool IsMiddleButtonPreviouslyPressed = false;
        private HotbarPanel HotbarPanel;

        private Texture2D defaultCursorTexture;
        private Texture2D actionCursorTexture;
        private ConfigFile.TerrainClass Terrain;

        // Для отображения линий перехода
        private List<Vector2> currentPath = new List<Vector2>();
        private Texture2D pathPixel;

        public DesktopGame()
        {
            var loader = new AssetsDataLoader();
            Config = loader.GetFromJson();
            Core = new GameCore(Config.Assets.Terrain.TerrainTiles);
            gfx = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //IsMouseVisible = true;
            IsMouseVisible = false;                   // скрываем системный курсор
        }

        protected override void Initialize()
        {
            InventoryPanel = new InventoryPanel();
            int mapPixelWidth = GameCore.Map.Width * GameCore.Map.TileSize;
            int mapPixelHeight = GameCore.Map.Height * GameCore.Map.TileSize;
            Camera = new Camera(GraphicsDevice, mapPixelWidth, mapPixelHeight);
            base.Initialize();
        }

        private void LoadInventory()
        {
            //Icons.LoadIcons(Content);
            UIFont = Content.Load<SpriteFont>(Config.Assets.Ui.Font);
            SlotTexture = Content.Load<Texture2D>(Config.Assets.Ui.Slot);
            PanelTexture = Content.Load<Texture2D>(Config.Assets.Ui.Panel);

            CharacterInventory = new CharacterInventory(40);
            //CharacterInventory.AddItem(new InventoryItem("sword", "Steel Sword", MAIN_ATHLAS_NAME + "_sword", 1));
            //CharacterInventory.AddItem(new InventoryItem("potion", "Health Potion", "potion_potion", 5));
            //CharacterInventory.AddItem(new InventoryItem("arrow", "Arrows", MAIN_ATHLAS_NAME + "_arrow", 50));
            CharacterInventory.AddItem(new InventoryItem(Config.Assets.Items["sword"], 1));
            CharacterInventory.AddItem(new InventoryItem(Config.Assets.Items["potion"], 5));
            CharacterInventory.AddItem(new InventoryItem(Config.Assets.Items["arrow"], 50));
            InventoryPanel.Initialize(CharacterInventory, PanelTexture, SlotTexture, UIFont, GraphicsDevice);

            // Создаем панель с передачей GraphicsDevice
            HotbarPanel = new HotbarPanel(GraphicsDevice);
            HotbarPanel.Initialize(
                CharacterInventory,
                SlotTexture,
                Content.Load<Texture2D>(Config.Assets.Ui.SelectionFrame),
                UIFont
            );
        }

        protected override void LoadContent()
        {
            // Создаем текстуру для рисования линий
            pathPixel = new Texture2D(GraphicsDevice, 1, 1);
            pathPixel.SetData(new[] { Color.White });

            sb = new SpriteBatch(GraphicsDevice);
            TileSet = Content.Load<Texture2D>("terrain/output_tileset");
            TileManager.LoadTileset(new Vector2i(TileSet.Width, TileSet.Height));

            playerTex = Content.Load<Texture2D>("player");

            TextureManager = new TextureManager(Content);

            // Загрузка отдельных текстур
            TextureManager.LoadTexture("player", "player");
            TextureManager.LoadTexture("ui_slot", Config!.Assets.Ui.Slot);

            var potionRegions = new Dictionary<string, Rectangle>
            {
                { "potion",  new Rectangle(32, 0, 16, 16)    },
            };

            var errorRegion = new Dictionary<string, Rectangle>
            {
                { "error",  new Rectangle(0, 0, 16, 16)},
            };
            TextureManager.LoadAtlas();

            //TextureManager.LoadAtlas($"{MAIN_ATHLAS_NAME}", "terrain/output_tileset", tileRegions);
            TextureManager.LoadAtlas("potion", "UI/icons/potion", potionRegions);
            TextureManager.LoadAtlas("error", "UI/icons/error", errorRegion);
            LoadInventory();

            // Курсоры
            defaultCursorTexture = Content.Load<Texture2D>("UI/cursor_default");
            actionCursorTexture = Content.Load<Texture2D>("UI/cursor_action");
        }

        private void DrawPath()
        {
            if (currentPath.Count < 2) return;

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                DrawLine(sb, currentPath[i], currentPath[i + 1], Color.Red, 2);
            }
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            sb.Draw(pathPixel,
                start,
                null,
                color,
                angle,
                new Vector2(0, 0.5f),
                new Vector2(length, thickness),
                SpriteEffects.None,
                0);
        }

        protected override void Update(GameTime gameTime)
        {
            Input(gameTime);

            base.Update(gameTime);
            InventoryPanel.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // ————— Мир + персонаж (с учётом смещения камеры) —————
            sb.Begin(transformMatrix: Camera.ViewMatrix);
            DrawMap();
            sb.Draw(playerTex, Core.Player.Position, Color.White);
            sb.End();

            // ————— UI + курсор (без смещения) —————
            sb.Begin(); // без transformMatrix!
            //HotbarPanel.Draw(sb);
            if (InventoryPanel.IsVisible)
                InventoryPanel.Draw(sb);

            // рисуем курсор ровно в позиции экрана
            var ms = Mouse.GetState();
            sb.Draw(
                defaultCursorTexture,
                ms.Position.ToVector2(),
                Color.White
            );
            sb.End();

            sb.Begin();
                DrawPath();
            sb.End();
            base.Draw(gameTime);
        }

        private void DrawMap()
        {
            // рендерим тайлы
            //for (int x = 0; x < TileManager.TileMap!.Width; x++)
            for (int x = 0; x < GameCore.Map.Width; x++)
                //for (int y = 0; y < TileManager.TileMap.Height; y++)
                for (int y = 0; y < GameCore.Map.Height; y++)
                {
                    // Позиция на экране
                    Rectangle destRect = GameCore.Map.GetTileBounds(x, y).ToMonoGame();
                    string textureId = "-1";
                    int tileType = GameCore.Map.GetTileType(x, y);
                    if (Config.Assets.Terrain.Tiles.ContainsKey(tileType.ToString()))
                    {
                        textureId = Config.Assets.Terrain.Tiles[tileType.ToString()].TileId;
                        string atlasId = Config.Assets.Terrain.Tiles[tileType.ToString()].Atlas;
                        if(atlasId != null)
                        {
                            textureId = $"{atlasId}_{textureId}";
                        }
                    }

                    Texture2D texture = TextureManager.GetTexture(textureId);
                    sb.Draw(texture, destRect, Color.White);
                }
        }

        private void DrawCursor()
        {
            // 1) экранная позиция мыши
            var mouseState = Mouse.GetState();
            var mouseScreenPos = mouseState.Position.ToVector2();

            // 2) вычисляем мировую позицию (для попадания в Bounds)
            var mouseWorldPos = GetMouseWorldPosition();

            // 3) находим первый интерактивный объект под курсором
            IInteractable target = Core.Interactables
                .FirstOrDefault(obj => obj.Bounds.Contains(mouseWorldPos));

            // 4) проверяем CanInteract
            var selectedItem = HotbarPanel.GetSelectedItem();
            bool canAction = target != null && target.CanInteract(selectedItem);

            // 5) рисуем нужный курсор
            var tex = canAction ? actionCursorTexture : defaultCursorTexture;
            sb.Draw(tex, mouseScreenPos, Color.White);
        }

        private void Input(GameTime gameTime)
        {
            var currentKeyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();
            var currentMouse = Mouse.GetState();
            Point? click = null;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 camDelta = Vector2.Zero;
            if (currentKeyboard.IsKeyDown(Keys.Left)) camDelta.X -= CameraSpeed * dt;
            if (currentKeyboard.IsKeyDown(Keys.Right)) camDelta.X += CameraSpeed * dt;
            if (currentKeyboard.IsKeyDown(Keys.Up)) camDelta.Y -= CameraSpeed * dt;
            if (currentKeyboard.IsKeyDown(Keys.Down)) camDelta.Y += CameraSpeed * dt;

            Camera.Move(camDelta);

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
                    //click = new Point(mouse.X, mouse.Y);
                    click = GetMouseWorldPosition().ToPoint();
                    Console.WriteLine(click.ToString());
                }
            }
            Core.Update(dt, click);

            //Hotbar
            // Выбор слота цифрами 1-9
            //var keyboard = Keyboard.GetState();
            for (int i = 0; i < 9; i++)
            {
                if (currentKeyboard.IsKeyDown(Keys.D1 + i) && PrevKeyboard.IsKeyUp(Keys.D1 + i))
                {
                    HotbarPanel.SelectSlot(i);
                }
            }

            // Выбор слота колёсиком мыши
            if (mouse.ScrollWheelValue > PrevMouse.ScrollWheelValue)
            {
                HotbarPanel.SelectSlot(HotbarPanel.SelectedSlotIndex - 1);
            }
            else if (mouse.ScrollWheelValue < PrevMouse.ScrollWheelValue)
            {
                HotbarPanel.SelectSlot(HotbarPanel.SelectedSlotIndex + 1);
            }

            
            // Обработка ПКМ с учётом выбранного предмета
            if (mouse.RightButton == ButtonState.Pressed && IsActive)
            {
                var selectedItem = HotbarPanel.GetSelectedItem();
                var mouseWorldPos = GetMouseWorldPosition(); // Метод для преобразования экранных координат в игровые

                // Path
                // 1) Получаем клик в мировых координатах и преобразуем в сетку:
                var mouseWorld = GetMouseWorldPosition(); // Vector2 из MonoGame
                var clickCell = new GridPoint(
                    (int)(mouseWorld.X / GameCore.Map.TileSize),
                    (int)(mouseWorld.Y / GameCore.Map.TileSize));

                // 2) Стартовый и целевой узлы:
                var start = new GridPoint(
                    (int)(Core.Player.Position.X / GameCore.Map.TileSize),
                    (int)(Core.Player.Position.Y / GameCore.Map.TileSize));
                var goal = clickCell;

                // 3) Строим карту проходимости (bool[,]) и ищем путь:
                bool[,] walkable = Core.BuildWalkableMap(); // ваш метод, учитывающий препятствия
                var rawPath = AStar.FindPath(walkable, start, goal);
                var path = AStar.SmoothPath(rawPath, walkable);

                // 4) Передаём маршрут игроку:
                Core.Player.SetPath(path);
                // Рисуем путь
                currentPath.Clear();
                if (path.Count > 0)
                {
                    foreach (var point in path)
                    {
                        currentPath.Add(new Vector2(
                            point.X * GameCore.Map.TileSize + GameCore.Map.TileSize / 2,
                            point.Y * GameCore.Map.TileSize + GameCore.Map.TileSize / 2
                        ));
                    }
                }
                // ----------Path


                if (selectedItem != null)
                {
                    // Проверка, можно ли взаимодействовать с объектом
                    var target = GetTargetAtPosition(mouseWorldPos);
                    if (target != null && target.CanInteract(selectedItem))
                    {
                        // Запуск действия (например, рубка дерева)
                        target.OnInteract(selectedItem, Core.Player);
                    }
                    else
                    {
                        // Обычное перемещение персонажа
                        //Core.Player.SetDestination(mouseWorldPos.ToPoint().ToSystemDrawing());
                    }
                }
                else
                {
                    // Обычное перемещение персонажа
                    Core.Player.SetDestination(mouseWorldPos.ToPoint().ToSystemDrawing());
                }
            }
            // Обновление предыдущих состояний
            PrevKeyboard = currentKeyboard;
            PrevMouse = currentMouse;
            base.Update(gameTime);
        }

        private Vector2 GetMouseWorldPosition()
        {
            var ms = Mouse.GetState();
            // Единственный вызов Invert с текущей матрицей камеры
            return Vector2.Transform(
                new Vector2(ms.X, ms.Y),
                Matrix.Invert(Camera.ViewMatrix)
            );
        }

        private IInteractable GetTargetAtPosition(Vector2 worldPos)
        {
            foreach (var obj in Core.Interactables)
            {
                if (obj.Bounds.Contains(worldPos))
                {
                    return obj;
                }
            }
            return null;
        }
    }
}
