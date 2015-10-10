namespace Practicum.Tetris
{
    class blockSquare:TetrisBlock
    {
       
        public blockSquare(byte color)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, true,  true,  false,
                   false, false, false, false, color) 
        {
        
        }
        public blockSquare()
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, true,  true,  false,
                   false, false, false, false)
        {

        }

        public override void turnClockwise(PlayingField field) { }

        public override void turnAntiClockwise(PlayingField field) { }
        
    }
    class blockLineH:TetrisBlock
    {
        
        public blockLineH(byte color)
            : base(false, false, false, false,
                   false, false, false, false,
                   true,  true,  true,  true,
                   false, false, false, false, color) 
        {
        
        }

        public blockLineH()
            : base(false, false, false, false,
                   false, false, false, false,
                   true,  true,  true,  true,
                   false, false, false, false)
        {

        }


    }
    class blockLineV:TetrisBlock
    {

        public blockLineV(byte color)
            : base(false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   false, true, false, false, color) 
        {
        
        }

        public blockLineV()
            : base(false, true, false, false,
                   false, true, false, false,
                   false, true, false, false,
                   false, true, false, false)
        {

        }
    }
    class blockZ:TetrisBlock
    {
    
        public blockZ(byte color)
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, false, true,  true,
                   false, false, false, false, color) 
        {
        
        }

        public blockZ()
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, false, true,  true,
                   false, false, false, false)
        {

        }
    }
    class blockReverseZ : TetrisBlock
    {
        public blockReverseZ(byte color)
            : base(false, false, false, false,
                   false, false, true,  true,
                   false, true,  true,  false,
                   false, false, false, false, color)
        {

        }
        
        public blockReverseZ()
            : base(false, false, false, false,
                   false, false, true,  true,
                   false, true,  true,  false,
                   false, false, false, false)
        {

        }


    }
    class blockFlatReverseL:TetrisBlock
    {

        public blockFlatReverseL(byte color)
            : base(false, false, false, false,
                   false, true,  false, false,
                   false, true,  true,  true,
                   false, false, false, false, color) 
        {
        
        }
        
        public blockFlatReverseL()
            : base(false, false, false, false,
                   false, true,  false, false,
                   false, true,  true,  true,
                   false, false, false, false)
        {

        }

    }
    class blockFlatL:TetrisBlock
    {
        public blockFlatL(byte color)
            : base(false, false, false, false,
                   false, false, true,  false,
                   true,  true,  true,  false,
                   false, false, false, false, color) 
        {
        
        }
        
        public blockFlatL()
            : base(false, false, false, false,
                   false, false, true,  false,
                   true,  true,  true,  false,
                   false, false, false, false)
        {

        }
    }
    class blockC:TetrisBlock
    {
        public blockC(byte color)
            : base(false, false, false, false, 
                   false, true,  true,  false,
                   false, false, true,  false,
                   false, false, false, false, color) 
        {
        
        }

        public blockC()
            : base(false, false, false, false,
                   false, true,  true,  false,
                   false, false, true,  false,
                   false, false, false, false)
        {

        }
    
    }
    class blockRoof:TetrisBlock
    {
        public blockRoof(byte color)
            : base(false, false, false, false,
                   false, false, true,  false,
                   false, true,  true,  true,
                   false, false, false, false, color) 
        {
        
        }

        public blockRoof()
            : base(false, false, false, false,
                   false, false, true,  false,
                   false, true,  true,  true,
                   false, false, false, false)
        {

        }
    }
}
