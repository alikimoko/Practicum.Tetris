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

        public Button(int screenWidth, Texture2D btn, MenuActions action, int offsetY)
        {
            this.btn = btn;
            this.action = action;

            btnBox = new Rectangle(screenWidth / 2 - btn.Width / 2, offsetY, btn.Width, btn.Height);
        }

        public bool isClicked(InputHelper input)
        {
            return input.MouseLeftButtonPressed() && // mouseclick
                   btnBox.Contains((int)input.MousePosition.X, (int)input.MousePosition.Y); // over the button
        }

        public void Draw(SpriteBatch spriteBatch)
        { spriteBatch.Draw(btn, btnBox, Color.White); }
    }
}
