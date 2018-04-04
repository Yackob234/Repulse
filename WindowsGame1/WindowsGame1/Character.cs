using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace repulse
{
    public enum CharacterEnum
    {
        PixelGenji,
        CuteGenji,
        EvilGenji,
        Reinhardt,
        Mercy,
        Torbjorn
    }

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
            _position = Position(_cha);
            
        }

        public override void Update(GameTime gameTime)
        {
            
                _position = Position(_cha);
            
            
            base.Update(gameTime);
        }

        public Vector2 Position(CharacterEnum cha)
        {
            //updates the position of the characters on the choosing screen
            Vector2 pos = Vector2.Zero;
            if (_drawData.Stage == StageEnum.CharacterSelect)
            {
                switch (cha)
                {
                    case CharacterEnum.PixelGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.CuteGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.EvilGenji:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height);
                        break;
                    case CharacterEnum.Mercy:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;
                    case CharacterEnum.Reinhardt:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;
                    case CharacterEnum.Torbjorn:
                        pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 5 * 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2);
                        break;
                }
                
            } else {
                pos = new Vector2(1000, 1000);
            }
            return pos;
        }
    }
}
