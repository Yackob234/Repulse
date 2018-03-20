using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    public class Character : Entity
    {
        private string _assetName;
        private CharacterEnum _cha;
        private bool StageChangePositionRan;

        public Character(EntityDrawData drawData, string assetName, CharacterEnum cha)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _cha = cha;
            _position = Position(_cha, Stage);
            
        }

        public override void Update(GameTime gameTime)
        {
            if (StageChangePositionRan == false)
            {
                _position = Position(_cha, Stage);
            }
            
            base.Update(gameTime);
        }

        public Vector2 Position(CharacterEnum cha, int _Stage)
        {
            Vector2 pos = Vector2.Zero;
            if (_Stage == 1)
            {
                switch (cha)
                {
                    case CharacterEnum.PixelGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.CuteGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.EvilGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.Mercy:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;
                    case CharacterEnum.Reinhardt:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;
                    case CharacterEnum.Torbjorn:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;

                }
            }
            else if (_Stage == 2)
            {
                pos = new Vector2(1000,1000);
                StageChangePositionRan = true;
            }
            return pos;
        }
    }
}
