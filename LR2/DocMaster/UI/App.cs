using DocMaster.Roles;
using DocMaster.Services;
using DocMaster.Utilities;
using DocMaster.Models;
using DocMaster.Services.FileService;
using DocMaster.Services.StorageStrategies;

namespace DocMaster.UI
{
    public class App
    {
        private readonly Menu _menu = new();
        private readonly RoleContext _roleContext = new();
        private readonly UserManager _userManager = new();

        private User _currentUser;
        private readonly DocumentManager _documentManager = new DocumentManager(new LocalFileService(), Directory.GetCurrentDirectory());
        private Document _currentDocument;

        public void Run()
        {
            InitializeUsers();
            while (true)
            {
                var selectedRole = ChooseRole();
                if (selectedRole == UserRole.Admin)
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
            if (choice == 4) Environment.Exit(0);
            return (UserRole)(choice - 1);
        }
        private bool HandleAdminLogin()
        {
            Console.Write("\nEnter admin password: ");
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
            Console.Write("\nEnter your username: ");
            string username = Console.ReadLine();
            _currentUser = _userManager.GetUser(username);

            if (_currentUser == null)
            {
                _currentUser = new User(username, role);
                _userManager.AddUser(_currentUser);
            }

            _roleContext.SetRole(role);
            Console.WriteLine($"Welcome, {username}! Your role: {role}.");
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
                if (_roleContext.CurrentRole == UserRole.Admin)
                {
                    switch (choice)
                    {
                        case 1 when _roleContext.CanEditDocument:
                            CreateDocument();
                            break;
                        case 2:
                            OpenDocument();
                            break;
                        case 3:
                            DeleteDocument();
                            break;
                        case 4:
                            //EditDocument();
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
                else if (_roleContext.CurrentRole == UserRole.Editor)
                {
                    switch (choice)
                    {
                        case 1 when _roleContext.CanEditDocument:
                            CreateDocument();
                            break;
                        case 2:
                            OpenDocument();
                            break;
                        case 3:
                            DeleteDocument();
                            break;
                        case 4:
                            //EditDocument();
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
                            OpenDocument();
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
                string username = Console.ReadLine();

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
            else if (choice == 2)
            {
                Console.WriteLine("\nAll users: ");
                Console.WriteLine("-------------");
                foreach (var user in _userManager.GetAllUsers())
                {
                    if (user.CurrentRole == UserRole.Admin)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"|* {user.Username} - {user.CurrentRole}");
                        Console.ResetColor();
                        continue;
                    }
                    Console.WriteLine($"| {user.Username} - {user.CurrentRole}");
                }
            }
        }

        private void CreateDocument()
        {
            Console.Write("Enter document name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Select document format:");
            Console.WriteLine("1. TXT");
            Console.WriteLine("2. Markdown");
            Console.WriteLine("3. RichText");
            Console.Write("Choice: ");

            int formatChoice = InputValidator.GetIntInput(1, 3);
            DocumentFormat format = (DocumentFormat)(formatChoice - 1);

            _currentDocument = _documentManager.CreateDocument(name, format);
            _documentManager.SaveDocument(_currentDocument);

            Console.WriteLine($"-----> Document {name} created successfully!");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private void OpenDocument()
        {
            var documents = _documentManager.GetDocumentList();

            if (documents.Count == 0)
            {
                Console.WriteLine("-----> No documents found!");
                Console.Write("Press any key...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nAvailable documents:");
            Console.WriteLine("--------------------");
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine($"{i + 1}.* {Path.GetFileName(documents[i])}");
            }

            Console.Write("\nEnter file number or full path:");
            string input = Console.ReadLine();

            try
            {
                if (int.TryParse(input, out int choice) && choice > 0 && choice <= documents.Count)
                {
                    _currentDocument = _documentManager.OpenDocument(documents[choice - 1]);
                }
                else
                {
                    _currentDocument = _documentManager.OpenDocument(input);
                }

                Console.WriteLine("\nDocument content:");
                Console.WriteLine("--------------------\n");
                Console.WriteLine(_currentDocument.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error opening document: {ex.Message}");
            }
            Console.WriteLine("\n--------------------");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
        private void DeleteDocument()
        {
            var documents = _documentManager.GetDocumentList();

            if (documents.Count == 0)
            {
                Console.WriteLine("-----> No documents found!");
                Console.Write("Press any key...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nAvailable documents:");
            Console.WriteLine("--------------------");
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine($"{i + 1}.* {Path.GetFileName(documents[i])}");
            }

            Console.Write("\nEnter file number:");
            string input = Console.ReadLine();


            if (int.TryParse(input, out int choice) && choice > 0 && choice <= documents.Count)
            {
                _documentManager.DeleteDocument(documents[choice - 1]);
            }

            Console.WriteLine("\n-----> Document was deleted.");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

    }
}
