using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    public class SelectionBox : Entity
    {
        private string _assetName;
        public int x = 0;
        public int y = 0;

        private bool StageChangePositionRan;

        public SelectionBox(EntityDrawData drawData, string assetName)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _position = Position(x, y);

        }

        public override void Update(GameTime gameTime)
        {
            limitingSelectionMovement();
            _position = Position(x, y);
            base.Update(gameTime);
        }

        public Vector2 Position(int x, int y)
        {
            //updates the position of the box
            Vector2 pos = Vector2.Zero;
            if (_drawData.Stage == StageEnum.GamemodeSelect)
            {
                if (x == 0)
                {
                   
                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    
                }
                else if (x == 1)
                {
                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);
                    
                }
            }
            else if (_drawData.Stage == StageEnum.BackgroundSelect)
            {
                if (x == 0)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
                else if (x == 1)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
                else if (x == 2)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
            }
            else if (_drawData.Stage == StageEnum.CharacterSelect)
            {
                if (x == 0)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
                else if (x == 1)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
                else if (x == 2)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
            }
            else if(_drawData.Stage == StageEnum.MainGameplay)
            {
                pos = new Vector2(1000, 1000);
                StageChangePositionRan = true;
            }
            else
            {
                pos = new Vector2(1000, 1000);
            }
            return pos;
        }

        public void limitingSelectionMovement()
        {
            //if you try to go outside of the given options, it will reset it back
            if(_drawData.Stage == StageEnum.GamemodeSelect)
            {
                if (y < 0)
                {
                    y = 0;
                }
                if (y > 0)
                {
                    y = 0;
                }
                if (x < 0)
                {
                    x = 0;
                }
                if (x > 1)
                {
                    x = 1;
                }
            } else if (_drawData.Stage == StageEnum.BackgroundSelect)
            {
                if (y < 0)
                {
                    y = 0;
                }
                if (y > 1)
                {
                    y = 1;
                }
                if (x < 0)
                {
                    x = 0;
                }
                if (x > 2)
                {
                    x = 2;
                }

            } else if (_drawData.Stage == StageEnum.CharacterSelect)
            {
                if (y < 0)
                {
                    y = 0;
                }
                if (y > 1)
                {
                    y = 1;
                }
                if (x < 0)
                {
                    x = 0;
                }
                if (x > 2)
                {
                    x = 2;
                }
            }
                
        }
    }
}
