using System.Text.Json;

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
            Program.clear();

            //Process.Start("PAINT.exe", "/c cls");
            //Console.SetCursorPosition(Console.WindowTop, Console.WindowTop);
            //Console.Clear();

            Console.WriteLine(new string('_', _width)); 
            for (int i = 0; i < _height; i++) 
            { 
                Console.WriteLine("|" + new string(' ', _width - 1) + "|"); 
            }
            Console.WriteLine("|" + new string('_', _width - 1) + "|");

            foreach (var shape in _shapes)
            {
                shape.Draw(_width, _height+1);

            }
        }
        public void ClearShapes()
        {
            _shapes.Clear();
        }

        public void AddShape(Shape shape)
        {
            _shapes.Add(shape);
            Display();
        }

        public void ListShapes()
        {
            int i = 0;
            foreach(var shape in _shapes)
            {
                Console.WriteLine((i + 1) + ". " + shape.Name);
                i++;
            }
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
            if(index>=0 && index< _shapes.Count)
            {
                _shapes.RemoveAt(index);
                Display();
            }
        }
    }
}
