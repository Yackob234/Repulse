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
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            timer++;
            if (timer >= timerLimit) timer = 0;

        }

        public void StageUpdate()
        {
            //changes the stage for all of the entity classes
            Stage++;
            if (Stage > 3)
            {
                Stage = 1;
            }
        }
        protected int Stage = 1;
        protected static int timer = 0;
        protected static int timerLimit = 500;
        protected Vector2 _position;
        protected EntityDrawData _drawData;
        protected Texture2D _texture;
    }
}
