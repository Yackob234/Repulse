using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    public class Background : Entity
    {
        private string _assetName;
        private int _number;
        public bool chosen;
        public int x = 0;
        public int y = 0;

        public Background(EntityDrawData drawData, string assetName, int number, bool chosen1)
            : base(drawData, assetName)
        {
            _number = number;
            _assetName = assetName;
            chosen = chosen1;
            _position = Position();
            
        }

        public override void Update(GameTime gameTime)
        {

            checkChosen();
            base.Update(gameTime);
        }

        public Vector2 Position()
        {
            Vector2 pos = Vector2.Zero;
            float factor;
            if(_drawData.Stage == StageEnum.BackgroundSelect)
            {
                switch (_number)
                {
                    case 1:
                        factor = 0.15f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height * factor);
                        _scale = 0.15f;
                        break;
                    case 2:
                        factor = 0.16f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height * factor);
                        _scale = factor;
                        break;
                    case 3:
                        factor = 0.23f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height * factor);
                        _scale = factor;
                        break;
                    case 4:
                        factor = 0.15f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        _scale = factor;
                        break;
                    case 5:
                        factor = 0.3f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        _scale = factor;
                        break;
                    case 6:
                        factor = 0.23f;
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width * factor / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        _scale = factor;
                        break;
                }
            }
            else if (_drawData.Stage == StageEnum.MainGameplay)
            {
                if (chosen)
                {
                    pos = Vector2.Zero;
                } else
                {
                    pos = new Vector2(1000.0f, 1000.0f);
                }
            }
            else if (_drawData.Stage == StageEnum.EndScreen)
            {
                if (chosen)
                {
                    pos = Vector2.Zero;
                }
                else
                {
                    pos = new Vector2(1000.0f, 1000.0f);
                }
            }
            else
            {
                pos = new Vector2(1000.0f, 1000.0f);
            }
            return pos;
        }

        public void checkChosen()
        {
            if(_drawData.Stage == StageEnum.BackgroundSelect)
            {
                
                _position = Position();
                chosen = false;
            }

            else if (chosen)
            {
                switch (_number)
                {
                    case 1:
                        _scale = 0.56f;
                        break;
                    case 2:
                        _scale = 0.66f;
                        //needs adjustin
                        break;
                    case 3:
                        _scale = 0.79f;
                        break;
                    case 4:
                        _scale = 0.53f;
                        break;
                    case 5:
                        _scale = 0.98f;
                        break;
                    case 6:
                        _scale = 0.84f;
                        break;
                }
                _position = Position();
            }
            else
            {
                _position = Position();
            }

        }
        
    }

}
