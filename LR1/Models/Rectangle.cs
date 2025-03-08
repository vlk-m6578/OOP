
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
            
        }
        public override void Move(int deltaX, int deltaY)
        {
           
        }
        public override Shape Clone()
        {
      
        }
    }
}
