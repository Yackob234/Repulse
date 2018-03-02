using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    class Sword : Entity
    {
        private string _assetName;
        private Vector2 _speed;
        public DirectionEnum _direction;

        public Sword(EntityDrawData drawData, DirectionEnum direction, string assetName)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void resetSwordPosition(DirectionEnum dir)
        {
            Vector2 swordPosition;
            switch (dir)
            {
                case DirectionEnum.Up:
                    swordPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, - _texture.Height);
                    break;
                case DirectionEnum.Down:
                    swordPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height);
                    break;
                case DirectionEnum.Left:
                    swordPosition = new Vector2(-_texture.Width, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    break;
                case DirectionEnum.Right:
                    swordPosition = new Vector2(_drawData.GraphicsDevice.Viewport.Width, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    break;
                default:
                    swordPosition = Vector2.Zero;
                    break;
            }
            _position = swordPosition;
        }

        public void resetSwordSpeed(DirectionEnum dir, int time)
        {
            float xSpeed;
            float ySpeed;
            Vector2 swordSpeed;
            int xDistance = _drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width;
            int yDistance = _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height;

            xSpeed = xDistance / time * 50;
            ySpeed = yDistance / time * 50;
            switch (dir)
            {
                case DirectionEnum.Up:
                    swordSpeed = new Vector2(0.0f, ySpeed);
                    break;
                case DirectionEnum.Down:
                    swordSpeed = new Vector2(0.0f, -ySpeed);
                    break;
                case DirectionEnum.Left:
                    swordSpeed = new Vector2(xSpeed, 0.0f);
                    break;
                case DirectionEnum.Right:
                    swordSpeed = new Vector2(-xSpeed, 0.0f);
                    break;
                default:
                    swordSpeed = Vector2.Zero;
                    break;
            }
            _speed = swordSpeed;
        }

        public void swordMovement(GameTime gameTime)
        {
            _position += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }
    }
}