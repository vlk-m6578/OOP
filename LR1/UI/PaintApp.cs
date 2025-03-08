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
            
        }
        private void AddShape()
        {
            
        }
        private void MoveShape()
        {
            
        }
        
    }
}
