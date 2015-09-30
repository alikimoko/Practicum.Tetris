using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum2.Tetris
{
    class PlayingField
    {

        byte width, height;

        bool[][] fieldStruc;
        byte[][] fieldCol;

        /// <summary>
        /// Creates the second level arrays.
        /// </summary>
        /// <param name="height">The length of the second level arrays.</param>
        public void finishArrays(byte height)
        {
            for(int i = 0; i < fieldStruc.Length; i++)
            {
                fieldStruc[i] = new bool[height];
                fieldCol[i] = new byte[height];

                for (int j = 0; j < height; j++)
                {
                    fieldStruc[i][j] = false;
                    fieldCol[i][j] = 0; 
                }
            }
        }

        /// <summary>
        /// Create a new playing field.
        /// </summary>
        /// <param name="width">The width of the playing field in cels.</param>
        /// <param name="height">The height of the playing field in cels.</param>
        public PlayingField(byte width, byte height)
        {
            this.width = width <= 4 ? (byte)4 : width;
            this.height = (height < width || height <= 6) ? (byte)6 : height;

            fieldStruc = new bool[this.width][];
            fieldCol = new byte[this.width][];

            finishArrays(this.height);
        }


    }
}
