namespace Practicum.Tetris
{
    class PlayingField : GridObject
    {

        /// <summary>
        /// Create a new playing field.
        /// </summary>
        /// <param name="width">The width of the playing field in cels.</param>
        /// <param name="height">The height of the playing field in cels.</param>
        /// <param name="isColor">Are colors enabled?</param>
        public PlayingField(byte width, byte height, bool isColor = false) : base(width, height, isColor)
        {
            // get an empty playing field
            reset();
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
        
        /// <summary>Checks if the block can move in a given direction.</summary>
        /// <param name="blockToMove">The block that should be checked.</param>
        /// <param name="direction">The direction of the displacement. 0 = down, 1 = left, 2 = right, default = stay</param>
        public bool canBlockMove(TetrisBlock blockToMove, sbyte direction = -1)
        {
            return canBlockMove(blockToMove.FieldStruct, blockToMove.OffsetX, blockToMove.OffsetY, direction);
        }

        /// <summary>Checks if the block can move in a given direction.</summary>
        /// <param name="block">The structure to check.</param>
        /// <param name="offsetX">The block its x offset from the playing field.</param>
        /// <param name="offsetX">The block its y offset from the playing field.</param>
        /// <param name="direction">The direction of the displacement. 0 = down, 1 = left, 2 = right, default = stay</param>
        public bool canBlockMove(bool[][] block, sbyte offsetX, sbyte offsetY, sbyte direction = -1)
        {
            sbyte displaceX = 0, displaceY = 0;

            switch (direction)
            {
                case 0:
                    displaceY = 1;
                    break;
                case 1:
                    displaceX = -1;
                    break;
                case 2:
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
                    if(checkGridStruct(block, y,x) && (offsetY + y + displaceY >= height ||
                                                       offsetX + x + displaceX >= width ||
                                                       offsetX + x + displaceX < 0))
                    { return false; }

                    // already occupied?
                    if (offsetY + y + displaceY < height &&
                        offsetX + x + displaceX < width &&
                        offsetX + x + displaceX >= 0)
                    { // only empty rows can get outside the field (index out of bounds exeption if this doesn't happen)
                        if (checkGridStruct(offsetY + y + displaceY, offsetX + x + displaceX) &&
                            checkGridStruct(block, y, x))
                        { return false; }
                    }
                }
            }

            return true;
        }

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
                        fieldStruc[y + block.OffsetY][x + block.OffsetX] = true;
                        if (isColor) { fieldCol[y + block.OffsetY][x + block.OffsetX] = block.Color; }
                    }
                }
            }
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
    }
}
