using PAINT.Models;
using PAINT.Utilities;

namespace PAINT.UI
{
    public class PaintApp
    {
        private Canvas _canvas;
        private Menu _menu;
        private CommandManager _commandManager = new CommandManager();
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
                        SaveCanvas();
                        break;
                    case 6:
                        LoadCanvas();
                        break;
                    case 7:
                        Undo();
                        break;
                    case 8:
                        Redo();
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

            //Console.Write("Enter canvas width: ");
            //int width = InputValidator.GetIntInput(1, 100);
            int width = 60;
            //Console.Write("Enter canvas height: ");
            //int height=InputValidator.GetIntInput(1,100);
            int height = 20;

            _canvas = new Canvas(width, height);
            _commandManager.SaveState(_canvas);
            Console.SetCursorPosition(0, 0);
            //Console.Clear();
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
                    Console.Write("Enter shape name: ");
                    string name2 = Console.ReadLine();

                    Console.Write("Enter the first side of the triangle: ");
                    int a = InputValidator.GetIntInput(2, 59);
                    Console.Write("Enter the second side of the triangle: ");
                    int b = InputValidator.GetIntInput(2, 59);
                    Console.Write("Enter the third side of the triangle: ");
                    int c = InputValidator.GetIntInput(2, 59);
                    shape = new Triangle(name2, symbol, symbolBackground, a, b, c);
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
            _commandManager.SaveState(_canvas);
        }
        private void MoveShape()
        {
            if (_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------"); 
                return;
            }
            if (_canvas.ShapesCount() == 0)
            {
                Console.WriteLine("\n----------> No shapes to move <----------"); 
                return;
            }

            Console.WriteLine("\nSelect a shape to move: ");
            _canvas.ListShapes();
            Console.Write("Choose: ");
            int index = InputValidator.GetIntInput(1, _canvas.ShapesCount());
            
            Console.Write("Enter delta X:");
            int deltaX = InputValidator.GetIntInput(-Console.WindowWidth, Console.WindowWidth);
            Console.Write("Enter delta Y:");
            int deltaY = InputValidator.GetIntInput(-Console.WindowHeight, Console.WindowHeight);

            Shape selectedShape = _canvas.GetShape(index-1);
            selectedShape.Move(deltaX, deltaY);
            _canvas.Display();
            _commandManager.SaveState(_canvas);
        }
        private void RemoveShape()
        {
            if (_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------");
                return;
            }
            if (_canvas.ShapesCount() == 0)
            {
                Console.WriteLine("\n----------> No shapes to remove <----------");
                return;
            }
            Console.WriteLine("\nSelect a shape to remove: ");
            _canvas.ListShapes();
            Console.Write("Choose: ");
            int index = InputValidator.GetIntInput(1, _canvas.ShapesCount());

            _canvas.RemoveShape(index - 1);
            _commandManager.SaveState(_canvas);
            _canvas.Display();
        }
        private void SaveCanvas()
        {
            if (_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------");
                return;
            }
            try
            {
                _canvas.SaveToFile("canvas.json");
                Console.WriteLine("\n----------> Canvas saved successfully on the way \"D:\\PAINT\\bin\\Debug\\net8.0\\canvas.json\" <----------");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
        private void LoadCanvas()
        {
            if (_canvas != null)
            {
                _canvas.LoadFromFile("canvas.json");
                _commandManager.SaveState(_canvas);
            }
            else
            {
                Console.WriteLine("\n----------> No canvas to save <----------");
            }
        }
        private void Undo()
        {
            if (_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------");
                return;
            }

            if (!_commandManager.Undo(_canvas))
                Console.WriteLine("\n----------> Nothing to undo <----------");
        }

        private void Redo()
        {
            if (_canvas == null)
            {
                Console.WriteLine("\n----------> Canvas not created <----------");
                return;
            }

            if (!_commandManager.Redo(_canvas))
                Console.WriteLine("\n----------> Nothing to redo <----------");
        }
    }
}
