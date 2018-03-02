using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    public class Player : Entity
    {
        public int health;
        public bool attacker;
        private string _assetName;
        private string _assetNameHurt;
        private string _assetNameDead;
        private Vector2 _normalPosition;
        private Vector2 _adjustedPosition;
        
        public Player(EntityDrawData drawData, string assetName, string assetNameHurt, string assetNameDead, int Health, bool Attacker)
            : base(drawData, assetName)
        {
            health = Health;
            attacker = Attacker;
            _assetName = assetName;
            _assetNameHurt = assetNameHurt;
            _assetNameDead = assetNameDead;
            _normalPosition = Position();
            _position = Position();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (attacker == false)
            {
                if (health == 2) _texture = _drawData.LoadTexture(_assetName);
                else if (health == 1) _texture = _drawData.LoadTexture(_assetNameHurt);
                else if (health <= 0) _texture = _drawData.LoadTexture(_assetNameDead);

                _adjustedPosition = _normalPosition;
            }
            else
            {
                _adjustedPosition = new Vector2(1000, 1000);
                
            }

            _position = _adjustedPosition;
            base.Update(gameTime);
        }

        public Vector2 Position()
        {
            Vector2 pos;
            pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
            return pos;
        }

       
    }
}
