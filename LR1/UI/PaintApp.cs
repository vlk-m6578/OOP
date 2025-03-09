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

                    break;
                case 2:

                    break;
                case 3:

                    break;
            }
            _canvas.AddShape(shape);
        }
        private void MoveShape()
        {
            
        }
        private void RemoveShape()
        {

        }
       
    }
}
