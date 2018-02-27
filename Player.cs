using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace repulse
{
    class Player
    {
        public int health;
        public Texture2D image;
        public Texture2D imageHurt;
        public bool attacker;
        public Player(int Health, bool Attacker)
        {
            health = Health;
            attacker = Attacker;
            //image = Image;
            //imageHurt = ImageHurt;
        }
    }
}
