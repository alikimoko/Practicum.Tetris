using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class GridObject
    {

        protected bool isColor;
        protected byte width, height;
        public byte Width { get { return width; } }
        public byte Height { get { return height; } }

        protected bool[][] fieldStruc;
        protected byte[][] fieldCol;

        protected Texture2D[] blockSprites;

        public GridObject(byte width, byte height, Texture2D[] blockSprites, bool isColor = false)
        {
            this.width = width;
            this.height = height;
            this.isColor = isColor;
            this.blockSprites = blockSprites;
            makeField();
            if (isColor) { makeColorField(); }
        }

        private void makeField()
        {
            fieldStruc = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                fieldStruc[i] = new bool[width];
            }
        }

        private void makeColorField()
        {
            fieldCol = new byte[height][];
            for (int i = 0; i < height; i++)
            {
                fieldCol[i] = new byte[width];
            }
        }

        /// <summary>Get the value of this object its structure at a given x and y</summary>
        public bool checkGridStruct(int y, int x) { return fieldStruc[y][x]; }
        
        /// <summary>Get the value of a given structure at a given x and y</summary>
        public static bool checkGridStruct(bool[][] fieldStruc, int y, int x) { return fieldStruc[y][x]; }

        /// <summary>Get the value of this object its color at a given x and y</summary>
        public byte checkGridCol(int y, int x) { return fieldCol[y][x]; }

        /// <summary>Get the value of a given color structure at a given x and y</summary>
        public static byte checkGridCol(byte[][] fieldCol, int y, int x) { return fieldCol[y][x]; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (checkGridStruct(y, x))
                    {
                        if (isColor) { spriteBatch.Draw(blockSprites[checkGridCol(y, x)], new Vector2(x * 20, y * 20), Color.White); }
                        else { spriteBatch.Draw(blockSprites[1], new Vector2(x * 20, y * 20), Color.White); }
                    }
                    else { spriteBatch.Draw(blockSprites[0], new Vector2(x * 20, y * 20), Color.White); }
                }
            }
        }

    }
}
