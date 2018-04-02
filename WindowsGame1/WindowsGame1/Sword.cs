using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    class Weapon : Entity
    {
        private string _assetName;
        private Vector2 _speed;
        public DirectionEnum _direction;
        private CharacterEnum _cha;
        private bool characterChanged = false;

        public Weapon(EntityDrawData drawData, DirectionEnum direction, string assetName, CharacterEnum cha)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _direction = direction;
            _cha = cha;
        }

        public override void Update(GameTime gameTime)
        {
            if(characterChanged == true)
            {
                _texture = _drawData.LoadTexture(_assetName);
                resetWeaponPosition(_direction);
                characterChanged = false;
            }
            base.Update(gameTime);
        }

        public void characterChanging(CharacterEnum cha, DirectionEnum dir)
        {
            
            switch (cha)
            {
                case CharacterEnum.Mercy:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _assetName = "mercystaffU";
                            break;
                        case DirectionEnum.Down:
                            _assetName = "mercystaffR";
                            break;
                        case DirectionEnum.Left:
                            _assetName = "mercystaffL";
                            break;
                        case DirectionEnum.Right:
                            _assetName = "mercystaffR";
                            break;
                        default:
                            _assetName = null;
                            break;
                    }
                    break;
                case CharacterEnum.Reinhardt:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _assetName = "ReinHammerU";
                            break;
                        case DirectionEnum.Down:
                            _assetName = "ReinHammerD";
                            break;
                        case DirectionEnum.Left:
                            _assetName = "ReinHammerL";
                            break;
                        case DirectionEnum.Right:
                            _assetName = "ReinHammerR";
                            break;
                        default:
                            _assetName = null;
                            break;
                    }
                    break;
                case CharacterEnum.Torbjorn:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _assetName = "TorbHammerU";
                            break;
                        case DirectionEnum.Down:
                            _assetName = "TorbHammerD";
                            break;
                        case DirectionEnum.Left:
                            _assetName = "TorbHammerL";
                            break;
                        case DirectionEnum.Right:
                            _assetName = "TorbHammerR";
                            break;
                        default:
                            _assetName = null;
                            break;
                    }
                    break;
                default:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _assetName = "swordU";
                            break;
                        case DirectionEnum.Down:
                            _assetName = "swordR";
                            break;
                        case DirectionEnum.Left:
                            _assetName = "swordL";
                            break;
                        case DirectionEnum.Right:
                            _assetName = "swordR";
                            break;
                        default:
                            _assetName = null;
                            break;
                    }
                    break;
                                
            }
            characterChanged = true;
        }

        public void resetWeaponPosition(DirectionEnum dir)
        {
            //resets the weapon that was moved back to off screen
            Vector2 weaponPosition;
            switch (dir)
            {
                case DirectionEnum.Up:
                    weaponPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, - _texture.Height);
                    break;
                case DirectionEnum.Down:
                    weaponPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height);
                    break;
                case DirectionEnum.Left:
                    weaponPosition = new Vector2(-_texture.Width, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    break;
                case DirectionEnum.Right:
                    weaponPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    break;
                default:
                    weaponPosition = Vector2.Zero;
                    break;
            }
            _position = weaponPosition;
        }

        public void resetWeaponSpeed(DirectionEnum dir, int time)
        {
            //updates the speed of the weapon
            double xSpeed;
            double ySpeed;
            Vector2 weaponSpeed;
            int xDistance = _drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width / 2;
            int yDistance = _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height / 2;
            xSpeed = (xDistance * 1000) / time;
            ySpeed = (yDistance * 1000) / time;
            switch (dir)
            {
                case DirectionEnum.Up:
                    weaponSpeed = new Vector2(0.0f, (float)ySpeed);
                    break;
                case DirectionEnum.Down:
                    weaponSpeed = new Vector2(0.0f, (float)-ySpeed);
                    break;
                case DirectionEnum.Left:
                    weaponSpeed = new Vector2((float)xSpeed, 0.0f);
                    break;
                case DirectionEnum.Right:
                    weaponSpeed = new Vector2((float)-xSpeed, 0.0f);
                    break;
                default:
                    weaponSpeed = Vector2.Zero;
                    break;
            }
            _speed = weaponSpeed;
        }

        public void weaponMovement(GameTime gameTime)
        {
            //moves the weapon
            _position += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }
    }
}