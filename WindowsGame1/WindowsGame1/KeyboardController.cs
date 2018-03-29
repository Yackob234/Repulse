using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace repulse
{
    public class KeyboardController : Controller
    {
        public enum KeyboardStyleEnum
        {
            WASD,
            IJKL,
            Arrow,
            NumPad
        }

        public KeyboardController(KeyboardStyleEnum style)
        {
            switch (style)
            {
                case KeyboardStyleEnum.WASD:
                    _upKey = Keys.W;
                    _downKey = Keys.S;
                    _leftKey = Keys.A;
                    _rightKey = Keys.D;
                    break;
                case KeyboardStyleEnum.IJKL:
                    _upKey = Keys.I;
                    _downKey = Keys.K;
                    _leftKey = Keys.J;
                    _rightKey = Keys.L;
                    break;
                case KeyboardStyleEnum.Arrow:
                    _upKey = Keys.Up;
                    _downKey = Keys.Down;
                    _leftKey = Keys.Left;
                    _rightKey = Keys.Right;
                    break;
                case KeyboardStyleEnum.NumPad:
                    _upKey = Keys.NumPad8;
                    _downKey = Keys.NumPad5;
                    _leftKey = Keys.NumPad4;
                    _rightKey = Keys.NumPad6;
                    break;

            }

            _keyState.Add(Keys.Enter, false);
            _keyState.Add(_upKey, false);
            _keyState.Add(_downKey, false);
            _keyState.Add(_leftKey, false);
            _keyState.Add(_rightKey, false);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            SetState(newState, _upKey, DirectionEnum.Up);
            SetState(newState, _downKey, DirectionEnum.Down);
            SetState(newState, _leftKey, DirectionEnum.Left);
            SetState(newState, _rightKey, DirectionEnum.Right);

            SetActionState(newState, Keys.Enter, ActionEnum.Button1);

            base.Update(gameTime);
        }

        private void SetState(KeyboardState state, Keys key, DirectionEnum dir)
        {
            bool pressed = IsKeyPressed(state, key);
            if (_keyState[key] != pressed)
            {
                _keyState[key] = pressed;
                DoDirection(dir, pressed);
            }
        }

        private void SetActionState(KeyboardState state, Keys key, ActionEnum act)
        {
            bool pressed = IsKeyPressed(state, key);
            if (_keyState[key] != pressed)
            {
                _keyState[key] = pressed;
                DoAction(act, pressed);
            }
        }

        private bool IsKeyPressed(KeyboardState current, Keys keys)
        {
            return current.IsKeyDown(keys);
        }

        private Keys _upKey;
        private Keys _downKey;
        private Keys _leftKey;
        private Keys _rightKey;
        private Dictionary<Keys, bool> _keyState = new Dictionary<Keys, bool>();
    }
}
