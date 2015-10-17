using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    abstract class GridObject
    {
        // base atributes
        protected bool isColor;
        protected byte width, height;
        public byte Width { get { return width; } }
        public byte Height { get { return height; } }

        // field data
        protected bool[][] fieldStruc;
        protected byte[][] fieldCol;

        protected Texture2D blockSprites;

        /// <summary>Make a new grid based object.</summary>
        /// <param name="width">The ammount of cels next to each other.</param>
        /// <param name="height">The ammount of cels below each other.</param>
        /// <param name="blockSprites">The sprite string containing the block sprites.</param>
        /// <param name="isColor">Are colors enabled?</param>
        public GridObject(byte width, byte height, Texture2D blockSprites, bool isColor = false)
        {
            this.width = width;
            this.height = height;
            this.isColor = isColor;
            this.blockSprites = blockSprites;
            makeField();
            if (isColor) { makeColorField(); }
        }

        /// <summary>Create the field structure.</summary>
        private void makeField()
        {
            fieldStruc = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                fieldStruc[i] = new bool[width];
            }
        }

        /// <summary>Create the field color data structure.</summary>
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

        /// <summary>Draw the field.</summary>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
