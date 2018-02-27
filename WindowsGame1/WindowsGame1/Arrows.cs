using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    class Arrows
    {
        private Vector2 baseShift = new Vector2(-3, -3);
        public Vector2 Position= Vector2.Zero;
        public Arrows(Vector2 wanted)
        {
            Position = wanted - baseShift;
            //image = Image;
            //imageHurt = ImageHurt;
        }
    }
}
