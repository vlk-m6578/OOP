
namespace PAINT.Models
{
    public class Rectangle : Shape
    {
        int _length;
        int _height;
        public Rectangle(string name, char s, char sB, int length, int height) : base(name, s, sB)
        {
            _height = height;
            _length = length;
            X = 30 - length / 2;
            Y = 10 - height / 2;
        }
        public Rectangle() { }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        public override void Draw(int w, int h)
        {
            char s = Symbol; char sB = SymbolBackground;

            ///// Fill borders of the canvas //////
            char[,] consoleBuffer = new char[h, w];
            for (int x = 0; x < w; x++)
            {
                consoleBuffer[0, x] = '_';
            }
            for (int y = 1; y < h - 1; y++)
            {
                consoleBuffer[y, 0] = '|';
                for (int x = 1; x < w - 1; x++)
                {
                    consoleBuffer[y, x] = ' ';
                }
                consoleBuffer[y, w - 1] = '|';
            }

            int startX = Math.Max(X, 0);
            int startY = Math.Max(Y, 0);
            int endX = Math.Min(X + _length, w);
            int endY = Math.Min(Y + _height, h);

            for (int y = startY; y < endY; y++)
            {
                Console.SetCursorPosition(startX, y);
                for (int x = startX; x < endX; x++)
                {
                    if (y == Y || y == Y + _height - 1 || x == X || x == X + _length - 1)
                    {
                        if (consoleBuffer[y, x] == '|' || consoleBuffer[y, x] == '_')
                        {
                            continue; // don't touch canvas

                        }
                        Console.Write(s);
                    }
                    else
                    {
                        if (consoleBuffer[y, x] == '|' || consoleBuffer[y, x] == '_')
                        {
                            continue; // don't touch canvas
                        }
                        Console.Write(sB);
                    }
                }
            }
            Console.SetCursorPosition(0, h+2); // For output menu
        }
    }
}
