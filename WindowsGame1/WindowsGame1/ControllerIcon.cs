using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace repulse
{
    
    public class ControllerIcon : Entity
    {
        private string _assetName;
        private ControllerEnum _controllerType;
        public int player = 0;


        public ControllerIcon(EntityDrawData drawData, string assetName, ControllerEnum controllerType)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _controllerType = controllerType;

            ScaleResize();
            Position();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public void Position()
        {
            Vector2 _pos = new Vector2(3000, 3000);
            switch (player)
            {
                case 1:
                    _pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width * _scale / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height * _scale / 2);
                    break;
                case 2:
                    _pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width * _scale / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height * _scale / 2);
                    break;
               
            }
            if(_drawData.Stage != StageEnum.ControllerTypeSelect)
            {
                _pos = new Vector2(3000, 3000);
            }
            _position = _pos;
        }
        public void ScaleResize()
        {
            switch (_controllerType)
            {
                case ControllerEnum.WASD:
                    _scale = 0.5f;
                    break;
                case ControllerEnum.IJKL:
                    _scale = 0.55f;
                    break;
                case ControllerEnum.Arrow:
                    _scale = 0.7f;
                    break;
                case ControllerEnum.NumPad:
                    _scale = 0.5f;
                    break;
                case ControllerEnum.LeftSide1:
                    _scale = 1.5f;
                    break;
                case ControllerEnum.RightSide1:
                    _scale = 1.5f;
                    break;
                case ControllerEnum.LeftSide2:
                    _scale = 1.5f;
                    break;
                case ControllerEnum.RightSide2:
                    _scale = 1.5f;
                    break;
            }
        }
    }
}
