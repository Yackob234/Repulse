using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repulse
{
    public class Controller
    {

        public delegate void ControllerDirectionEvent(Controller controller, DirectionEnum dir, bool pressed);
        public event ControllerDirectionEvent Direction;

        public delegate void ControllerCharacterEvent(Controller controller, CharacterEnum cha, bool pressed);
        public event ControllerCharacterEvent Character;

        public virtual void Update(GameTime gameTime)
        {

        }

       

        protected void DoDirection(DirectionEnum dir, bool pressed)
        {
            Direction?.Invoke(this, dir, pressed);
        }    
        protected void DoCharacter(CharacterEnum cha, bool pressed)
        {
            Character?.Invoke(this, cha, pressed);
        }
    }
}
