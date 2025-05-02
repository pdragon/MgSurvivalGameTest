using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    public class TextureManager
    {
        private readonly ContentManager _content;
        private readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        //private readonly Dictionary<string, AtlasDefinition> _atlases = new Dictionary<string, AtlasDefinition>();

        public TextureManager(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// Загрузка отдельной текстуры
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetPath"></param>
        public void LoadTexture(string id, string assetPath)
        {
            Textures[id] = _content.Load<Texture2D>(assetPath);
        }

        /// <summary>
        /// Загрузка атласа с определением регионов
        /// </summary>
        /// <param name="atlasId"></param>
        /// <param name="assetPath"></param>
        /// <param name="regions"></param>
        public void LoadAtlas(string atlasId, string assetPath, Dictionary<string, Rectangle> regions)
        {
            var atlasTexture = _content.Load<Texture2D>(assetPath);
            foreach (var region in regions)
            {
                string textureId = $"{atlasId}_{region.Key}";
                Textures[textureId] = ExtractRegion(atlasTexture, region.Value);
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
