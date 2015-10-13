﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Practicum.Tetris
{
    enum BlockType : byte { Square, LineH, LineV, Roof, Z, ReverseZ, FlatL, FlatReverseL }

    class TetrisBlock : GridObject
    {
        private bool isNext;
        private sbyte offsetX, offsetY;
        public sbyte OffsetX { get { return offsetX; } }
        public sbyte OffsetY { get { return offsetY; } }
        public bool[][] FieldStruct { get { return fieldStruc; } }
        public byte[][] FieldCol { get { return fieldCol; } }

        private PlayingField field;
        
        int moveTimer = 0, moveTimerLim;

        private byte color;
        public byte blockColor { get { return color; } }

        private BlockType blockType;
        public BlockType BlockType { get { return blockType; } }

        Random rand = new Random();

        private static sbyte prevBlock = -1;
        public static TetrisBlock createBlock(Texture2D[] blockSprites, PlayingField field, int moveTimerLim, bool isColor = false, sbyte blockKind = -1)
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
                        // square
                        block = new blockSquare(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 1:
                        // horizontal line
                        block = new blockLineH(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 2:
                        // Z shape
                        block = new blockZ(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 3:
                        // reverse Z shape
                        block = new blockReverseZ(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 4:
                        // L shape
                        block = new blockFlatL(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 5:
                        // reverse L shape
                        block = new blockFlatReverseL(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 6:
                        // vertical line
                        block = new blockLineV(blockSprites, field, moveTimerLim, isColor);
                        break;
                    case 7:
                        // T shape
                        block = new blockRoof(blockSprites, field, moveTimerLim, isColor);
                        break;
                    default:
                        // square
                        block = new blockSquare(blockSprites, field, moveTimerLim, isColor);
                        break;
                }
            return block;
        }
        
        public TetrisBlock(bool x1y1, bool x2y1, bool x3y1, bool x4y1,
                           bool x1y2, bool x2y2, bool x3y2, bool x4y2,
                           bool x1y3, bool x2y3, bool x3y3, bool x4y3,
                           bool x1y4, bool x2y4, bool x3y4, bool x4y4,
                           Texture2D[] blockSprites, PlayingField field, int moveTimerLim, BlockType blockType, bool isColor) :
            base(4, 4, blockSprites, isColor)
        {
            this.field = field;
            this.moveTimerLim = moveTimerLim;
            this.blockType = blockType;
            isNext = true;

            fillStruct(x1y1, x2y1, x3y1, x4y1, x1y2, x2y2, x3y2, x4y2, x1y3, x2y3, x3y3, x4y3, x1y4, x2y4, x3y4, x4y4);
            
            if (isColor)
            {
                color = (byte)rand.Next(1, 7);
                colorBlock();
            }
        }

        /// <summary>Move in the given direction.</summary>
        /// <param name="direction">0 = down, 1 = left, 2 = right, default = down</param>
        public void move(Movement direction = Movement.Down)
        {
            switch (direction)
            {
                case Movement.Down:
                    offsetY += 1;
                    break;
                case Movement.Left:
                    offsetX -= 1;
                    break;
                case Movement.Right:
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
            if (isColor) { colorBlock(); }
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
            if (isColor) { colorBlock(); }
        }

        /// <summary>Checks if the block can be placed. Places when posible, if not posible it's game over.</summary>
        public bool placeBlock()
        {
            isNext = false;

            switch (blockType)
            {
                case BlockType.Roof:
                    return placeBlock(1, 1, 3);

                case BlockType.LineH:
                    return placeBlock(1, 0, 3);

                case BlockType.LineV:
                    return placeBlock(0, 1, 1);

                case BlockType.FlatL:
                    return placeBlock(1, 0, 2);

                case BlockType.FlatReverseL:
                case BlockType.Z:
                case BlockType.ReverseZ:
                    return placeBlock(1, 1, 3);

                case BlockType.Square:
                default:
                    // square
                    return placeBlock(1, 1, 2);

            }
        }

        private bool placeBlock(byte topmost, byte leftmost, byte rightmost)
        {
            offsetY = (sbyte)(0 - topmost);

            if (field.canBlockMove(fieldStruc, (sbyte)((field.Width / 2) - 3), offsetY))
            {
                // first try the centre
                offsetX = (sbyte)((field.Width / 2) - 3);
            }
            else
            {
                // check bounds
                int leftBound = 0 - leftmost,
                    rightBound = field.Width - 1 - rightmost;

                if(rand.Next(2) == 0)
                {
                    // start checking from the left
                    for(int i = leftBound; i <= rightBound; i++)
                    {
                        if(field.canBlockMove(fieldStruc, (sbyte)i, offsetY))
                        {
                            offsetX = (sbyte)i;
                            break;
                        }

                        if(i == rightBound)
                        {
                            // couldn't place it
                            // take this as game over
                            return false;
                        }
                    }
                }
                else
                {
                    // start checking from the right
                    for (int i = rightBound; i >= leftBound; i--)
                    {
                        if (field.canBlockMove(fieldStruc, (sbyte)i, offsetY))
                        {
                            offsetX = (sbyte)i;
                            break;
                        }

                        if (i == leftBound)
                        {
                            // couldn't place it
                            // take this as game over
                            return false;
                        }
                    }
                }
            }
            return true; // successfully placed
        }

        private void fillStruct(bool x1y1, bool x2y1, bool x3y1, bool x4y1, bool x1y2, bool x2y2, bool x3y2, bool x4y2, bool x1y3, bool x2y3, bool x3y3, bool x4y3, bool x1y4, bool x2y4, bool x3y4, bool x4y4)
        {
            fieldStruc[0][0] = x1y1; fieldStruc[0][1] = x2y1; fieldStruc[0][2] = x3y1; fieldStruc[0][3] = x4y1;
            fieldStruc[1][0] = x1y2; fieldStruc[1][1] = x2y2; fieldStruc[1][2] = x3y2; fieldStruc[1][3] = x4y2;
            fieldStruc[2][0] = x1y3; fieldStruc[2][1] = x2y3; fieldStruc[2][2] = x3y3; fieldStruc[2][3] = x4y3;
            fieldStruc[3][0] = x1y4; fieldStruc[3][1] = x2y4; fieldStruc[3][2] = x3y4; fieldStruc[3][3] = x4y4;
        }

        private void colorBlock()
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

        private bool checkRotation(PlayingField field, bool[][] tempBlockStruc)
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
                        return true;
                    }
                    else if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX + 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX += 1;
                        return true;
                    }
                }
                else
                {
                    if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX + 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX += 1;
                        return true;
                    }
                    else if (field.canBlockMove(tempBlockStruc, (sbyte)(offsetX - 1), offsetY))
                    {
                        fieldStruc = tempBlockStruc;
                        offsetX -= 1;
                        return true;
                    }
                }
            }
            return false; // could not rotate
        }

        public bool handleMovement(GameTime gameTime, InputHelper input)
        {
            // handle user inputs
            // go left
            if (input.KeyPressed(Keys.A) && field.canBlockMove(this, Movement.Left))
            { move(Movement.Left); }

            // go right
            if (input.KeyPressed(Keys.D) && field.canBlockMove(this, Movement.Right))
            { move(Movement.Right); }

            if (input.KeyPressed(Keys.S))
            {
                if (field.canBlockMove(this, Movement.Down)) // can still go lower
                {
                    move();
                    moveTimer = 0;
                }
                else
                { return false; } // may no longer move
            }

            if (input.IsKeyDown(Keys.W))
            {
                // move all the way down
                while (field.canBlockMove(this, Movement.Down))
                { move(); }
                moveTimer = 0;
                return false; // may no longer move
            }

            if (input.KeyPressed(Keys.Q)) { turnAntiClockwise(field); }

            if (input.KeyPressed(Keys.E)) { turnClockwise(field); }

            //movement tick
            if (moveTimer >= moveTimerLim)
            {
                moveTimer = moveTimer % moveTimerLim;

                //move block
                if (field.canBlockMove(this, Movement.Down))
                { move(); }
                else
                { return false; } // may no longer move
            }
            else { moveTimer += gameTime.ElapsedGameTime.Milliseconds; }

            return true; // was able to go down if it had to
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (!isNext)
                    {
                        if (checkGridStruct(y, x))
                        {
                            if (isColor) { spriteBatch.Draw(blockSprites[checkGridCol(y, x)], new Vector2((OffsetX + x) * 20, (OffsetY + y) * 20), Color.White); }
                            else { spriteBatch.Draw(blockSprites[1], new Vector2((OffsetX + x) * 20, (OffsetY + y) * 20), Color.White); }
                        }
                    }
                    else
                    {
                        if (checkGridStruct(y, x))
                        {
                            if (isColor) { spriteBatch.Draw(blockSprites[checkGridCol(y, x)], new Vector2(field.Width * 20 + 40 + x * 20, 70 + y * 20), Color.White); }
                            else { spriteBatch.Draw(blockSprites[1], new Vector2(field.Width * 20 + 40 + x * 20, 70 + y * 20), Color.White); }
                        }
                        else
                        { spriteBatch.Draw(blockSprites[0], new Vector2(field.Width * 20 + 40 + x * 20, 70 + y * 20), Color.White); }
                    }
                }
            }
        }

    }
}
