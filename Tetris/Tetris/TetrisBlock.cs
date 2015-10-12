using Microsoft.Xna.Framework.Graphics;
using System;

namespace Practicum.Tetris
{
    class TetrisBlock : GridObject
    {
        private sbyte offsetX, offsetY;
        public sbyte OffsetX { get { return offsetX; } }
        public sbyte OffsetY { get { return offsetY; } }
        public bool[][] FieldStruct { get { return fieldStruc; } }
        public byte[][] FieldCol { get { return fieldCol; } }

        private byte color;
        public byte Color { get { return color; } }

        Random rand = new Random();

        private static sbyte prevBlock = -1;
        public static TetrisBlock createBlock(Texture2D[] blockSprites, bool isColor = false, sbyte blockKind = -1)
        {
            TetrisBlock block;
            Random random = new Random();

            if (blockKind == -1)
            {
                do
                {
                    blockKind = (sbyte)random.Next(8);
                } while (blockKind == prevBlock);
            }
            prevBlock = blockKind;
            
            switch (blockKind)
                {
                    case 0:
                        //square
                        block = new blockSquare(blockSprites, isColor);
                        break;
                    case 1:
                        //horizontal line
                        block = new blockLineH(blockSprites, isColor);
                        break;
                    case 2:
                        block = new blockZ(blockSprites, isColor);
                        break;
                    case 3:
                        block = new blockReverseZ(blockSprites, isColor);
                        break;
                    case 4:
                        block = new blockFlatL(blockSprites, isColor);
                        break;
                    case 5:
                        block = new blockFlatReverseL(blockSprites, isColor);
                        break;
                    case 6:
                        block = new blockLineV(blockSprites, isColor);
                        break;
                    case 7:
                        block = new blockRoof(blockSprites, isColor);
                        break;
                    default:
                        block = new blockSquare(blockSprites, isColor);
                        break;
                }
            
            return block;
        }
        
        public TetrisBlock(bool x1y1, bool x2y1, bool x3y1, bool x4y1,
                           bool x1y2, bool x2y2, bool x3y2, bool x4y2,
                           bool x1y3, bool x2y3, bool x3y3, bool x4y3,
                           bool x1y4, bool x2y4, bool x3y4, bool x4y4,
                           Texture2D[] blockSprites, bool isColor) :
            base(4, 4, blockSprites, isColor)
        {
            fillStruct(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4);

            if (isColor)
            {
                color = (byte)rand.Next(1, 7);
                colorBlock(color);
            }
            
        }

        /// <summary>Move in the given direction.</summary>
        /// <param name="direction">0 = down, 1 = left, 2 = right, default = down</param>
        public void move(byte direction = 0)
        {
            switch (direction)
            {
                case 0:
                    offsetY += 1;
                    break;
                case 1:
                    offsetX -= 1;
                    break;
                case 2:
                    offsetX += 1;
                    break;
            }
        }

        public virtual void turnClockwise(PlayingField field)
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            { tempBlockStruc[i] = new bool[4]; }

            tempBlockStruc[0][0] = fieldStruc[3][0];    tempBlockStruc[1][0] = fieldStruc[3][1];    tempBlockStruc[2][0] = fieldStruc[3][2];    tempBlockStruc[3][0] = fieldStruc[3][3];
            tempBlockStruc[0][1] = fieldStruc[2][0];    tempBlockStruc[1][1] = fieldStruc[2][1];    tempBlockStruc[2][1] = fieldStruc[2][2];    tempBlockStruc[3][1] = fieldStruc[2][3];
            tempBlockStruc[0][2] = fieldStruc[1][0];    tempBlockStruc[1][2] = fieldStruc[1][1];    tempBlockStruc[2][2] = fieldStruc[1][2];    tempBlockStruc[3][2] = fieldStruc[1][3];
            tempBlockStruc[0][3] = fieldStruc[0][0];    tempBlockStruc[1][3] = fieldStruc[0][1];    tempBlockStruc[2][3] = fieldStruc[0][2];    tempBlockStruc[3][3] = fieldStruc[0][3];

            checkRotation(field, tempBlockStruc);
        }

        public virtual void turnAntiClockwise(PlayingField field)
        {
            bool[][] tempBlockStruc = new bool[4][];
            for (int i = 0; i < 4; i++)
            { tempBlockStruc[i] = new bool[4]; }

            tempBlockStruc[0][0] = fieldStruc[0][3]; tempBlockStruc[1][0] = fieldStruc[0][2]; tempBlockStruc[2][0] = fieldStruc[0][1]; tempBlockStruc[3][0] = fieldStruc[0][0];
            tempBlockStruc[0][1] = fieldStruc[1][3]; tempBlockStruc[1][1] = fieldStruc[1][2]; tempBlockStruc[2][1] = fieldStruc[1][1]; tempBlockStruc[3][1] = fieldStruc[1][0];
            tempBlockStruc[0][2] = fieldStruc[2][3]; tempBlockStruc[1][2] = fieldStruc[2][2]; tempBlockStruc[2][2] = fieldStruc[2][1]; tempBlockStruc[3][2] = fieldStruc[2][0];
            tempBlockStruc[0][3] = fieldStruc[3][3]; tempBlockStruc[1][3] = fieldStruc[3][2]; tempBlockStruc[2][3] = fieldStruc[3][1]; tempBlockStruc[3][3] = fieldStruc[3][0];

            checkRotation(field, tempBlockStruc);
        }


        private void fillStruct(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4)
        {
            fieldStruc[0][0] = x1y1; fieldStruc[0][1] = x2y1; fieldStruc[0][2] = x3y1; fieldStruc[0][3] = x4y1;
            fieldStruc[1][0] = x1y2; fieldStruc[1][1] = x2y2; fieldStruc[1][2] = x3y2; fieldStruc[1][3] = x4y2;
            fieldStruc[2][0] = x1y3; fieldStruc[2][1] = x2y3; fieldStruc[2][2] = x3y3; fieldStruc[2][3] = x4y3;
            fieldStruc[3][0] = x1y4; fieldStruc[3][1] = x2y4; fieldStruc[3][2] = x3y4; fieldStruc[3][3] = x4y4;
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

        private void checkRotation(PlayingField field, bool[][] tempBlockStruc)
        {
            if (field.canBlockMove(tempBlockStruc, offsetX, offsetY))
            { fieldStruc = tempBlockStruc; }
            else
            {
                // check if the turn is posible if the block displaces one x
                if (rand.Next(2) == 0)
                {
                    if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX - 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX -= 1;
                    }
                    else if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX + 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX += 1;
                    }
                }
                else
                {
                    if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX + 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX += 1;
                    }
                    else if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX - 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX -= 1;
                    }
                }
            }
        }
    }
}
