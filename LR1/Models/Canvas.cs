
namespace PAINT.Models
{
    public class Canvas
    {
        private int _width;
        private int _height;

        private List<Shape> _shapes;

        public Canvas(int width, int height)
        {
            _width = width;
            _height = height;
            _shapes = new List<Shape>();
        }
        public Canvas() { }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public List<Shape> Shapes
        {
            get { return _shapes; }
            set {  _shapes = value; }
        }

        public void Display()
        {
            
        }
        public void ClearShapes()
        {
            _shapes.Clear();
        }

        public void AddShape(Shape shape)
        {
            _shapes.Add(shape);
        }

        public void ListShapes()
        {
            
        }
        public int ShapesCount()
        {
            return _shapes.Count;
        }
        public Shape GetShape(int index)
        {
            return _shapes[index];
        }

        public void RemoveShape(int index)
        {
           
        }
        public void SaveToFile(string filename)
        {
            
        }
        public void LoadFromFile(string filename)
        {
            
        }
    }
}
