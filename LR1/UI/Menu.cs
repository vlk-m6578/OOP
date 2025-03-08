
namespace PAINT.UI
{
    public class Menu
    {
        public void ShowMainMenu()
        {
            Console.WriteLine("\nChoose an option: ");
            Console.WriteLine("1) Create canvas");
            Console.WriteLine("2) Add shape");
            Console.WriteLine("3) Remove shape");
            Console.WriteLine("4) Move shape");
            Console.WriteLine("5) Save");
            Console.WriteLine("6) Load");
            Console.WriteLine("7) Undo");
            Console.WriteLine("8) Redo");
            Console.WriteLine("9) Exit");
            Console.Write("Option: ");
        }
        public void ShowShapesMenu()
        {
            Console.WriteLine("\nChoose an option: ");
            Console.WriteLine("1) Add Rectangle");
            Console.WriteLine("2) Add Triangle");
            Console.WriteLine("3) Add Circle");
            Console.Write("Option: ");
        }
    }
}
