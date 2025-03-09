
namespace PAINT.Models
{
    public class Circle : Shape
    {
        private int _radius;

        public Circle(string name, char s, char sB, int radius) : base(name, s, sB)
        {
            _radius = radius;
            X = 30; Y = 10;
        }
        public Circle() { }

        public int Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public override void Draw(int w, int h)
        {
            int centerX = X; int centerY = Y; char s = Symbol; char sB = SymbolBackground;

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
            for (int x = 0; x < w; x++)
            {
                consoleBuffer[h - 1, x] = '_';
            }

            /// Calculate the borders of the circle ////
            int startX = Math.Max(centerX - _radius, 0);
            int endX = Math.Min(centerX + _radius, w - 1);
            int startY = Math.Max(centerY - _radius, 0);
            int endY = Math.Min(centerY + _radius, h - 1);

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    /////////////////////// Calculate distance from the center /////////////////////
                    double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));

                    if (distance < _radius - 0.5) // Fill circle
                    {
                        Console.SetCursorPosition(x, y);
                        if (consoleBuffer[y, x] == '|' || consoleBuffer[y, x] == '_')
                        {
                            continue;
                        }
                        Console.Write(sB);
                    }
                    else if (distance >= _radius - 0.5 && distance <= _radius + 0.5) // Fill borders
                    {
                        Console.SetCursorPosition(x, y);
                        if (consoleBuffer[y, x] == '|' || consoleBuffer[y, x] == '_')
                        {
                            continue;
                        }
                        Console.Write(s);
                    }
                }
            }
            Console.SetCursorPosition(0, h + 2); // For output menu
        }
        public override void Move(int deltaX, int deltaY)
        {
            
        }
        public override Shape Clone()
        {
            
        }
    }
}
