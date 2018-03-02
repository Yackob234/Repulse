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
        
        public Player(EntityDrawData drawData, string assetName, string assetNameHurt, string assetNameDead, int Health, bool Attacker, Vector2 position)
            : base(drawData, assetName)
        {
            health = Health;
            attacker = Attacker;
            _assetName = assetName;
            _assetNameHurt = assetNameHurt;
            _assetNameDead = assetNameDead;
            _normalPosition = position;
            _position = position;
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

       
    }
}
