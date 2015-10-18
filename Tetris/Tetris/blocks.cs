using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class blockSquare:TetrisBlock
    {
        /// <summary>Make a square block.</summary>
        public blockSquare(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, true,  true,  false,
                   false, false, false, false,
                   blockSprites, field, BlockType.Square, isColor) { }

        // square doesn't need to turn
        public override void turnClockwise() { }
        public override void turnAntiClockwise() { }
    }

    class blockLineH:TetrisBlock
    {
        /// <summary>Make a horizontal line.</summary>
        public blockLineH(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, false, false, false,
                   true,  true,  true,  true,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.LineH, isColor) { }

        // rotate arround centre
        public override void turnClockwise()
        { if (checkRotation(turnClockwise4()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise4()) && isColor) { colorBlock(); } }
    }

    class blockZ:TetrisBlock
    {
        /// <summary>Make a Z shaped block.</summary>
        public blockZ(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(true,  true,  false, false,
                   false, true,  true,  false,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.Z, isColor) { }

        // rotate arround base
        public override void turnClockwise()
        { if (checkRotation(turnClockwise3()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise3()) && isColor) { colorBlock(); } }
    }

    class blockReverseZ : TetrisBlock
    {
        /// <summary>Make a reversed Z shape block.</summary>
        public blockReverseZ(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, true,  true,  false,
                   true,  true,  false, false,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.ReverseZ, isColor) { }

        // rotate arround base
        public override void turnClockwise()
        { if (checkRotation(turnClockwise3()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise3()) && isColor) { colorBlock(); } }
    }

    class blockFlatReverseL:TetrisBlock
    {
        /// <summary>Make a reversed L shaped block.</summary>
        public blockFlatReverseL(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(true,  false, false, false,
                   true,  true,  true,  false,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.FlatReverseL, isColor) { }

        // rotate arround base
        public override void turnClockwise()
        { if (checkRotation(turnClockwise3()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise3()) && isColor) { colorBlock(); } }
    }

    class blockFlatL:TetrisBlock
    {
        /// <summary>Make an L shaped block.</summary>
        public blockFlatL(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, false, true,  false,
                   true,  true,  true,  false,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.FlatL, isColor) { }

        // rotate arround base
        public override void turnClockwise()
        { if (checkRotation(turnClockwise3()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise3()) && isColor) { colorBlock(); } }
    }

    class blockLineV : TetrisBlock
    {
        /// <summary>Make a vertical line.</summary>
        public blockLineV(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   blockSprites, field, BlockType.LineV, isColor) { }

        // rotate arround centre
        public override void turnClockwise()
        { if (checkRotation(turnClockwise4()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise4()) && isColor) { colorBlock(); } }
    }

    class blockRoof : TetrisBlock
    {
        /// <summary>Make a T shaped block.</summary>
        public blockRoof(Texture2D blockSprites, PlayingField field, bool isColor)
            : base(false, true,  false, false,
                   true,  true,  true,  false,
                   false, false, false, false,
                   false, false, false, false,
                   blockSprites, field, BlockType.Roof, isColor) { }

        // rotate arround base
        public override void turnClockwise()
        { if (checkRotation(turnClockwise3()) && isColor) { colorBlock(); } }

        public override void turnAntiClockwise()
        { if (checkRotation(turnAntiClockwise3()) && isColor) { colorBlock(); } }
    }
}
