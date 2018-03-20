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

        public Weapon(EntityDrawData drawData, DirectionEnum direction, string assetName, CharacterEnum cha)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _direction = direction;
            _cha = cha;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            float xSpeed;
            float ySpeed;
            Vector2 weaponSpeed;
            int xDistance = _drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width;
            int yDistance = _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height;

            xSpeed = xDistance / time * 50;
            ySpeed = yDistance / time * 50;
            switch (dir)
            {
                case DirectionEnum.Up:
                    weaponSpeed = new Vector2(0.0f, ySpeed);
                    break;
                case DirectionEnum.Down:
                    weaponSpeed = new Vector2(0.0f, -ySpeed);
                    break;
                case DirectionEnum.Left:
                    weaponSpeed = new Vector2(xSpeed, 0.0f);
                    break;
                case DirectionEnum.Right:
                    weaponSpeed = new Vector2(-xSpeed, 0.0f);
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