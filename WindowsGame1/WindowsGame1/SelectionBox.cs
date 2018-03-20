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
            _position = Position(x, y, Stage);

        }

        public override void Update(GameTime gameTime)
        {
            limitingSelectionMovement();
            _position = Position(x, y, Stage);
            base.Update(gameTime);
        }

        public Vector2 Position(int x, int y, int _Stage)
        {
            Vector2 pos = Vector2.Zero;
            if (_Stage == 1)
            {
                if (x == 0)
                {
                    if (y == 0)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
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
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                    }
                    else if (y == 1)
                    {
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                    }
                }
            }
            else if(_Stage == 2)
            {
                pos = new Vector2(1000, 1000);
                StageChangePositionRan = true;
            }
            return pos;
        }

        public void limitingSelectionMovement()
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
