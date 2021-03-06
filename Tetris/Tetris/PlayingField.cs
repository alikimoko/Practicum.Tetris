﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class PlayingField : GridObject
    {
        Color white = Color.White;

        /// <summary>Create a new playing field.</summary>
        /// <param name="width">The width of the playing field in cels.</param>
        /// <param name="blockSprites">The sprites for the blocks.</param>
        /// <param name="height">The height of the playing field in cels.</param>
        /// <param name="isColor">Are colors enabled?</param>
        public PlayingField(byte width, byte height, Texture2D blockSprites, bool isColor = false) :
            base(width, height, blockSprites, isColor)
        {
            // get an empty playing field
            reset();
        }

        /// <summary>Checks if rows have been cleared.</summary>
        /// <param name="blockOffset">The offset of the placed block.</param>
        public int[] checkClearedRows(sbyte blockOffset)
        {
            List<byte> clearedRows = new List<byte>();
            int[] scoreChange = new int[] { 0, 0, // completed rows, rows with only one color
                                            0, 0, 0, 0, 0, 0 }; // blocks of each color

            for(int y=3; y >= 0; y--) // only check the rows where the block has been placed
            {
                if(blockOffset + y < height && blockOffset >= 0) // inside the field
                {
                    for(int x = 0; x < width; x++)
                    {
                        if(!fieldStruc[blockOffset + y][x])
                        {
                            // no block here -> row not complete
                            break;
                        }
                        if(x == width - 1)
                        {
                            // entire row filled
                            clearedRows.Add((byte)(blockOffset + y));
                            scoreChange[0] += 1;
                            if (isColor)
                            {
                                // get the colors of the block
                                for (int X = 0; X < width; X++)
                                {
                                    scoreChange[fieldCol[blockOffset + y][X] + 1] += 1;
                                }

                                byte rowColorCheck = 0;
                                // is the entire row 1 color?
                                for (int X = 0; X < width; X++)
                                {
                                    if (X == 0) { rowColorCheck = fieldCol[blockOffset + y][X]; }
                                    else if (X == width - 1) { scoreChange[1] += 1; }
                                    else if (rowColorCheck != fieldCol[blockOffset + y][X]) { break; }
                                }
                            }
                        }
                    }
                }
            }

            if (clearedRows.Count > 0)
            {
                // rows have been cleared
                moveRows(clearedRows);
            }
            
            return scoreChange;
        }

        /// <summary>Moves the rows after the row has been cleared.</summary>
        /// <param name="clearedRows">List of the rows that have been completed.</param>
        private void moveRows(List<byte> clearedRows)
        {
            for(int rows = 0; rows < clearedRows.Count; rows++)
            {
                // move the rows to the new position
                for (int rowNum = clearedRows[rows] + rows; rowNum > 0; rowNum--)
                {
                    fieldStruc[rowNum] = fieldStruc[rowNum - 1];
                    if (isColor) { fieldCol[rowNum] = fieldCol[rowNum - 1]; }
                }

                // make a new empty row on top
                fieldStruc[0] = new bool[width];
                if (isColor) { fieldCol[0] = new byte[width]; }

                for (int j = 0; j < width; j++)
                {
                    fieldStruc[0][j] = false;
                    if (isColor) { fieldCol[0][j] = 0; }
                }
            }
        }

        /// <summary>Checks if the block can move in a given direction.</summary>
        /// <param name="blockToMove">The block that should be checked.</param>
        /// <param name="direction">The direction of the displacement. 0 = down, 1 = left, 2 = right, default = stay</param>
        public bool canBlockMove(TetrisBlock blockToMove, Movement direction = Movement.Stay)
        {
            return canBlockMove(blockToMove.FieldStruct, blockToMove.OffsetX, blockToMove.OffsetY, direction);
        }

        /// <summary>Checks if the block can move in a given direction.</summary>
        /// <param name="block">The structure to check.</param>
        /// <param name="offsetX">The block its x offset from the playing field.</param>
        /// <param name="offsetX">The block its y offset from the playing field.</param>
        /// <param name="direction">The direction of the displacement. 0 = down, 1 = left, 2 = right, default = stay</param>
        public bool canBlockMove(bool[][] block, sbyte offsetX, sbyte offsetY, Movement direction = Movement.Stay)
        {
            sbyte displaceX = 0, displaceY = 0;

            switch (direction)
            {
                case Movement.Down:
                    displaceY = 1;
                    break;
                case Movement.Left:
                    displaceX = -1;
                    break;
                case Movement.Right:
                    displaceX = 1;
                    break;
                default:
                    // stay at the same place (for rotation checking)
                    break;
            }

            // check cels after displacement
            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 4; x++)
                {
                    // at edge of playing field?
                    if(checkGridStruct(block, y, x) && (offsetY + y + displaceY < 0 || // cel above the field (can only happen after turning some blocks just after they were placed)
                                                        offsetY + y + displaceY >= height || // cel below the field
                                                        offsetX + x + displaceX >= width || // cel right of the field
                                                        offsetX + x + displaceX < 0)) // cel left of the field
                    { return false; }

                    // already occupied?
                    if (offsetY + y + displaceY >= 0 &&
                        offsetY + y + displaceY < height &&
                        offsetX + x + displaceX < width &&
                        offsetX + x + displaceX >= 0)
                    {
                        // only empty rows can get outside the field
                        // (index out of bounds exeption if this check doesn't happen)
                        if (checkGridStruct(offsetY + y + displaceY, offsetX + x + displaceX) &&
                            checkGridStruct(block, y, x))
                        { return false; }
                    }
                }
            }

            return true;
        }

        /// <summary>Copies the block data to the field data uppon placement.</summary>
        /// <param name="block">The block to place.</param>
        public void placeBlock(TetrisBlock block)
        {
            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 4; x++)
                {
                    if (block.checkGridStruct(y, x) &&
                        block.OffsetX + x >= 0 &&
                        block.OffsetX + x < width &&
                        block.OffsetY + y < height)
                    {
                        // within the field
                        fieldStruc[y + block.OffsetY][x + block.OffsetX] = true;
                        if (isColor) { fieldCol[y + block.OffsetY][x + block.OffsetX] = block.blockColor; }
                    }
                }
            }
        }

        /// <summary>Has the field been filled to the top?</summary>
        public bool reachedTop()
        {
            for(int x = 0; x < width; x++)
            {
                if (fieldStruc[0][x]) { return true; }
            }
            return false;
        }

        /// <summary>Completely cleares the playing field.</summary>
        public void reset()
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    fieldStruc[y][x] = false;
                    if(isColor) { fieldCol[y][x] = 0; }
                }
            }
        }

        /// <summary>Draw the field.</summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (checkGridStruct(y, x))
                    {
                        if (isColor) { spriteBatch.Draw(blockSprites, new Vector2(x * 20, y * 20), new Rectangle(checkGridCol(y, x) * 20, 0, 20, 20), white); }
                        else { spriteBatch.Draw(blockSprites, new Vector2(x * 20, y * 20), new Rectangle(20, 0, 20, 20), white); }
                    }
                    else { spriteBatch.Draw(blockSprites, new Vector2(x * 20, y * 20), new Rectangle(0, 0, 20, 20), white); }
                }
            }
        }
    }
}
