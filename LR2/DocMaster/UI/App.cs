using DocMaster.Roles;
using DocMaster.Services;
using DocMaster.Utilities;
using DocMaster.Models;

namespace DocMaster.UI
{
    public class App
    {
        private readonly Menu _menu = new();
        private readonly RoleContext _roleContext = new();
        private readonly UserManager _userManager = new();
        private User _currentUser;

        public void Run()
        {
            InitializeUsers();
            while (true)
            {
                var selectedRole = ChooseRole();
                if(selectedRole == UserRole.Admin)
                {
                    if (!HandleAdminLogin()) continue;
                }
                else
                {
                    HandleUserLogin(selectedRole);
                }
                MainLoop();
            }
        }
        private void InitializeUsers()
        {
            _userManager.AddUser(new User("admin", UserRole.Admin));
        }
        
        private UserRole ChooseRole()
        {
            Console.Clear();
            Console.WriteLine("Choose a role:");
            Console.WriteLine("1. Viewer");
            Console.WriteLine("2. Editor");
            Console.WriteLine("3. Admin");
            Console.WriteLine("4. Exit");
            Console.Write("Choice: ");

            int choice = InputValidator.GetIntInput(1, 4);
            if(choice == 4) Environment.Exit(0);
            return (UserRole)(choice - 1);
        }
        private bool HandleAdminLogin()
        {
            Console.Write("Enter admin password: ");
            string password = Console.ReadLine();
            if (password != "parol")
            {
                Console.WriteLine("Incorrect password! Press any key...");
                Console.ReadKey();
                return false;
            }
            _currentUser = _userManager.GetUser("admin");
            _roleContext.SetRole(UserRole.Admin);
            Console.WriteLine($"Welcome, Admin!");
            Console.Write("Press any key...");
            Console.ReadKey();
            return true;
        }
        private void HandleUserLogin(UserRole role)
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            _currentUser = _userManager.GetUser(username);
            
            if(_currentUser ==  null)
            {
                _currentUser = new User(username, role);
                _userManager.AddUser(_currentUser);
            }

            _roleContext.SetRole(role);
            Console.WriteLine($"Welcome, {username}! Role: {role}.");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
        private void MainLoop()
        {
            while (true)
            {
                //Console.Clear();
                _menu.ShowMainMenu(_roleContext.CurrentRole);
                int maxOption = GetMaxMenuOption();
                int choice = InputValidator.GetIntInput(1, maxOption);

                if ((maxOption == 9 && choice == 9) || (maxOption == 7 && choice == 7) || (maxOption == 2 && choice == 2))
                {
                    _currentUser = null;
                    return;
                }
                if(_roleContext.CurrentRole == UserRole.Admin)
                {
                    switch (choice)
                    {
                        case 1 when _roleContext.CanEditDocument:
                            //create
                            break;
                        case 2:
                            //open
                            break;
                        case 3:
                            //view
                            break;
                        case 4:
                            //edit
                            break;
                        case 5:
                            //save
                            break;
                        case 6:
                            //text formattig
                            break;
                        case 7 when _roleContext.CanManageUsers:
                            ManageUsers();
                            break;
                        case 8:
                            //system settings
                            break;
                        case 9:
                            break;
                    }
                }
                else if(_roleContext.CurrentRole == UserRole.Editor)
                {
                    switch (choice)
                    {
                        case 1 when _roleContext.CanEditDocument:
                            //create
                            break;
                        case 2:
                            //open
                            break;
                        case 3:
                            //view
                            break;
                        case 4:
                            //edit
                            break;
                        case 5:
                            //save
                            break;
                        case 6:
                            //text formatting
                            break;
                        case 7 when _roleContext.CanManageUsers:
                            ManageUsers();
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            //read for viewer
                            break;
                        case 2:
                            break;
                    }
                }
            }
        }
        private int GetMaxMenuOption() => _roleContext.CurrentRole switch
        {
            UserRole.Admin => 9,
            UserRole.Editor => 7,
            _ => 2
        };

        private void ManageUsers()
        {
            Console.Clear();
            _menu.ShowAdminMenu();

            int choice = InputValidator.GetIntInput(1, 3);
            if (choice == 1)
            {
                Console.Write("\nEnter username: ");
                string username  = Console.ReadLine();

                var targetUser = _userManager.GetUser(username);
                if (targetUser == null)
                {
                    Console.WriteLine($"\n-----> User {username} not found!");
                    return;
                }
                if (targetUser.Username == _currentUser.Username)
                {
                    Console.WriteLine("\n-----> You cannot change your own role!");
                    return;
                }

                Console.WriteLine("Select new role:");
                Console.WriteLine("1. Viewer\n2. Editor");
                Console.Write("Choice: ");
                int roleChoice = InputValidator.GetIntInput(1, 2);

                _userManager.ChangeUserRole(username, (UserRole)(roleChoice - 1));
                //Console.WriteLine($"\n-----> Role for {username} updated.");
            }
            else if(choice == 2)
            {
                Console.WriteLine("\nAll users:");
                foreach (var user in _userManager.GetAllUsers())
                {
                    Console.WriteLine($"{user.Username} - {user.CurrentRole}");
                }
            }
        }
        
    }
}
