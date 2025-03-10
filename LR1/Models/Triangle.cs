
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
        }
        public Triangle() { }
        public override void Draw(int w, int h)
        {
            
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
