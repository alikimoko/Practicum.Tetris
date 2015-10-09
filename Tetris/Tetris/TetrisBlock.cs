using System;

namespace Practicum.Tetris
{
    class TetrisBlock : GridObject
    {
        private sbyte offsetX, offsetY;
        public sbyte OffsetX { get { return offsetX; } }
        public sbyte OffsetY { get { return offsetY; } }

        private byte color;
        public byte Color { get { return color; } }

        private static int prevBlock;
        public static TetrisBlock createBlock(int blockKind = -1, byte color = 0)
        {
            TetrisBlock block;

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
                        block = color == 0 ? new blockSquare() : new blockSquare(color);
                        break;
                    case 1:
                        //horizontal line
                        block = color == 0 ? new blockLineH() : new blockLineH(color);
                        break;
                    case 2:
                        //vertical line
                        block = color == 0 ? new blockLineV() : new blockLineV(color);
                        break;
                    case 3:
                        block = color == 0 ? new blockZ() : new blockZ(color);
                        break;
                    case 4:
                        block = color == 0 ? new blockReverseZ() : new blockReverseZ(color);
                        break;
                    case 5:
                        block = color == 0 ? new blockFlatL() : new blockFlatL(color);
                        break;
                    case 6:
                        block = color == 0 ? new blockFlatReverseL() : new blockFlatReverseL(color);
                        break;
                    case 7:
                        block = color == 0 ? new blockC() : new blockC(color);
                        break;
                    case 8:
                        block = color == 0 ? new blockRoof() : new blockRoof(color);
                        break;
                    
                    default:
                        //default is square dus als er iets mis gaat wordt t een vierkant
                        block = color == 0 ? new TetrisBlock(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false)
                                           : new TetrisBlock(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false, color);
                        break;
                    
                }
            
            
            return block;
        }

        public TetrisBlock(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4) : base(4, 4)
        {
            fillStruct(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4);
        }

        public TetrisBlock(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4, byte color) : base(4, 4, true)
        {
            this.color = color;

            fillStruct(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4);
            colorBlock(color);
        }

        public void moveDown()
        {
            offsetY += 1;
        }

        public bool checkGrid(int y, int x)
        {

            return fieldStruc[y][x];
        }

        public byte checkGrid2(int y, int x)
        {

            return fieldCol[y][x];
        }

        public virtual void turnLeft()
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            {
                tempBlockStruc[i] = new bool[4];
            }

            tempBlockStruc[0][0] = fieldStruc[3][0];    tempBlockStruc[1][0] = fieldStruc[3][1];    tempBlockStruc[2][0] = fieldStruc[3][2];    tempBlockStruc[3][0] = fieldStruc[3][3];
            tempBlockStruc[0][1] = fieldStruc[2][0];    tempBlockStruc[1][1] = fieldStruc[2][1];    tempBlockStruc[2][1] = fieldStruc[2][2];    tempBlockStruc[3][1] = fieldStruc[2][3];
            tempBlockStruc[0][2] = fieldStruc[1][0];    tempBlockStruc[1][2] = fieldStruc[1][1];    tempBlockStruc[2][2] = fieldStruc[1][2];    tempBlockStruc[3][2] = fieldStruc[1][3];
            tempBlockStruc[0][3] = fieldStruc[0][0];    tempBlockStruc[1][3] = fieldStruc[0][1];    tempBlockStruc[2][3] = fieldStruc[0][2];    tempBlockStruc[3][3] = fieldStruc[0][3];

            fieldStruc = tempBlockStruc;
        }

        public virtual void turnRight()
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            {
                tempBlockStruc[i] = new bool[4];
            }

            tempBlockStruc[0][0] = fieldStruc[0][3]; tempBlockStruc[1][0] = fieldStruc[0][2]; tempBlockStruc[2][0] = fieldStruc[0][1]; tempBlockStruc[3][0] = fieldStruc[0][0];
            tempBlockStruc[0][1] = fieldStruc[1][3]; tempBlockStruc[1][1] = fieldStruc[1][2]; tempBlockStruc[2][1] = fieldStruc[1][1]; tempBlockStruc[3][1] = fieldStruc[1][0];
            tempBlockStruc[0][2] = fieldStruc[2][3]; tempBlockStruc[1][2] = fieldStruc[2][2]; tempBlockStruc[2][2] = fieldStruc[2][1]; tempBlockStruc[3][2] = fieldStruc[2][0];
            tempBlockStruc[0][3] = fieldStruc[3][3]; tempBlockStruc[1][3] = fieldStruc[3][2]; tempBlockStruc[2][3] = fieldStruc[3][1]; tempBlockStruc[3][3] = fieldStruc[3][0];

            fieldStruc = tempBlockStruc;
        }


        private void fillStruct(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4)
        {
            fieldStruc[0][0] = x1y1; fieldStruc[1][0] = x2y1; fieldStruc[2][0] = x3y1; fieldStruc[3][0] = x4y1;
            fieldStruc[0][1] = x1y2; fieldStruc[1][1] = x2y2; fieldStruc[2][1] = x3y2; fieldStruc[3][1] = x4y2;
            fieldStruc[0][2] = x1y3; fieldStruc[1][2] = x2y3; fieldStruc[2][2] = x3y3; fieldStruc[3][2] = x4y3;
            fieldStruc[0][3] = x1y4; fieldStruc[1][3] = x2y4; fieldStruc[2][3] = x3y4; fieldStruc[3][3] = x4y4;
        }

        private void colorBlock(byte color)
        {
            for (int i = 0; i < fieldStruc.Length; i++)
            {
                for (int j = 0; j < fieldStruc[i].Length; j++)
                {
                    if (fieldStruc[i][j]) { fieldCol[i][j] = color; }
                    else { fieldCol[i][j] = 0; }
                }
            }
        }
    }
}
