//using DocMaster.Utilities;

using DocMaster.Utilities;

namespace DocMaster.UI
{
    public class App
    {
        private readonly Menu _menu = new();
        private UserRole _currentRole;

        public void Run()
        {
            SelectRole();
        }
        public void SelectRole()
        {
            Console.WriteLine("Choose a role:");
            Console.WriteLine("1. Viewer");
            Console.WriteLine("2. Editor");
            Console.WriteLine("3. Admin");

            int choice = InputValidator.GetIntInput(1, 3);
            _currentRole = (UserRole)(choice - 1);
            Console.WriteLine($"-----> Role selected: {_currentRole}");
        }
        public void MainLoop()
        {
            while (true)
            {
                _menu.ShowMainMenu(_currentRole);
                int maxOption = GetMaxMenuOption();
                int choice = InputValidator.GetIntInput(1, maxOption);

                switch (choice)
                {
                    case 1 when CanEdit():

                        break;
                    case 2:

                        break;
                }
            }
        }
        private int GetMaxMenuOption() => _currentRole switch
        {
            UserRole.Admin => 9,
            UserRole.Editor => 7,
            _ => 5
        };
        private bool CanEdit() => _currentRole == UserRole.Editor;
        private bool IsAdmin() => _currentRole == UserRole.Admin;
    }
}
