using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class InputHelper
{
    /* MouseState currentMouseState, previousMouseState;*/
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
        // check if keys are pressed and update the timeSinceLastKeyPress variable
        Keys[] prevKeysDown = previousKeyboardState.GetPressedKeys();
        Keys[] currKeysDown = currentKeyboardState.GetPressedKeys();
        if (currKeysDown.Length != 0 && (prevKeysDown.Length == 0 || timeSinceLastKeyPress > keyPressInterval))
            timeSinceLastKeyPress = 0;
        else
            timeSinceLastKeyPress += gameTime.ElapsedGameTime.TotalMilliseconds;

        // update the mouse and keyboard states
        //previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        //currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    /*
     * returns the current mouse position
     
    public Vector2 MousePosition
    {
        get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
    }*/

    /*
     * indicates whether the left mouse button is pressed
     
    public bool MouseLeftButtonPressed()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }*/

    /*
     * indicates whether the player has pressed the key k in the current update, a key press is detected
     * if either the key wasn't pressed in the previous state, or enough time has passed since the last time the key press
     * was detected
     */
    public bool KeyPressed(Keys k, bool detecthold = true)
    {
        return currentKeyboardState.IsKeyDown(k) && (previousKeyboardState.IsKeyUp(k) || (timeSinceLastKeyPress > keyPressInterval && detecthold));
    }

    /*
     * indicates whether key k is currently down
     */
    public bool IsKeyDown(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k);
    }
}