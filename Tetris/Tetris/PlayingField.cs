using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum.Tetris
{
    class PlayingField
    {

        public bool isColor;
        byte width, height;

        bool[][] fieldStruc;
        byte[][] fieldCol;

        /// <summary>
        /// Creates the second level arrays.
        /// </summary>
        public void finishArrays()
        {
            for(int i = 0; i < fieldStruc.Length; i++)
            {
                fieldStruc[i] = new bool[width];
                fieldCol[i] = new byte[width];

                for (int j = 0; j < width; j++)
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

            fieldStruc = new bool[this.height][];
            fieldCol = new byte[this.height][];

            finishArrays();
        }

        /// <summary>
        /// Moves the rows after the row has been cleared.
        /// </summary>
        /// <param name="rowNum">The row that has been cleared.</param>
        public void moveRows(byte rowNum)
        {
            moveRows(rowNum, 1);
        }

        /// <summary>
        /// Moves the rows after a number of rows has been cleared.
        /// </summary>
        /// <param name="lowestRow">The row lowest in the field that has been cleared.</param>
        /// <param name="numOfRows">The number of rows that has been cleared.</param>
        public void moveRows(byte lowestRow, byte numOfRows)
        {
            //TODO: wat als 1 rij wel,1 rij niet dan weer wel?
            int i;
            
            // move the rows to the new lowest position
            for(i = lowestRow-numOfRows; i>=numOfRows; i--)
            {
                fieldStruc[i + numOfRows] = fieldStruc[i];
                fieldCol[i + numOfRows] = fieldCol[i];
            }

            // make empty rows at the top
            while (i >= 0)
            {
                fieldStruc[i] = new bool[width];
                fieldCol[i] = new byte[width];

                for (int j = 0; j < width; j++)
                {
                    fieldStruc[i][j] = false;
                    fieldCol[i][j] = 0;
                }
                i--;
            }
        }

        public bool checkGrid(int y, int x){
        
        return fieldStruc[y][x];
        }

        public byte checkGrid2(int y, int x)
        {

            return fieldCol[y][x];
        }

    }
}
