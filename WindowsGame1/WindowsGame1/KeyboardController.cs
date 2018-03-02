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
            WSAD,
            IJKL,
            Arrow
        }

        public KeyboardController(KeyboardStyleEnum style)
        {
            switch (style)
            {
                case KeyboardStyleEnum.WSAD:
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

            }
            _pixelGenji = Keys.P;
            _cuteGenji = Keys.C;
            _evilGenji = Keys.E;
            _reinhardt = Keys.R;
            _torbjorn = Keys.T;
            _mercy = Keys.M;

            _keyState.Add(_pixelGenji, false);
            _keyState.Add(_cuteGenji, false);
            _keyState.Add(_evilGenji, false);
            _keyState.Add(_reinhardt, false);
            _keyState.Add(_torbjorn, false);
            _keyState.Add(_mercy, false);
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
           
            SetCharacterState(newState, _pixelGenji, CharacterEnum.PixelGenji);
            SetCharacterState(newState, _cuteGenji, CharacterEnum.CuteGenji);
            SetCharacterState(newState, _evilGenji, CharacterEnum.EvilGenji);
            SetCharacterState(newState, _reinhardt, CharacterEnum.Reinhardt);
            SetCharacterState(newState, _torbjorn, CharacterEnum.Torbjorn);
            SetCharacterState(newState, _mercy, CharacterEnum.Mercy);
            

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

        private void SetCharacterState(KeyboardState state, Keys key, CharacterEnum cha)
        {
            bool pressed = IsKeyPressed(state, key);
            if (_keyState[key] != pressed)
            {
                _keyState[key] = pressed;
                DoCharacter(cha, pressed);
            }
        }

        private bool IsKeyPressed(KeyboardState current, Keys keys)
        {
            return current.IsKeyDown(keys);
        }
        private Keys _pixelGenji;
        private Keys _cuteGenji;
        private Keys _evilGenji;
        private Keys _mercy;
        private Keys _reinhardt;
        private Keys _torbjorn;

        private Keys _upKey;
        private Keys _downKey;
        private Keys _leftKey;
        private Keys _rightKey;
        private Dictionary<Keys, bool> _keyState = new Dictionary<Keys, bool>();
    }
}
