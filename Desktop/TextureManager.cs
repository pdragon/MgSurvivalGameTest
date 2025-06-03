using Desktop.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Desktop
{
    public class TextureManager
    {
        private readonly ContentManager Content;
        private readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        //private readonly Dictionary<string, AtlasDefinition> _atlases = new Dictionary<string, AtlasDefinition>();

        public TextureManager(ContentManager content)
        {
            Content = content;
        }

        /// <summary>
        /// Загрузка отдельной текстуры
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetPath"></param>
        public void LoadTexture(string id, string assetPath)
        {
            Textures[id] = Content.Load<Texture2D>(assetPath);
        }

        /// <summary>
        /// Загрузка атласа с определением регионов
        /// </summary>
        /// <param name="atlasId"></param>
        /// <param name="assetPath"></param>
        /// <param name="regions"></param>
        public void LoadAtlas(string atlasId, string assetPath, Dictionary<string, Rectangle> regions)
        {
            var atlasTexture = Content.Load<Texture2D>(assetPath);
            foreach (var region in regions)
            {
                string textureId = $"{atlasId}_{region.Key}";
                Textures[textureId] = ExtractRegion(atlasTexture, region.Value);
            }
        }

        public void LoadAtlas()
        {
            foreach (var tile in AssetsDataLoader.Config!.Assets.Terrain.Tiles)
            {
                var atlasKey = tile.Value.Atlas;
                if (atlasKey == null)
                {
                    //TODO: Читать из файла одиночного а не спрайт щет
                    continue;
                }
                if (AssetsDataLoader.Config.Assets.Atlases[atlasKey].Texture == null)
                {
                    AssetsDataLoader.Config.Assets.Atlases[atlasKey].Texture = Content.Load<Texture2D>(AssetsDataLoader.Config.Assets.Atlases[atlasKey].File);
                }
                //TextureManager.LoadAtlas(atlasKey, tile.Value.TileId, tile.Value.TexturePosition);
                string textureId = $"{atlasKey}_{tile.Value.TileId}";
                Textures[textureId] = ExtractRegion(AssetsDataLoader.Config.Assets.Atlases[atlasKey].Texture, tile.Value.TexturePosition);
            }
        }

        /// <summary>
        /// Извлечение региона из атласа и создание отдельной текстуры
        /// </summary>
        /// <param name="atlas"></param>
        /// <param name="region"></param>
        /// <returns></returns> 
        private Texture2D ExtractRegion(Texture2D atlas, Rectangle region)
        {
            Color[] data = new Color[region.Width * region.Height];
            atlas.GetData(0, region, data, 0, data.Length);

            Texture2D newTexture = new Texture2D(atlas.GraphicsDevice, region.Width, region.Height);
            newTexture.SetData(data);
            return newTexture;
        }

        /// <summary>
        ///  Получение текстуры по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Texture2D GetTexture(string id)
        {
            if (!Textures.ContainsKey(id))
            {
                return Textures["error_error"];
            }
            return Textures.TryGetValue(id, out var texture) ? texture : null;
        }

        /// <summary>
        /// Регистрирует новый регион в атласе под указанным ID.
        /// </summary>
        public void RegisterRegion(string id, Rectangle region, Texture2D atlas)
        {
            var tex = ExtractRegion(atlas, region);
            Textures[id] = tex;
        }
    }
}
