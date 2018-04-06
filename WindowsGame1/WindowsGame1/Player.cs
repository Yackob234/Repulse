using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace repulse
{
    public class Player : Entity
    {
        public float slow = 0f;
        public float fast = 0f;
        public double timer = 0;
        public double timerLimit = 350;
        public int health;
        public bool attacker;
        private string _assetName;
        private string _assetNameHurt;
        private string _assetNameDead;
        private Vector2 _normalPosition;
        private Vector2 _adjustedPosition;
        public CharacterEnum _character;
        private bool _characterChanged = false;
        private int _num;
        private PlayerIndex playerNum;
        private bool _damage = false;
        private bool _newVib = true;
        
        public Player(EntityDrawData drawData, string assetName, string assetNameHurt, string assetNameDead, int Health, bool Attacker, CharacterEnum cha, int num)
            : base(drawData, assetName)
        {
            health = Health;
            attacker = Attacker;
            _assetName = assetName;
            _assetNameHurt = assetNameHurt;
            _assetNameDead = assetNameDead;
            _character = cha;
            _num = num;
            _normalPosition = Position();
            _position = Position();
            if(_num == 1)
            {
                playerNum = PlayerIndex.One;
            } else if (_num == 2)
            {
                playerNum = PlayerIndex.Two;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(_damage == true)
            {
                timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            checkHealth();
            shakeSlow();
            shake();
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
        public void shake()
        {
            if (timer > timerLimit)
            {
                _newVib = true;
                fast = 0.0f;
                timer = 0;
                _damage = false;
            }
            if (_newVib)
            {
                if (_num == 1)
                {
                    if (_drawData.Player1Controller == 4 || _drawData.Player1Controller == 5)
                    {
                        Console.WriteLine(fast);
                        GamePad.SetVibration(PlayerIndex.One, slow, fast);
                    }
                    if (_drawData.Player1Controller == 6 || _drawData.Player1Controller == 7)
                        GamePad.SetVibration(PlayerIndex.Two, slow, fast);
                }
                if (_num == 2)
                {
                    if (_drawData.Player2Controller == 4 || _drawData.Player2Controller == 5)
                        GamePad.SetVibration(PlayerIndex.One, slow, fast);
                    if (_drawData.Player2Controller == 6 || _drawData.Player2Controller == 7)
                        GamePad.SetVibration(PlayerIndex.Two, slow, fast);
                }
                _newVib = false;
            }
        }
        public void shakeSlow()
        {
            if (_drawData._lastStand && slow != 0.3f)
            {
                _newVib = true;
                slow = 0.3f;

                _damage = true;
                fast = 0.5f;
            } else if (_drawData._lastStand == false && slow != 0.0f)
            {
                _newVib = true;
                slow = 0.0f;
            }
        }
        public void shakeFast()
        {
            _newVib = true;
            _damage = true;
            fast = 0.5f;
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
