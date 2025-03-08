
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

        }
        public override void Move(int deltaX, int deltaY)
        {
            
        }
        public override Shape Clone()
        {
            
        }
    }
}
