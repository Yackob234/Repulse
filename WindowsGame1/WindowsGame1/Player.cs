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
        public CharacterEnum _character;
        private bool _characterChanged = false;
        
        public Player(EntityDrawData drawData, string assetName, string assetNameHurt, string assetNameDead, int Health, bool Attacker, CharacterEnum cha)
            : base(drawData, assetName)
        {
            health = Health;
            attacker = Attacker;
            _assetName = assetName;
            _assetNameHurt = assetNameHurt;
            _assetNameDead = assetNameDead;
            _character = cha;
            _normalPosition = Position();
            _position = Position();
        }

        public override void Update(GameTime gameTime)
        {
            checkHealth();
            _position = _adjustedPosition;
            base.Update(gameTime);
        }

        public Vector2 Position()
        {
            //sets the position
            Vector2 pos = Vector2.Zero;

            
            pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
           
            return pos;
        }

       public void CharacterChanging(CharacterEnum cha)
       {
            //choosing stuff
            switch (cha)
            {
                case CharacterEnum.CuteGenji:
                    _assetName = "genji1";
                    _assetNameHurt = "genji1Dead";
                    _assetNameDead = "genji1deaddead";
                    break;
                case CharacterEnum.EvilGenji:
                    _assetName = "genji2";
                    _assetNameHurt = "genji2Dead";
                    _assetNameDead = "genji2deaddead";
                    break;
                case CharacterEnum.PixelGenji:
                    _assetName = "genji3";
                    _assetNameHurt = "genji3Dead";
                    _assetNameDead = "genji3deaddead";
                    break;
                case CharacterEnum.Reinhardt:
                    _assetName = "reinhardt";
                    _assetNameHurt = "reinhardtDead";
                    _assetNameDead = "reinhardtdeaddead";
                    break;
                case CharacterEnum.Torbjorn:
                    _assetName = "torbjorn";
                    _assetNameHurt = "torbjornDead";
                    _assetNameDead = "torbjorndeaddead";
                    break;
                case CharacterEnum.Mercy:
                    _assetName = "mercy";
                    _assetNameHurt = "mercyDead";
                    _assetNameDead = "mercydeaddead";
                    break;
                default:
                    _assetName = "mercy";
                    _assetNameHurt = "mercyDead";
                    _assetNameDead = "mercydeaddead";
                    break;
            }
            _characterChanged = true;

            Console.WriteLine(_assetName);
        }

       public void checkHealth()
       {
            //changes health
            if (_drawData.Stage == StageEnum.MainGameplay)
            {
                if (attacker == false)
                {
                    if (health == 2) _texture = _drawData.LoadTexture(_assetName);
                    else if (health == 1) _texture = _drawData.LoadTexture(_assetNameHurt);
                    else if (health == 0) _texture = _drawData.LoadTexture(_assetNameHurt);
                    else if (health <= -1) _texture = _drawData.LoadTexture(_assetNameDead);

                    _adjustedPosition = _normalPosition;
                }
                else
                {
                    _adjustedPosition = new Vector2(1000, 1000);
                }
                if (_characterChanged == true)
                {
                    _normalPosition = Position();
                    _characterChanged = false;
                }
            }

            else if (_drawData.Stage == StageEnum.EndScreen)
            {
                if (attacker == false)
                {
                    if (health == 2) _texture = _drawData.LoadTexture(_assetName);
                    else if (health == 1) _texture = _drawData.LoadTexture(_assetNameHurt);
                    else if (health == 0) _texture = _drawData.LoadTexture(_assetNameHurt);
                    else if (health <= -1) _texture = _drawData.LoadTexture(_assetNameDead);

                    _adjustedPosition = _normalPosition;
                }
                else
                {
                    _adjustedPosition = new Vector2(1000, 1000);
                }
            }
            else
            {
                _adjustedPosition = new Vector2(1000, 1000);
            }
        }
    }
}
