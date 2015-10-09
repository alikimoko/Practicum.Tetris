using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum.Tetris
{
    class blockSquare:Block
    {
       
        public blockSquare(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, true, true, false, false, false, false, false, isColor) 
        {
        
        }

        public override void turnLeft() { }

        public override void turnRight() { }
        
    }
    class blockLineH:Block
    {
        
        public blockLineH(bool isColor = false)
            : base(false, false, false, false, false, false, false, false, true, true, true, true, false, false, false, false, isColor) 
        {
        
        }


    }
    class blockLineV:Block
    {

        public blockLineV(bool isColor = false)
            : base(false, true, false, false, false, true, false, false, false, true, false, false, false, true, false, false, isColor) 
        {
        
        }
    }
    class blockZ:Block
    {
    
        public blockZ(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, false, true, true, false, false, false, false, isColor) 
        {
        
        }
    }
    class blockReverseZ : Block
    {
        public blockReverseZ(bool isColor = false)
            : base(false, false, false, false, false, false, true, true, false, true, true, false, false, false, false, false, isColor)
        {

        }

    }
    class blockFlatReverseL:Block
    {

        public blockFlatReverseL(bool isColor = false)
            : base(false, false, false, false, false, true, false, false, false, true, true, true, false, false, false, false, isColor) 
        {
        
        }

    }
    class blockFlatL:Block
    {
        public blockFlatL(bool isColor = false)
            : base(false, false, false, false, false, false, true, false, true, true, true, false, false, false, false, false, isColor) 
        {
        
        }
    }
    class blockC:Block
    {
        public blockC(bool isColor = false)
            : base(false, false, false, false, false, true, true, false, false, false, true, false, false, false, false, false, isColor) 
        {
        
        }
    
    }
    class blockRoof:Block
    {
        public blockRoof(bool isColor = false)
            : base(false, false, false, false, false, false, true, false, false, true, true, true, false, false, false, false, isColor) 
        {
        
        }
    }
}
