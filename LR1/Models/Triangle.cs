
namespace PAINT.Models
{
    public class Triangle : Shape
    {
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public int x3 { get; set; }
        public int y3 { get; set; }

        public Triangle(string name, char s, char sB, int a, int b, int c) : base(name, s, sB)
        {
            //// Checking the triangle for existance ////
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                return;
            }

            double d = (Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2.0 * a); // projection of the height 3 point
            double e = Math.Sqrt(Math.Pow(b, 2) - Math.Pow(d, 2)); // theorem (how high is it?)

            double localX1 = 0;
            double localY1 = 0;
            double localX2 = a;
            double localY2 = 0;
            double localX3 = d;
            double localY3 = e;

            ////////////////// Center of mass /////////////////////
            double localCenterX = (localX1 + localX2 + localX3) / 3;
            double localCenterY = (localY1 + localY2 + localY3) / 3;

            /// Offset to the center of the canvas ///
            double deltaX = 30 - localCenterX;
            double deltaY = 10 - localCenterY;

            ///////// Final coordinates //////////
            x1 = (int)Math.Round(localX1 + deltaX);
            y1 = (int)Math.Round(localY1 + deltaY);
            x2 = (int)Math.Round(localX2 + deltaX);
            y2 = (int)Math.Round(localY2 + deltaY);
            x3 = (int)Math.Round(localX3 + deltaX);
            y3 = (int)Math.Round(localY3 + deltaY);

            //////////////// Correct ///////////////////
            AdjustCoordinates(x1, x2, x3, 90); // 30 * 3
            AdjustCoordinates(y1, y2, y3, 30); // 10 * 3

        }
        public Triangle() { }
        public override void Draw(int w, int h)
        {
            char s = Symbol, sB = SymbolBackground;

            //////////// Checking the triangle for existance //////////
            int area = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);
            if (area == 0)
            {
                Console.WriteLine("\n----------> The triangle doesn't exist <----------");
                return;
            }

            //////////// Calculate borders of the area ////////////
            int minX = Math.Max(0, Math.Min(x1, Math.Min(x2, x3)));
            int maxX = Math.Min(w - 1, Math.Max(x1, Math.Max(x2, x3)));
            int minY = Math.Max(0, Math.Min(y1, Math.Min(y2, y3)));
            int maxY = Math.Min(h - 1, Math.Max(y1, Math.Max(y2, y3)));

            ///////// Fill triangle //////////
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (IsInside(x, y))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(sB);
                    }
                }
            }

            DrawLine(x1, y1, x2, y2, s);
            DrawLine(x2, y2, x3, y3, s);
            DrawLine(x3, y3, x1, y1, s);

            Console.SetCursorPosition(0, h + 2); // For output menu
        }
        private bool IsInside(int x, int y)
        {                                                   
            int a = (x2 - x1) * (y - y1) - (y2 - y1) * (x - x1);
            int b = (x3 - x2) * (y - y2) - (y3 - y2) * (x - x2);                   // A vector multiply
            int c = (x1 - x3) * (y - y3) - (y1 - y3) * (x - x3);

            return (a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0);   // The same sign
        }
        private void DrawLine(int x1, int y1, int x2, int y2, char s)
        {

            int dx = Math.Abs(x2 - x1), dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1, sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                // Проверка на выход за границы холста (60x20)
                if (x1 >= 0 && x1 < 60 && y1 >= 0 && y1 < 20)
                {
                    Console.SetCursorPosition(x1, y1);
                    Console.Write(s);
                }

                if (x1 == x2 && y1 == y2) break;

                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x1 += sx; }
                if (e2 < dx) { err += dx; y1 += sy; }
            }
        }
        private void AdjustCoordinates(int a, int b, int c, int targetSum)
        {
            int sum = a + b + c;
            int diff = sum - targetSum;

            if (diff == 0) return;

            int steps = Math.Abs(diff);

            for (int i = 0; i < steps; i++)
            {
                if (diff > 0)
                {
                    //// < max coordinate ////
                    if (a >= b && a >= c) a--;
                    else if (b >= c) b--;
                    else c--;
                }
                else
                {
                    //// > min coordinate ////
                    if (a <= b && a <= c) a++;
                    else if (b <= c) b++;
                    else c++;
                }
            }
        }
        public override void Move(int deltaX, int deltaY)
        {
            x1 += deltaX;
            y1 += deltaY;
            x2 += deltaX;
            y2 += deltaY;
            x3 += deltaX;
            y3 += deltaY;
        }
        public override Shape Clone()
        {
            return new Triangle(this.Name, this.Symbol, this.SymbolBackground, 0, 0, 0)
            {
                x1=this.x1,
                y1=this.y1,
                x2=this.x2,
                y2=this.y2,
                x3=this.x3,
                y3=this.y3
            };
        }
    }
}
