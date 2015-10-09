using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum.Tetris
{
    class blockSquare:TetrisBlock
    {
       
        public blockSquare(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false, isColor) 
        {
        
        }

        public override void turnLeft() { }

        public override void turnRight() { }
        
    }
    class blockLineH:TetrisBlock
    {
        
        public blockLineH(bool isColor = false)
            : base(false, false, false, false, false, false, false, false, true, true, true, true, false, false, false, false, isColor) 
        {
        
        }


    }
    class blockLineV:TetrisBlock
    {

        public blockLineV(bool isColor = false)
            : base(false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, isColor) 
        {
        
        }
    }
    class blockZ:TetrisBlock
    {
    
        public blockZ(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, false, true, true, false, false, false, false, isColor) 
        {
        
        }
    }
    class blockReverseZ : TetrisBlock
    {
        public blockReverseZ(bool isColor = false)
            : base(false, false, false, false, false, false, true, true, false, true, true, false, false, false, false, false, isColor)
        {

        }

    }
    class blockFlatReverseL:TetrisBlock
    {

        public blockFlatReverseL(bool isColor = false)
            : base(false, false, false, false, false, true, false, false, false, true, true, true, false, false, false, false, isColor) 
        {
        
        }

    }
    class blockFlatL:TetrisBlock
    {
        public blockFlatL(bool isColor = false)
            : base(false, false, false, false, false, false, true, false, true, true, true, false, false, false, false, false, isColor) 
        {
        
        }
    }
    class blockC:TetrisBlock
    {
        public blockC(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, false, true, false, false, false, false, false, isColor) 
        {
        
        }
    
    }
    class blockRoof:TetrisBlock
    {
        public blockRoof(bool isColor = false)
            : base(false, false, false, false, false, false, true, false, false, true, true, true, false, false, false, false, isColor) 
        {
        
        }
    }
}
