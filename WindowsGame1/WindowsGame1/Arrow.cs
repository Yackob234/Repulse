using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repulse
{
    public class Arrow : ControllerEntity
    {

        public Arrow(EntityDrawData drawData, DirectionEnum direction, string onAssetName, string offAssetName)
            : base(drawData, offAssetName)
        {
            _direction = direction;
            _onAssetName = onAssetName;
            _offAssetName = offAssetName;
            _position = IndiPos(_direction);

        }

        private Vector2 IndiPos(DirectionEnum direction)
        {
            Vector2 position= Vector2.Zero;
            
            switch (direction)
            {
                case DirectionEnum.Up:
                    position = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height / 2 - _texture.Height * 2);
                    break;
                case DirectionEnum.Right:
                    position = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width / 2 + _texture.Width * 2, _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height / 2);
                    break;
                case DirectionEnum.Down:
                    position = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height / 2 + _texture.Height * 2);
                    break;
                case DirectionEnum.Left:
                    position = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 + _texture.Width / 2 - _texture.Width * 2, _drawData.GraphicsDevice.Viewport.Height / 2 + _texture.Height / 2);
                    break;
            }
            
            return position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }  

        public void Toggle(bool pressed)
        {
            
            string asset = pressed ? _onAssetName : _offAssetName;
            _texture = _drawData.LoadTexture(asset);
            
        }

        public int attackDirection = 0;
        public DirectionEnum _direction;
       
        private string _onAssetName;
        private string _offAssetName;
    }
}
