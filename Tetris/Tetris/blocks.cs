using Microsoft.Xna.Framework.Graphics;

namespace Practicum.Tetris
{
    class blockSquare:TetrisBlock
    {
        public blockSquare(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, true,  true,  false,
                   false, false, false, false,
                   blockSprites, isColor) { }

        public override void turnClockwise(PlayingField field) { }
        public override void turnAntiClockwise(PlayingField field) { }
    }

    class blockLineH:TetrisBlock
    {
        public blockLineH(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, false, false, false,
                   true,  true,  true,  true,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }

    class blockZ:TetrisBlock
    {
        public blockZ(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, false, true,  true,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }

    class blockReverseZ : TetrisBlock
    {
        public blockReverseZ(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  true,
                   false, true,  true,  false,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }

    class blockFlatReverseL:TetrisBlock
    {
        public blockFlatReverseL(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, true,  false, false,
                   false, true,  true,  true,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }

    class blockFlatL:TetrisBlock
    {
        public blockFlatL(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  false,
                   true,  true,  true,  false,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }

    class blockLineV : TetrisBlock
    {
        public blockLineV(Texture2D[] blockSprites, bool isColor)
            : base(false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   blockSprites, isColor) { }
    }

    class blockRoof : TetrisBlock
    {
        public blockRoof(Texture2D[] blockSprites, bool isColor)
            : base(false, false, false, false,
                   false, false, true,  false,
                   false, true,  true,  true,
                   false, false, false, false,
                   blockSprites, isColor) { }
    }
}
