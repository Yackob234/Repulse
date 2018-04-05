using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repulse
{
    public enum ControllerEnum
    {
        WASD,
        IJKL,
        Arrow,
        NumPad,
        LeftSide1,
        RightSide1,
        LeftSide2,
        RightSide2,
        Blank
    }

    public class Controller
    {

        public delegate void ControllerDirectionEvent(Controller controller, DirectionEnum dir, bool pressed);
        public event ControllerDirectionEvent Direction;

        public delegate void ControllerActionEvent(Controller controller, ActionEnum act, bool pressed);
        public event ControllerActionEvent Action;

        public virtual void Update(GameTime gameTime)
        {

        }  

        protected void DoDirection(DirectionEnum dir, bool pressed)
        {
            Direction?.Invoke(this, dir, pressed);
        }    
        protected void DoAction(ActionEnum act, bool pressed)
        {
            Action?.Invoke(this, act, pressed);
        }
    }
}
