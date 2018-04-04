using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace repulse
{
    public class Gamemode : Entity
    {
        private string _assetName;
        private int _gamemode;
        public int x = 0;
        public int y = 0;

        public Gamemode(EntityDrawData drawData, string assetName, int gamemode)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            _gamemode = gamemode;
            _position = Position(_gamemode);
        }

        public override void Update(GameTime gameTime)
        {
            _position = Position(_gamemode);
            base.Update(gameTime);
        }

        public Vector2 Position(int gamemode)
        {
            Vector2 pos = Vector2.Zero;
            if (_drawData.Stage == StageEnum.GamemodeSelect)
            {
                if (gamemode == 1)
                {

                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);

                }
                else if (gamemode == 2)
                {
                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 4 * 3 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);

                }
            }
            else if (_drawData.Stage == StageEnum.PlayerInformationScreen)
            {
                if (_drawData.gameMode == 1 && _gamemode == 1)
                {
                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);

                }
                else if (_drawData.gameMode == 2 && _gamemode == 2)
                {
                    pos = new Vector2(_drawData.GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, _drawData.GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2);

                }
                else
                {
                    pos = new Vector2(1000, 1000);
                }
            }
            else
            {
                pos = new Vector2(1000, 1000);
            }

           
            return pos;
        }
    }
}
