//using DocMaster.Utilities;

using DocMaster.Roles;
using DocMaster.Services;
using DocMaster.Utilities;

namespace DocMaster.UI
{
    public class App
    {
        private readonly Menu _menu = new();
        private readonly RoleContext _roleContext = new();
        private readonly UserManager _userManager = new();

        public void Run()
        {
            ChooseRole();
            MainLoop();
        }
        public void ChooseRole()
        {
            Console.WriteLine("Choose a role:");
            Console.WriteLine("1. Viewer");
            Console.WriteLine("2. Editor");
            Console.WriteLine("3. Admin");

            int choice = InputValidator.GetIntInput(1, 3);
            _roleContext.SetRole((UserRole)(choice - 1));
            Console.WriteLine($"-----> Role selected: {_roleContext.CurrentRole}");
        }
        public void MainLoop()
        {
            while (true)
            {
                _menu.ShowMainMenu(_roleContext.CurrentRole);
                int maxOption = GetMaxMenuOption();
                Console.Write("Choice: ");
                int choice = InputValidator.GetIntInput(1, maxOption);

                switch (choice)
                {
                    case 1 when _roleContext.CanEditDocument:

                        break;
                    case 2:

                        break;
                    case 7 when _roleContext.CanManageUsers:
                        ManageUsers();
                        break;
                }
            }
        }
        private int GetMaxMenuOption() => _roleContext.CurrentRole switch
        {
            UserRole.Admin => 9,
            UserRole.Editor => 7,
            _ => 5
        };
        //private bool CanEdit() => _currentRole == UserRole.Editor;
        //private bool IsAdmin() => _currentRole == UserRole.Admin;

        private void ManageUsers()
        {
            _menu.ShowAdminMenu();

            int choice = InputValidator.GetIntInput(1, 3);
            if (choice == 1)
            {
                Console.Write("Enter username: ");
                string username  = Console.ReadLine();

                Console.WriteLine("Select new role:");
                Console.WriteLine("1. Viewer\n2. Editor\n3. Admin");
                Console.Write("Choice: ");
                int roleChoice = InputValidator.GetIntInput(1, 3);

                _userManager.ChangeUserRole(username, (UserRole)(roleChoice - 1));
                Console.WriteLine($"Role for {username} updated.");
            }
        }
        
    }
}
