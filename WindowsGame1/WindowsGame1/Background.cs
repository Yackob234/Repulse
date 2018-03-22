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
        public int x = 0;
        public int y = 0;

        public Background(EntityDrawData drawData, string assetName)
            : base(drawData, assetName)
        {
            _assetName = assetName;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
