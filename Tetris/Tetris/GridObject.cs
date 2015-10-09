namespace Practicum.Tetris
{
    class GridObject
    {

        protected bool isColor;
        protected byte width, height;

        protected bool[][] fieldStruc;
        protected byte[][] fieldCol;

        public GridObject(byte width, byte height, bool isColor = false)
        {
            this.width = width;
            this.height = height;
            this.isColor = isColor;
            makeField();
            if (isColor) { makeColorField(); }
        }

        private void makeField()
        {
            fieldStruc = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                fieldStruc[i] = new bool[width];
            }
        }

        private void makeColorField()
        {
            fieldCol = new byte[height][];
            for (int i = 0; i < height; i++)
            {
                fieldCol[i] = new byte[width];
            }
        }

    }
}
