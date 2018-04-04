using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace repulse
{
    public class PS3Controller : Controller
    {
        public enum PS3StyleEnum
        {
            LeftSide,
            RightSide
        }

        public PS3Controller(PS3StyleEnum style)
        {
            _style = style;
            switch (_style)
            {
                case PS3StyleEnum.LeftSide:
                    _upStick = Buttons.LeftThumbstickUp;
                    _downStick = Buttons.LeftThumbstickDown;
                    _leftStick = Buttons.LeftThumbstickLeft;
                    _rightStick = Buttons.LeftThumbstickRight;
                    _trigger = Buttons.LeftTrigger;
                    break;
                case PS3StyleEnum.RightSide:
                    _upStick = Buttons.RightThumbstickUp;
                    _downStick = Buttons.RightThumbstickDown;
                    _leftStick = Buttons.RightThumbstickLeft;
                    _rightStick = Buttons.RightThumbstickRight;
                    _trigger = Buttons.RightTrigger;
                    break;
            }
            _buttonState.Add(_upStick, false);
            _buttonState.Add(_downStick, false);
            _buttonState.Add(_leftStick, false);
            _buttonState.Add(_rightStick, false);
            _buttonState.Add(_trigger, false);
      
        }

        public override void Update(GameTime gameTime)
        {
            
            GamePadState newState = GamePad.GetState(PlayerIndex.One);
            /*
            // Check the device for Player One
            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                               PlayerIndex.One);

            // If there a controller attached, handle it
            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1

                // You can check explicitly if a gamepad has support for a certain feature
                if (capabilities.HasLeftXThumbStick)
                    {
                        // Check teh direction in X axis of left analog stick
                    if (newState.ThumbSticks.Left.X < -0.5f)
                        if (state.ThumbSticks.Left.X > 0.5f)
                    }
            }
            */
            
            SetState(newState, _upStick, DirectionEnum.Up);
            SetState(newState, _downStick, DirectionEnum.Down);   
            SetState(newState, _leftStick, DirectionEnum.Left);
            SetState(newState, _rightStick, DirectionEnum.Right);
            SetActionState(newState, _trigger, ActionEnum.Button1);

            base.Update(gameTime);
        }

        private void SetState(GamePadState state, Buttons button, DirectionEnum dir)
        {
            bool pressed = IsStickPressed(state, dir);
            if (_buttonState[button] != pressed)
            {
                _buttonState[button] = pressed;
                DoDirection(dir, pressed);
            }
        }

        private void SetActionState(GamePadState state, Buttons button, ActionEnum act)
        {
            bool pressed = IsButtonPressed(state, button);
            if (_buttonState[button] != pressed)
            {
                _buttonState[button] = pressed;
                DoAction(act, pressed);
            }
        }

        private bool IsButtonPressed(GamePadState current, Buttons button)
        {
            return current.IsButtonDown(button);
        }

        private bool IsStickPressed(GamePadState current, DirectionEnum dir)
        {
            switch (_style)
            {
                case PS3StyleEnum.LeftSide:
                    if (current.ThumbSticks.Left.X < -0.5f && dir == DirectionEnum.Left)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Left.X > 0.5f && dir == DirectionEnum.Right)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Left.Y < -0.5f && dir == DirectionEnum.Down)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Left.Y > 0.5f && dir == DirectionEnum.Up)
                    {
                        return true;
                    }
                    break;
                case PS3StyleEnum.RightSide:
                    if (current.ThumbSticks.Right.X < -0.5f && dir == DirectionEnum.Left)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Right.X > 0.5f && dir == DirectionEnum.Right)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Right.Y < -0.5f && dir == DirectionEnum.Down)
                    {
                        return true;
                    }
                    else if (current.ThumbSticks.Right.Y > 0.5f && dir == DirectionEnum.Up)
                    {
                        return true;
                    }
                    break;
            }
            
            return false;
        }
        private Buttons _leftStick;
        private Buttons _rightStick;
        private Buttons _upStick;
        private Buttons _downStick;
        private Buttons _trigger;
        private PS3StyleEnum _style;
        private Dictionary<Buttons, bool> _buttonState = new Dictionary<Buttons, bool>();
    }
}
