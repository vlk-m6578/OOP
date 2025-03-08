
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
            
        }
        public override Shape Clone()
        {
           
        }
    }
}
