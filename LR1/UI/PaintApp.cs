using PAINT.Models;
using PAINT.Utilities;

namespace PAINT.UI
{
    public class PaintApp
    {
        private Canvas _canvas;
        private Menu _menu;
        public PaintApp()
        {
            _menu = new Menu();
        }

        public void Run()
        {
            bool isRun = true;

            Console.Clear();
            if (_canvas != null)
            {
                _canvas.Display();
            }

            while(isRun)
            {
                _menu.ShowMainMenu();
                int choice = InputValidator.GetIntInput(1, 9);

                switch(choice)
                {
                    case 1:
                        CreateCanvas();
                        break;
                    case 2:
                        AddShape();
                        break;
                    case 3:
                        RemoveShape();
                        break;
                    case 4:
                        MoveShape();
                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:

                        break;
                    case 8:

                        break;
                    case 9:
                        isRun = false;
                        break;
                }
            }
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
        private void CreateCanvas()
        {
            Console.Clear();
            if(_canvas != null )
            {
                _canvas.ClearShapes();
            }

            int width = 60;
            int height = 20;

            _canvas = new Canvas(width, height);
            Console.SetCursorPosition(0, 0);
            _canvas.Display();
        }
        private void AddShape()
        {
            if(_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------");
                return;
            }

            Shape shape = null;
            Console.Write("Enter the symbol: ");
            char symbol = Console.ReadKey().KeyChar;

            Console.WriteLine();

            Console.Write("Enter the symbol of the background: ");
            char symbolBackground=Console.ReadKey().KeyChar;

            _menu.ShowShapesMenu();
            int choice = InputValidator.GetIntInput(1, 3);

            

            switch(choice)
            {
                case 1:
                    Console.Write("Enter shape name: ");
                    string name1 = Console.ReadLine();

                    Console.Write("Enter width for the rectangle: ");
                    int width = InputValidator.GetIntInput(2, 60);
                    Console.Write("Enter length for the rectangle: ");
                    int height = InputValidator.GetIntInput(2, 20);

                    shape = new Rectangle(name1, symbol, symbolBackground, width, height);
                    break;
                case 2:

                    break;
                case 3:
                    Console.Write("Enter shape name: ");
                    string name3 = Console.ReadLine();

                    Console.Write("Enter radius for the circle: ");
                    int radius = InputValidator.GetIntInput(2, Console.WindowWidth);            
                    shape = new Circle(name3, symbol, symbolBackground, radius);
                    break;
            }
            _canvas.AddShape(shape);
        }
        private void MoveShape()
        {

        }
        private void RemoveShape()
        {
            Console.WriteLine("\nSelect a shape to remove: ");
            _canvas.ListShapes();
            Console.Write("Choose: ");
            int index = InputValidator.GetIntInput(1, _canvas.ShapesCount());

            _canvas.RemoveShape(index - 1);
            _canvas.Display();
        }
    }
}
