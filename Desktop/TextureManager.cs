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

        // Загрузка отдельной текстуры
        public void LoadTexture(string id, string assetPath)
        {
            Textures[id] = _content.Load<Texture2D>(assetPath);
        }

        // Загрузка атласа с определением регионов
        public void LoadAtlas(string atlasId, string assetPath, Dictionary<string, Rectangle> regions)
        {
            var atlasTexture = _content.Load<Texture2D>(assetPath);
            foreach (var region in regions)
            {
                string textureId = $"{atlasId}_{region.Key}";
                Textures[textureId] = ExtractRegion(atlasTexture, region.Value);
            }
        }

        // Извлечение региона из атласа и создание отдельной текстуры
        private Texture2D ExtractRegion(Texture2D atlas, Rectangle region)
        {
            Color[] data = new Color[region.Width * region.Height];
            atlas.GetData(0, region, data, 0, data.Length);

            Texture2D newTexture = new Texture2D(atlas.GraphicsDevice, region.Width, region.Height);
            newTexture.SetData(data);
            return newTexture;
        }

        // Получение текстуры по ID
        public Texture2D GetTexture(string id)
        {
            if (!Textures.ContainsKey(id))
            {
                return Textures["error_error"];
            }
            return Textures.TryGetValue(id, out var texture) ? texture : null;
        }
    }
}
