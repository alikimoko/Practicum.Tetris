using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class blockSquare:TetrisBlock
    {
        /// <summary>Make a square block.</summary>
        public blockSquare(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, true,  true,  false,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.Square, isColor) { }

        // square doesn't need to turn
        public override void turnClockwise(PlayingField field) { }
        public override void turnAntiClockwise(PlayingField field) { }
    }

    class blockLineH:TetrisBlock
    {
        public blockLineH(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   true,  true,  true,  true,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.LineH, isColor) { }
    }

    class blockZ:TetrisBlock
    {
        public blockZ(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, false, true,  true,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.Z, isColor) { }
    }

    class blockReverseZ : TetrisBlock
    {
        public blockReverseZ(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  true,
                   false, true,  true,  false,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.ReverseZ, isColor) { }
    }

    class blockFlatReverseL:TetrisBlock
    {
        public blockFlatReverseL(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, true,  false, false,
                   false, true,  true,  true,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.FlatReverseL, isColor) { }
    }

    class blockFlatL:TetrisBlock
    {
        public blockFlatL(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  false,
                   true,  true,  true,  false,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.FlatL, isColor) { }
    }

    class blockLineV : TetrisBlock
    {
        public blockLineV(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   blockSprites, field, moveTimerLim, BlockType.LineV, isColor) { }
    }

    class blockRoof : TetrisBlock
    {
        public blockRoof(Texture2D blockSprites, PlayingField field, int moveTimerLim, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  false,
                   false, true,  true,  true,
                   false, false, false, false,
                   blockSprites, field, moveTimerLim, BlockType.Roof, isColor) { }
    }
}
