using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Desktop.Classes
{
    /// <summary>
    /// Свойства этого класса надо будет потом сделать, чтобы грузились из json файла.
    /// </summary>
    public class Item
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("desc")] public string Description { get; set; }
        [JsonProperty("icon_id")] public string IconId { get; set; }
    }

    public class ConfigFile
    {
        public class TerrainClass
        {
            [JsonProperty("tiles")] public Dictionary<string, TilesClass> Tiles { get; set; }

            public class TilesClass
            {
                [JsonProperty("tile_id")] public string TileId { get; set; }
                [JsonProperty("atlas")] public string Atlas { get; set; }
                //[JsonProperty("texture_position")] public Rectangle TexturePosition { get; set; }
                [JsonProperty("texture_position")] private int[] TexturePositionArray { get; set; }
                [JsonIgnore]
                public Rectangle TexturePosition =>
                  new Rectangle(
                      TexturePositionArray[0],
                      TexturePositionArray[1],
                      TexturePositionArray[2],
                      TexturePositionArray[3]
                  );
                public Texture2D Texture { get; set; }
            }
        }

        public class Atlases
        {
            [JsonProperty("atlases")] public Dictionary<string, Atlas> Atlas { get; set; }
        }

        public class Atlas
        {
            [JsonProperty("id")] public string Id { get; set; }
            [JsonProperty("file")] public string File { get; set; }
            public Texture2D Texture { get; set; } = null;
        }

        public class UI
        {
            [JsonProperty("path_SpriteFont")] public string Font { get; set; }
            [JsonProperty("path_Slot")] public string Slot { get; set; }
            [JsonProperty("path_Panel")] public string Panel { get; set; }
            [JsonProperty("path_SelectionFrame")] public string SelectionFrame { get; set; }
        }

        public class MainAssets
        {
            [JsonProperty("ui")] public UI Ui { get; set; }
            [JsonProperty("items")] public Dictionary<string, Item> Items { get; set; }
            [JsonProperty("terrain")] public TerrainClass Terrain { get; set; }
            [JsonProperty("atlases")] public Dictionary<string, Atlas> Atlases { get; set; }
        }

        [JsonProperty("assets")] public MainAssets Assets { get; set; }
    }
}
