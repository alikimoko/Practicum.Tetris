using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class Button
    {
        Texture2D btn;
        MenuActions action;
        public MenuActions Action { get { return action; } }
        Rectangle btnBox;

        /// <summary>Make a new button.</summary>
        /// <param name="screenWidth">The width of the game window.</param>
        /// <param name="btn">The button texture.</param>
        /// <param name="action">The action the button triggers.</param>
        /// <param name="offsetY">The y coördinate of the button.</param>
        public Button(int screenWidth, Texture2D btn, MenuActions action, int offsetY)
        {
            this.btn = btn;
            this.action = action;

            btnBox = new Rectangle(screenWidth / 2 - btn.Width / 2, offsetY, btn.Width, btn.Height);
        }

        /// <summary>Is the button being clicked?</summary>
        public bool isClicked(InputHelper input)
        {
            return input.MouseLeftButtonPressed() && // mouseclick
                   btnBox.Contains((int)input.MousePosition.X, (int)input.MousePosition.Y); // over the button
        }

        /// <summary>Is the mouse over the button?</summary>
        public bool isHovered(InputHelper input)
        { return btnBox.Contains((int)input.MousePosition.X, (int)input.MousePosition.Y); }

        /// <summary>Draws the button.</summary>
        public void Draw(SpriteBatch spriteBatch)
        { spriteBatch.Draw(btn, btnBox, Color.White); }
    }
}
