using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Practicum.Tetris
{
    class InputHelper
    {
        MouseState currentMouseState, previousMouseState;
        KeyboardState currentKeyboardState, previousKeyboardState; // current and previous keyboard states

        double timeSinceLastKeyPress, // time passed since the last key press
               keyPressInterval; // time interval to read separate keypresses when holding a key

        /// <summary>Initialize input helper.</summary>
        /// <param name="keyReCheckTime">The time after wich a key hold can trigger another action in milliseconds.</param>
        public InputHelper(double keyReCheckTime = 100)
        {
            keyPressInterval = keyReCheckTime;
            timeSinceLastKeyPress = 0;
        }

        /// <summary>Update the input helper.</summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            // update the mouse and keyboard states
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();

            // check if keys are pressed and update the timeSinceLastKeyPress variable
            Keys[] prevKeysDown = previousKeyboardState.GetPressedKeys();
            Keys[] currKeysDown = currentKeyboardState.GetPressedKeys();
            if (currKeysDown.Length != 0 && // key is now pressed
                    (prevKeysDown.Length == 0 || // no keypress last tick
                    timeSinceLastKeyPress > keyPressInterval)) // waited long enough
                timeSinceLastKeyPress = 0;
            else
                timeSinceLastKeyPress += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>The current mouse position.</summary>
        public Vector2 MousePosition
        { get { return new Vector2(currentMouseState.X, currentMouseState.Y); } }

        /// <summary>Has there been a mouseclick?</summary>
        public bool MouseLeftButtonPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && // mouse now pressed
                   previousMouseState.LeftButton == ButtonState.Released; // mouse not pressed in the last tick
        }

        /// <summary>Checks if a certain key is pressed.</summary>
        /// <param name="k">The key to check.</param>
        /// <param name="detecthold">Should the check be made once every few milliseconds?</param>
        public bool KeyPressed(Keys k, bool detecthold = true)
        {
            return currentKeyboardState.IsKeyDown(k) && // key is now down
                       (previousKeyboardState.IsKeyUp(k) || // key was up in the last tick
                       (timeSinceLastKeyPress > keyPressInterval && detecthold)); // was helt for long enough
        }

        /// <summary>Checks if a certain key is down.</summary>
        /// <param name="k">The key to check.</param>
        public bool IsKeyDown(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k);
        }
    }
}