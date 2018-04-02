using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repulse
{
    public class Entity
    {
        public Entity(EntityDrawData drawData, string assetName)
        {
            _drawData = drawData;
            _texture = drawData.LoadTexture(assetName);

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, _position, Color.White);
            spriteBatch.Draw(_texture, _position, null, Color.White, 0.0f, new Vector2(0, 0), _scale, 0, 0);
        }

        public virtual void Update(GameTime gameTime)
        {

        }
        /*
        public void changeTextureSize(string assetName, int width, int height)
        {
            _texture.Height = height;
            _texture.Width = width;
        }
        */
        protected float _scale = 1.0f;
        protected Vector2 _position;
        protected EntityDrawData _drawData;
        protected Texture2D _texture;
    }
}
