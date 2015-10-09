using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum.Tetris
{

    class Block
    {

        bool isColor;
        byte color;
        bool[][] blockStruc = new bool[4][];
        byte[][] blockCol = new byte[4][];
        public sbyte offsetX, offsetY;
        private static int prevBlock;
        /// <summary>
        /// Creates the second level arrays.
        /// </summary>
        public void finishArrays()
        {
            for (int i = 0; i < 4; i++)
            {
                blockStruc[i] = new bool[4];
                blockCol[i] = new byte[4];
            }
        }
        public static Block createBlock(int blockKind = -1, bool isColor=false)
        {
            Block block;

            if (blockKind == -1)
            {
                Random random = new Random();
                
                do
                {
                    blockKind = random.Next(9);
                } while (blockKind == prevBlock);
            }
            prevBlock = blockKind;
            switch (blockKind)
                {
                    case 0:
                        //square
                        block = new blockSquare(isColor);
                        break;
                    case 1:
                        //horizontal line
                        block = new blockLineH(isColor);
                        break;
                    case 2:
                        //vertical line
                        block = new blockLineV(isColor);
                        break;
                    case 3:
                        block = new blockZ(isColor);
                        break;
                    case 4:
                        block = new blockReverseZ(isColor);
                        break;
                    case 5:
                        block = new blockFlatL(isColor);
                        break;
                    case 6:
                        block = new blockFlatReverseL(isColor);
                        break;
                    case 7:
                        block = new blockC(isColor);
                        break;
                    case 8:
                        block = new blockRoof(isColor);
                        break;
                    
                    
                    //alle andere blokken hier ook toevoegen... (case 3-case 9)
                    default:
                        //default is square dus als er iets mis gaat wordt t een vierkant
                        block = new Block(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false, isColor);
                        break;
                    
                }
            
            
            return block;
        }
        /// <summary>
        /// Creates a new tetris block structure.
        /// </summary>
        /// <param name="x1y1">Is there a block at the internal x = 1, y = 1 coördinate?</param>
        /// <param name="x2y1">Is there a block at the internal x = 2, y = 1 coördinate?</param>
        /// <param name="x3y1">Is there a block at the internal x = 3, y = 1 coördinate?</param>
        /// <param name="x4y1">Is there a block at the internal x = 4, y = 1 coördinate?</param>
        /// <param name="x1y2">Is there a block at the internal x = 1, y = 2 coördinate?</param>
        /// <param name="x2y2">Is there a block at the internal x = 2, y = 2 coördinate?</param>
        /// <param name="x3y2">Is there a block at the internal x = 3, y = 2 coördinate?</param>
        /// <param name="x4y2">Is there a block at the internal x = 4, y = 2 coördinate?</param>
        /// <param name="x1y3">Is there a block at the internal x = 1, y = 3 coördinate?</param>
        /// <param name="x2y3">Is there a block at the internal x = 2, y = 3 coördinate?</param>
        /// <param name="x3y3">Is there a block at the internal x = 3, y = 3 coördinate?</param>
        /// <param name="x4y3">Is there a block at the internal x = 4, y = 3 coördinate?</param>
        /// <param name="x1y4">Is there a block at the internal x = 1, y = 4 coördinate?</param>
        /// <param name="x2y4">Is there a block at the internal x = 2, y = 4 coördinate?</param>
        /// <param name="x3y4">Is there a block at the internal x = 3, y = 4 coördinate?</param>
        /// <param name="x4y4">Is there a block at the internal x = 4, y = 4 coördinate?</param>
        public Block(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4)
            : this(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4, false)
        { }

        /// <summary>
        /// Creates a new tetris block structure wich may allow color differences.
        /// </summary>
        /// <param name="x1y1">Is there a block at the internal x = 1, y = 1 coördinate?</param>
        /// <param name="x2y1">Is there a block at the internal x = 2, y = 1 coördinate?</param>
        /// <param name="x3y1">Is there a block at the internal x = 3, y = 1 coördinate?</param>
        /// <param name="x4y1">Is there a block at the internal x = 4, y = 1 coördinate?</param>
        /// <param name="x1y2">Is there a block at the internal x = 1, y = 2 coördinate?</param>
        /// <param name="x2y2">Is there a block at the internal x = 2, y = 2 coördinate?</param>
        /// <param name="x3y2">Is there a block at the internal x = 3, y = 2 coördinate?</param>
        /// <param name="x4y2">Is there a block at the internal x = 4, y = 2 coördinate?</param>
        /// <param name="x1y3">Is there a block at the internal x = 1, y = 3 coördinate?</param>
        /// <param name="x2y3">Is there a block at the internal x = 2, y = 3 coördinate?</param>
        /// <param name="x3y3">Is there a block at the internal x = 3, y = 3 coördinate?</param>
        /// <param name="x4y3">Is there a block at the internal x = 4, y = 3 coördinate?</param>
        /// <param name="x1y4">Is there a block at the internal x = 1, y = 4 coördinate?</param>
        /// <param name="x2y4">Is there a block at the internal x = 2, y = 4 coördinate?</param>
        /// <param name="x3y4">Is there a block at the internal x = 3, y = 4 coördinate?</param>
        /// <param name="x4y4">Is there a block at the internal x = 4, y = 4 coördinate?</param>
        /// <param name="isColor">Does the structure allow colors?</param>
        public Block(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4, bool isColor)
        {
            finishArrays();

            blockStruc[0][0] = x1y1; blockStruc[1][0] = x2y1; blockStruc[2][0] = x3y1; blockStruc[3][0] = x4y1;
            blockStruc[0][1] = x1y2; blockStruc[1][1] = x2y2; blockStruc[2][1] = x3y2; blockStruc[3][1] = x4y2;
            blockStruc[0][2] = x1y3; blockStruc[1][2] = x2y3; blockStruc[2][2] = x3y3; blockStruc[3][2] = x4y3;
            blockStruc[0][3] = x1y4; blockStruc[1][3] = x2y4; blockStruc[2][3] = x3y4; blockStruc[3][3] = x4y4;

            this.isColor = isColor;
            offsetX = 0; offsetY = 0;
        }

        /// <summary>
        /// Creates a new tetris block structure wich may allow color differences.
        /// </summary>
        /// <param name="x1y1">Is there a block at the internal x = 1, y = 1 coördinate?</param>
        /// <param name="x2y1">Is there a block at the internal x = 2, y = 1 coördinate?</param>
        /// <param name="x3y1">Is there a block at the internal x = 3, y = 1 coördinate?</param>
        /// <param name="x4y1">Is there a block at the internal x = 4, y = 1 coördinate?</param>
        /// <param name="x1y2">Is there a block at the internal x = 1, y = 2 coördinate?</param>
        /// <param name="x2y2">Is there a block at the internal x = 2, y = 2 coördinate?</param>
        /// <param name="x3y2">Is there a block at the internal x = 3, y = 2 coördinate?</param>
        /// <param name="x4y2">Is there a block at the internal x = 4, y = 2 coördinate?</param>
        /// <param name="x1y3">Is there a block at the internal x = 1, y = 3 coördinate?</param>
        /// <param name="x2y3">Is there a block at the internal x = 2, y = 3 coördinate?</param>
        /// <param name="x3y3">Is there a block at the internal x = 3, y = 3 coördinate?</param>
        /// <param name="x4y3">Is there a block at the internal x = 4, y = 3 coördinate?</param>
        /// <param name="x1y4">Is there a block at the internal x = 1, y = 4 coördinate?</param>
        /// <param name="x2y4">Is there a block at the internal x = 2, y = 4 coördinate?</param>
        /// <param name="x3y4">Is there a block at the internal x = 3, y = 4 coördinate?</param>
        /// <param name="x4y4">Is there a block at the internal x = 4, y = 4 coördinate?</param>
        /// <param name="color">The color of the block.</param>
        public Block(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4, byte color)
            : this(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4, true)
        {

            this.color = color;

            for(int i = 0; i < blockStruc.Length; i++)
            {
                for(int j = 0; j<blockStruc[i].Length; j++)
                {
                    if (blockStruc[i][j]) { blockCol[i][j] = color; }
                    else { blockCol[i][j] = 0; }
                }
            }
        }

        public void moveDown()
        {
            offsetY += 1;
        }

        public bool checkGrid(int y, int x)
        {

            return blockStruc[y][x];
        }

        public byte checkGrid2(int y, int x)
        {

            return blockCol[y][x];
        }

        public virtual void turnLeft()
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            {
                tempBlockStruc[i] = new bool[4];
            }

            tempBlockStruc[0][0] = blockStruc[3][0];    tempBlockStruc[1][0] = blockStruc[3][1];    tempBlockStruc[2][0] = blockStruc[3][2];    tempBlockStruc[3][0] = blockStruc[3][3];
            tempBlockStruc[0][1] = blockStruc[2][0];    tempBlockStruc[1][1] = blockStruc[2][1];    tempBlockStruc[2][1] = blockStruc[2][2];    tempBlockStruc[3][1] = blockStruc[2][3];
            tempBlockStruc[0][2] = blockStruc[1][0];    tempBlockStruc[1][2] = blockStruc[1][1];    tempBlockStruc[2][2] = blockStruc[1][2];    tempBlockStruc[3][2] = blockStruc[1][3];
            tempBlockStruc[0][3] = blockStruc[0][0];    tempBlockStruc[1][3] = blockStruc[0][1];    tempBlockStruc[2][3] = blockStruc[0][2];    tempBlockStruc[3][3] = blockStruc[0][3];

            for (int i = 0; i < blockStruc.Length; i++)
            {
                for (int j = 0; j < blockStruc[i].Length; j++)
                {
                    blockStruc[i][j] = tempBlockStruc[i][j];
                }
            }
        }

        public virtual void turnRight()
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            {
                tempBlockStruc[i] = new bool[4];
            }

            tempBlockStruc[0][0] = blockStruc[0][3]; tempBlockStruc[1][0] = blockStruc[0][2]; tempBlockStruc[2][0] = blockStruc[0][1]; tempBlockStruc[3][0] = blockStruc[0][0];
            tempBlockStruc[0][1] = blockStruc[1][3]; tempBlockStruc[1][1] = blockStruc[1][2]; tempBlockStruc[2][1] = blockStruc[1][1]; tempBlockStruc[3][1] = blockStruc[1][0];
            tempBlockStruc[0][2] = blockStruc[2][3]; tempBlockStruc[1][2] = blockStruc[2][2]; tempBlockStruc[2][2] = blockStruc[2][1]; tempBlockStruc[3][2] = blockStruc[2][0];
            tempBlockStruc[0][3] = blockStruc[3][3]; tempBlockStruc[1][3] = blockStruc[3][2]; tempBlockStruc[2][3] = blockStruc[3][1]; tempBlockStruc[3][3] = blockStruc[3][0];

            for (int i = 0; i < blockStruc.Length; i++)
            {
                for (int j = 0; j < blockStruc[i].Length; j++)
                {
                    blockStruc[i][j] = tempBlockStruc[i][j];
                }
            }
        }
    }
}
