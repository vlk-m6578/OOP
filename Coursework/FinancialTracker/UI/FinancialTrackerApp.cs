using FinancialTracker.Entities;
using FinancialTracker.Utilities;
using FinancialTracker.Services;
using FinancialTracker.Entities.Accounts;
using FinancialTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using FinancialTracker.Interfaces;

namespace FinancialTracker.UI
{
    public class FinancialTrackerApp
    {
        private Menu _menu;
        private User _currentUser;
        private readonly PasswordRecoveryService _service;
        private readonly AccountService _accountService;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;
        private readonly BudgetService _budgetService;

        private readonly AppDbContext _context;
        public FinancialTrackerApp()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Создаст БД при первом запуске
            _accountService = new AccountService(_context);
            _service = new PasswordRecoveryService(_context);
            _categoryService = new CategoryService(_context);
            _budgetService = new BudgetService(_context);
            _transactionService = new TransactionService(_context, _budgetService);
            _menu = new Menu();
        }
        public void Run()
        {
            bool isRun = true;

            while (isRun)
            {
                _menu.ShowStartMenu();
                var choice = InputValidator.GetIntInput(1, 4);

                switch (choice)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        Registration();
                        break;
                    case 3:
                        PasswordRecovery();
                        break;
                    case 4:
                        isRun = false;
                        break;
                }
            }
            Console.Write("\nGoodbye!");
            Console.ReadKey();
        }
        private void Registration()
        {
            Console.Clear();
            Console.WriteLine("========================================================= REGISTRATION ==============================================================");

            var username = InputValidator.GetValidUsername();
            if (username == "0") return;
            var email = InputValidator.GetValidEmail();
            if (email == "0") return;
            var password = InputValidator.GetValidPassword();
            if (password == "0") return;

            var newUser = new User(username, email);
            if (newUser.Register(password))
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                Console.WriteLine(" -----> Registration successful! Auto-login...");
                _currentUser = newUser;
                ShowDashboard();
            }
        }
        private void Login()
        {
            Console.Clear();
            Console.WriteLine("================================================================ LOGIN ==============================================================");

            Console.Write("Enter username/email (0 to back): ");
            string login = Console.ReadLine();
            if (login == "0") return;

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            if (password == "0") return;

            var user = _context.Users
            .FirstOrDefault(u => (u.Username == login || u.Email == login));

            if (user != null && user.IsActive)
            {
                _currentUser = user;
                CheckPendingInvitations();
                Console.WriteLine($"Welcome back {user.Username}!");
                _currentUser = user;
                Console.Write("Press any key...");
                Console.ReadKey();
                ShowDashboard();
            }
            else
            {
                HandleError("Inactive account.");
            }
        }
        private void CheckPendingInvitations()
        {
            var pendingInvites = _context.Invitations
                .Include(i => i.SharedAccount)
                .Include(i => i.InviterUser)
                .Where(i => i.InvitedUserId == _currentUser.Id && i.Status == InvitationStatus.Pending)
                .ToList();

            foreach (var invite in pendingInvites)
            {
                var sharedAccount = _context.Accounts.OfType<SharedAccount>()
                    .First(a => a.Id == invite.SharedAccountId);

                Console.WriteLine($"\nYou've been invited to shared account '{sharedAccount.Name}' by {invite.InviterUser.Username}");
                Console.Write("Accept invitation? (Y/N): ");
                var response = Console.ReadLine().Trim().ToUpper();

                if (response == "Y")
                {
                    invite.Status = InvitationStatus.Accepted;
                    var members = sharedAccount.MemberUserIdsList;
                    members.Add(_currentUser.Id);
                    sharedAccount.MemberUserIds = string.Join(",", members.Distinct());
                    sharedAccount.LogHistory(_currentUser.Id, "Member Joined", $"User {_currentUser.Username} accepted invitation");
                }
                else
                {
                    invite.Status = InvitationStatus.Declined;
                    sharedAccount.LogHistory(invite.InviterUserId, "Invitation Declined", $"User {_currentUser.Username} declined invitation");
                }
            }

            _context.SaveChanges();
        }
        private void HandleError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {message}");
            Console.ResetColor();
        }
        private void PasswordRecovery()
        {
            Console.Clear();
            Console.WriteLine("========================================================= PASSWORD RECOVERY ========================================================");

            Console.Write("Enter your email: ");
            var email = Console.ReadLine()?.Trim();

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                HandleError("Email not found in system.");
                return;
            }

            var code = _service.GenerateRecoveryPassword(email);
            Console.WriteLine($"\nGenerated recovery code: {code}");
            Console.Write("Enter the 6-digit code: ");
            var inputCode = Console.ReadLine();

            if (!_service.ValidateCode(email, inputCode))
            {
                HandleError("Invalid code.");
                return;
            }

            Console.Write("Recovery -> ");
            var newPassword = InputValidator.GetValidPassword();

            user.ResetPassword(newPassword);
            Console.WriteLine("\n -----> Password successfully reset.");
            Console.WriteLine(" -----> You can now login with your new password.");
            return;
        }
        private void ShowDashboard()
        {
            bool inDashboard = true;
            while (inDashboard)
            {
                _menu.ShowDashboardMenu();
                int choice = InputValidator.GetIntInput(1, 7);

                switch (choice)
                {
                    case 1:
                        ManageAccounts();
                        break;
                    case 2:
                        ManageTransactions();
                        break;
                    case 3:
                        ManageCategories();
                        break;
                    case 4:
                        ManageBudgets();
                        break;
                    case 5:
                        ManageReports();
                        break;
                    case 6:
                        
                        break;
                    case 7:
                        _currentUser = null;
                        Console.WriteLine("Successfully logged out.");
                        inDashboard = false;
                        break;
                }
            }
        }
        private void ManageAccounts()
        {
            _menu.ShowAccountManagementMenu();
            int choice = InputValidator.GetIntInput(0, 6);

            switch (choice)
            {
                case 1:
                    CreatePersonalAccount();
                    break;
                case 2:
                    EditPersonalAccount();
                    break;
                case 3:
                    DeletePersonalAccount();
                    break;
                case 4:
                    CreateSharedAccount();
                    break;
                case 5:
                    ManageSharedAccounts();
                    break;
                case 6:
                    //ViewOperationHistory();
                case 0:
                    break;
            }

        }
        private void CreatePersonalAccount()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE PERSONAL ACCOUNT ===");

            Console.Write("Enter account name (0 to back): ");
            var accountName = Console.ReadLine();
            if (accountName == "0") return;

            var newAccount = _accountService.CreatePersonalAccount(
                name: accountName,
                userId: _currentUser.Id
                );
            Console.WriteLine($"Account '{newAccount.Name}' created successfully!");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
        private void EditPersonalAccount()
        {
            Console.Clear();
            Console.WriteLine("=== EDIT PERSONAL ACCOUNT ===");

            var accounts = _accountService.GetPersonalAccounts(_currentUser.Id);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No personal accounts found!");
                Console.Write("Press any key...");
                Console.ReadKey();
                return;
            }

            foreach (var acc in accounts)
            {
                Console.WriteLine($"ID: {acc.Id} | Name: {acc.Name} | Balance: {acc.Balance:C}");
            }

            Console.Write("Enter account Id to edit (0 to back): ");
            int accountId = InputValidator.GetIntInput(0, int.MaxValue);
            if (accountId == 0) return;

            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                HandleError("Account not found");
                return;
            }

            Console.Write("Enter new account name: ");
            string newName = Console.ReadLine().Trim();
            if (newName == "0") return;

            if (_accountService.UpdatePersonalAccountName(accountId, newName, _currentUser.Id))
            {
                Console.WriteLine("Account updated successfully!");
                Console.Write("Press any key...");
                Console.ReadKey();
            }
            else
            {
                HandleError("Failed to update account");
            }

            Console.WriteLine("No personal accounts found!");
            Console.Write("Press any key...");
        }
        private void DeletePersonalAccount()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE PERSONAL ACCOUNT ===");

            var accounts = _accountService.GetPersonalAccounts(_currentUser.Id);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No personal accounts found!");
                Console.Write("Press any key...");
                Console.ReadKey();
                return;
            }

            foreach (var a in accounts)
            {
                Console.WriteLine($"ID: {a.Id} | Name: {a.Name} | Balance: {a.Balance:C}");
            }

            Console.Write("Enter account ID to delete (0 to back): ");
            int accountId = InputValidator.GetIntInput(1, int.MaxValue);
            if (accountId == 0) return;

            if (_accountService.DeletePersonalAccount(accountId, _currentUser.Id))
            {
                Console.WriteLine("Account deleted successfully!");
            }
            else
            {
                HandleError("Failed to delete account. Check if balance is zero");
            }
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private void CreateSharedAccount()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE SHARED ACCOUNT ===");

            Console.Write("Enter account name (0 to back): ");
            var accountName = Console.ReadLine();
            if (accountName == "0") return;

            var newAccount = _accountService.CreateSharedAccount(accountName, _currentUser.Id);
            newAccount.LogHistory(_currentUser.Id, "Account Created", $"Created by {_currentUser.Username}");
            Console.WriteLine($"Shared account '{newAccount.Name}' created! ID: {newAccount.Id}");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
        private void ManageSharedAccounts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MANAGE SHARED ACCOUNTS ===");

                var sharedAccounts = _accountService.GetSharedAccountsForUser(_currentUser.Id);
                if (sharedAccounts.Count == 0)
                {
                    Console.WriteLine("No shared accounts found!");
                    Console.Write("Press any key...");
                    Console.ReadKey();
                    return;
                }

                foreach (var acc in sharedAccounts)
                {
                    Console.WriteLine($"ID: {acc.Id} | Name: {acc.Name} | Members: {acc.MemberUserIdsList.Count}");
                }

                Console.Write("Enter account ID to manage (0 to back): ");
                int accountId = InputValidator.GetIntInput(0, int.MaxValue);
                if (accountId == 0) return;

                var account = sharedAccounts.FirstOrDefault(a => a.Id == accountId) as SharedAccount;
                if (account == null)
                {
                    HandleError("Account not found");
                    continue;
                }

                ManageSharedAccount(account);
            }
        }
        private void ManageSharedAccount(SharedAccount account)
        {
            bool isCreator = _currentUser.Id == account.CreatorUserId;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== MANAGING ACCOUNT {account.Name} ===");

                if (isCreator)
                {
                    Console.WriteLine("1. Invite Member");
                    Console.WriteLine("2. Remove Member");
                    Console.WriteLine("3. View Members");
                    Console.WriteLine("4. View History");
                    Console.WriteLine("5. View Pending Invitations");
                    Console.WriteLine("0. Back");
                    Console.Write("Action: ");

                    int choice = InputValidator.GetIntInput(0, 5);
                    switch (choice)
                    {
                        case 1:
                            InviteMember(account);
                            break;
                        case 2:
                            RemoveMember(account);
                            break;
                        case 3:
                            ViewMembers(account);
                            break;
                        case 4:
                            ViewSharedAccountHistory(account);
                            break;
                        case 5:
                            ViewPendingInvitations(account);
                            break;
                        case 0:
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("1. View Members");
                    Console.WriteLine("2. View History");
                    Console.WriteLine("3. View Pending Invitations");
                    Console.WriteLine("0. Back");
                    Console.Write("Action: ");

                    int choice = InputValidator.GetIntInput(0, 3);
                    switch (choice)
                    {
                        case 1:
                            ViewMembers(account);
                            break;
                        case 2:
                            ViewSharedAccountHistory(account);
                            break;
                        case 3:
                            ViewPendingInvitations(account);
                            break;
                        case 0:
                            return;
                    }
                }
            }
        }
        private void InviteMember(SharedAccount account)
        {
            if (_currentUser.Id != account.CreatorUserId)
            {
                HandleError("Only creator can invite members");
                return;
            }

            Console.Write("Enter user email or username to invite (0 to back): ");
            string identifier = Console.ReadLine().Trim();
            if (identifier == "0") return;

            var user = _context.Users.FirstOrDefault(u => u.Email == identifier || u.Username == identifier);

            if (user == null)
            {
                HandleError("User not found");
                return;
            }

            if (_context.Invitations.Any(i => i.SharedAccountId == account.Id
                                            && i.InvitedUserId == user.Id
                                            && i.Status == InvitationStatus.Pending))
            {
                HandleError("User already has a pending invitation");
                return;
            }

            var invitation = new Invitation
            {
                SharedAccountId = account.Id,
                InvitedUserId = user.Id,
                InviterUserId = _currentUser.Id
            };

            _context.Invitations.Add(invitation);
            account.LogHistory(_currentUser.Id, "Invitation Sent", $"Invited user: {user.Username}");
            _context.SaveChanges();

            Console.WriteLine("Invitation sent successfully!");
            Console.ReadKey();
        }
        private void ViewSharedAccountHistory(SharedAccount account)
        {
            Console.WriteLine("\n=== ACCOUNT HISTORY ===");
            foreach (var entry in account.History)
            {
                Console.WriteLine($"[{entry.Timestamp}] {entry.Action}: {entry.Details}");
            }

            // Показать отклоненные приглашения
            var declinedInvites = _context.Invitations
                .Where(i => i.SharedAccountId == account.Id && i.Status == InvitationStatus.Declined)
                .ToList();

            foreach (var invite in declinedInvites)
            {
                var user = _context.Users.Find(invite.InvitedUserId);
                Console.WriteLine($"[{invite.Timestamp}] Invitation Declined: {user?.Username ?? "Unknown user"}");
            }

            Console.ReadKey();
        }
        private void ViewOperationHistory()
        {
            Console.Clear();
            Console.WriteLine("=== VIEW OPERATION HISTORY ===");

            Console.Write("Enter account ID (0 to back): ");
            int accountId = InputValidator.GetIntInput(1, int.MaxValue);
            if (accountId == 0) return;

            var account = _accountService.GetAccountById(accountId);
            if (account == null)
            {
                HandleError("Account not found");
                return;
            }

            if (account is SharedAccount sharedAccount)
            {
                ViewSharedAccountHistory(sharedAccount);
            }
            else
            {
                Console.WriteLine("Personal accounts history not implemented yet");
                Console.Write("Press any key...");
                Console.ReadKey();
            }
        }
        private void RemoveMember(SharedAccount account)
        {
            if (_currentUser.Id != account.CreatorUserId)
            {
                HandleError("Only creator can remove members");
                return;
            }

            Console.Write("Enter user ID to remove (0 to back): ");
            int userId = InputValidator.GetIntInput(0, int.MaxValue);
            if (userId == 0) return;

            if (account.RemoveMember(userId, _currentUser.Id))
            {
                Console.WriteLine($"User {userId} removed successfully!");
            }
            else
            {
                HandleError("Failed to remove member. Check permissions or user existence.");
            }
            Console.ReadKey();
        }
        private void ViewMembers(SharedAccount account)
        {
            Console.WriteLine("\n=== ACCOUNT MEMBERS ===");
            account.ViewMembers(
                message => Console.WriteLine(message),
                userId =>
                {
                    var user = _context.Users.Find(userId);
                    return user != null ? user.Username : "Unknown User";
                }
            );
            Console.ReadKey();
        }
        private void ViewPendingInvitations(SharedAccount account)
        {
            if (_currentUser.Id != account.CreatorUserId)
            {
                var userInvites = _context.Invitations
                .Where(i => i.SharedAccountId == account.Id && i.InvitedUserId == _currentUser.Id)
                .ToList();

                Console.WriteLine("\n=== PENDING INVITATIONS ===");
                foreach (var invite in userInvites)
                {
                    Console.WriteLine($"ID: {invite.Id} | User: {invite.InvitedUser.Username} | Sent: {invite.Timestamp}");

                }
                Console.ReadKey();
            }
            else
            {
                var pending = _context.Invitations
                .Where(i => i.SharedAccountId == account.Id && i.Status == InvitationStatus.Pending)
                .Include(i => i.InvitedUser)
                .ToList();

                Console.WriteLine("\n=== PENDING INVITATIONS ===");
                foreach (var invite in pending)
                {
                    Console.WriteLine($"ID: {invite.Id} | User: {invite.InvitedUser.Username} | Sent: {invite.Timestamp}");
                }
                Console.ReadKey();
            }
        }

        ///////////////////////////////////////////////////// CATEGORIES MANAGEMENT ////////////////////////////////////////////////

        private void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                _menu.ShowCategoriesMenu();

                var choice = InputValidator.GetIntInput(0, 3);

                switch (choice)
                {
                    case 1:
                        CreateUserCategory();
                        break;
                    case 2:
                        DeleteUserCategory();
                        break;
                    case 3:
                        ViewAllCategories();
                        break;
                    case 0:
                        return;
                }
            }
        }
        private void CreateUserCategory()
        {
            Console.Write("Enter the name of the new category (0 to back): ");
            var name = Console.ReadLine().Trim();
            if (name == "0") return;

            try
            {
                var category = _categoryService.CreateUserCategory(name);
                Console.WriteLine($"Category '{category.Name}' has been created!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DeleteUserCategory()
        {
            var userCategories = _categoryService.GetUserCategories();

            Console.WriteLine("Available categories to delete:");
            foreach (var category in userCategories)
            {
                Console.WriteLine($"{category.Id}. {category.Name}");
            }

            Console.Write("Enter the category ID to delete (0 to back): ");
            var categoryId = InputValidator.GetIntInput(0, int.MaxValue);

            if (categoryId == 0) return;

            try
            {
                _categoryService.DeleteCategory(categoryId);
                Console.WriteLine("Category has been deleted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewAllCategories()
        {
            var categories = _categoryService.GetAllCategories();

            Console.WriteLine("\n=== ALL CATEGORIES ===");
            Console.WriteLine("System categories:");
            foreach (var category in categories.Where(c => c.IsSystemCategory))
            {
                Console.WriteLine($"- {category.Name}");
            }

            Console.WriteLine("\nCustom Categories:");
            foreach (var category in categories.Where(c => !c.IsSystemCategory))
            {
                Console.WriteLine($"- {category.Name}");
            }

            Console.ReadKey();
        }

        ///////////////////////////////////////////////////// TRANSACTIONS MANAGEMENT ////////////////////////////////////////////////

        private void ManageTransactions()
        {
            while (true)
            {
                _menu.ShowTransactionMenu();
                int choice = InputValidator.GetIntInput(0, 5);

                switch (choice)
                {
                    case 1:
                        AddTransaction();
                        break;
                    case 2:
                        EditTransaction();
                        break;
                    case 3:
                        DeleteTransaction();
                        break;
                    case 4:
                        ViewEditHistory();
                        break;
                    case 5:
                        SearchTransactions();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void AddTransaction()
        {
            try
            {
                // Проверка наличия счетов
                if (!_accountService.UserHasAccounts(_currentUser.Id))
                {
                    Console.WriteLine("You don't have any accounts. Create an account first.");
                    Console.ReadKey();
                    return;
                }

                // Получаем все счета пользователя
                var accounts = _accountService.GetPersonalAccounts(_currentUser.Id)
                    .Cast<Account>()
                    .Concat(_accountService.GetSharedAccountsForUser(_currentUser.Id))
                    .ToList();


                // Выводим список счетов с типами
                Console.WriteLine("Available accounts:");
                foreach (var acc in accounts)
                {
                    var type = acc is SharedAccount ? "Shared" : "Personal";
                    Console.WriteLine($"{acc.Id}. {acc.Name} ({type}) - {acc.Balance}");
                }

                Console.Write("Select an account: ");
                int accountId = InputValidator.GetIntInput(0, int.MaxValue);
                if (accountId == 0) return;

                // Выбор типа
                Console.WriteLine("1. Income\n2. Expense");
                var typeT = InputValidator.GetIntInput(1, 2) == 1
                    ? TransactionType.Income
                    : TransactionType.Expense;

                // Выбор категории
                var categories = _categoryService.GetAllCategories()
                    .OrderBy(c => c.Id)
                    .ToList();
                Console.WriteLine("Available categories:");
                foreach (var cat in categories)
                {
                    Console.WriteLine($"{cat.Id}. {cat.Name}");
                }
                Console.Write("Select a category: ");
                int categoryId = InputValidator.GetIntInput(1, int.MaxValue);

                // Ввод суммы
                Console.Write("Sum: ");
                decimal amount = InputValidator.GetDecimalInput("", 0.01m, 1000000m);

                // Описание
                Console.Write("Description: ");
                string desc = Console.ReadLine();

                _transactionService.AddTransaction(amount, categoryId, accountId,
                    _currentUser.Id, desc, typeT);

                Console.WriteLine("Transaction added!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void EditTransaction()
        {
            try
            {
                // Проверка наличия счетов
                if (!_accountService.UserHasAccounts(_currentUser.Id))
                {
                    Console.WriteLine("You don't have any accounts. Create an account first.");
                    Console.ReadKey();
                    return;
                }

                using var context = new AppDbContext();
                var transactions = _context.Transactions
                    .AsNoTracking()
                    .Include(t => t.Category)
                    .Where(t => t.CreatedByUserId == _currentUser.Id && !t.IsDeleted)
                    .ToList();

                Console.WriteLine("\nYour transactions:");
                foreach (var t in transactions)
                {
                    Console.WriteLine($"{t.Id}. {t.Date:dd.MM.yyyy} | " +
                $"{(t.Type == TransactionType.Income ? "+" : "-")}{Math.Abs(t.Amount)} | " +
                $"{t.Category?.Name ?? "Without a category"} | " +
                $"{t.Description}");
                }

                Console.Write("\nSelect the transaction ID (0 to back): ");
                int transId = InputValidator.GetIntInput(0, int.MaxValue);
                if (transId == 0) return;

                // Ввод новых данных
                Console.Write("New sum: ");
                decimal amount = InputValidator.GetDecimalInput("", 0.01m, 1000000m);

                // Выбор категории
                Console.Write("New ID category: ");
                int newCategoryId = SelectCategory();

                Console.Write("New description: ");
                string desc = Console.ReadLine();

                // Используем новый контекст для операции
                var transactionService = new TransactionService(new AppDbContext(), new BudgetService(new AppDbContext()));
                transactionService.UpdateTransaction(transId, amount, newCategoryId, desc, _currentUser.Id);

                Console.WriteLine("The transaction has been updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error details: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Database error: {ex.InnerException.Message}");
            }
            Console.ReadKey();
        }
        private int SelectCategory()
        {
            var categories = _categoryService.GetAllCategories()
                .OrderBy(c => c.Id)
                .ToList();

            Console.WriteLine("\nAvailable categories:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.Id}. {cat.Name}");
            }

            while (true)
            {
                Console.Write("Select the category ID: ");
                if (int.TryParse(Console.ReadLine(), out int id) && categories.Any(c => c.Id == id))
                    return id;

                Console.WriteLine("Invalid category ID! Try again.");
            }
        }
        private void DeleteTransaction()
        {
            try
            {
                // Проверка наличия счетов
                if (!_accountService.UserHasAccounts(_currentUser.Id))
                {
                    Console.WriteLine("You don't have any accounts. Create an account first.");
                    Console.ReadKey();
                    return;
                }

                var transactions = _transactionService.GetTransactionsByUser(_currentUser.Id)
                    .Where(t => !t.IsDeleted)
                    .ToList();

                Console.WriteLine("Your transactions:");
                foreach (var t in transactions)
                {
                    Console.WriteLine($"{t.Id}. {t.Date:d} | {t.Amount} | {t.Category.Name}");
                }

                Console.Write("Enter transaction ID (0 to cancel): ");
                int transId = InputValidator.GetIntInput(0, int.MaxValue);
                if (transId == 0) return;

                _transactionService.DeleteTransaction(transId, _currentUser.Id);
                Console.WriteLine("Transaction deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewEditHistory()
        {
            try
            {
                // Проверка наличия счетов
                if (!_accountService.UserHasAccounts(_currentUser.Id))
                {
                    Console.WriteLine("You don't have any accounts. Create an account first.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nYour transactions:");
                var transactions = _transactionService.GetTransactionsByUser(_currentUser.Id);

                foreach (var t in transactions)
                {
                    Console.WriteLine($"{t.Id}. {t.Date:dd.MM.yyyy} | " +
                        $"{(t.Type == TransactionType.Income ? "+" : "-")}{Math.Abs(t.Amount)} | " +
                        $"{GetCategoryName(t.CategoryId)} | " +
                        $"{t.Description}");
                }

                Console.Write("Enter transaction ID: ");
                int transactionId = InputValidator.GetIntInput(1, int.MaxValue);

                using (var context = new AppDbContext())
                {
                    var history = context.TransactionEditHistories
                        .Where(h => h.TransactionId == transactionId)
                        .OrderByDescending(h => h.EditedAt)
                        .ToList();

                    Console.WriteLine("\n=== EDIT HISTORY ===");
                    foreach (var entry in history) 
                    {
                        var transaction = _context.Transactions
                            .Include(t => t.Account)
                            .First(t => t.Id == entry.TransactionId);

                        Console.WriteLine($"Account: {transaction.Account.Name} ({transaction.Account.GetType().Name})");
                        Console.WriteLine($"[{entry.EditedAt:dd.MM.yyyy HH:mm}] Edited by: {GetUserName(entry.EditedByUserId)}");
                        Console.WriteLine($"[{entry.EditedAt:dd.MM.yyyy HH:mm}] Edited by user {_currentUser.Username}:");
                        Console.WriteLine($"Amount: {entry.OldAmount} -> {entry.NewAmount}");
                        Console.WriteLine($"Category ID: {GetCategoryName(entry.OldCategoryId)} -> {GetCategoryName(entry.NewCategoryId)}");
                        Console.WriteLine($"Description: '{entry.OldDescription}' -> '{entry.NewDescription}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
        private string GetCategoryName(int categoryId)
        {
            using var context = new AppDbContext();
            return context.Categories
                .FirstOrDefault(c => c.Id == categoryId)?
                .Name ?? "Unknown category";
        }

        private void SearchTransactions()
        {
            try
            {
                // Проверка наличия счетов
                if (!_accountService.UserHasAccounts(_currentUser.Id))
                {
                    Console.WriteLine("You don't have any accounts. Create an account first.");
                    Console.ReadKey();
                    return;
                }

                // Базовый запрос только для доступных счетов
                var userAccounts = _accountService.GetPersonalAccounts(_currentUser.Id)
                    .Cast<Account>()
                    .Concat(_accountService.GetSharedAccountsForUser(_currentUser.Id))
                    .Select(a => a.Id)
                    .ToList();

                var query = _context.Transactions
                    .Include(t => t.Category)
                    .Include(t => t.Account)
                    .Where(t => userAccounts.Contains(t.AccountId)) // Фильтр по доступным счетам
                    .AsQueryable();

                // Фильтр по дате начала
                Console.Write("Search FROM date (yyyy-MM-dd, empty - skip): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    query = query.Where(t => t.Date >= startDate.Date);
                }

                // Фильтр по дате окончания
                Console.Write("Search TO date (yyyy-MM-dd, empty - skip): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    query = query.Where(t => t.Date <= endDate.Date.AddDays(1));
                }

                // Фильтр по категории
                var categories = _categoryService.GetAllCategories();
                Console.Write("Category ID (0 - all): ");
                if (int.TryParse(Console.ReadLine(), out int categoryId) && categoryId > 0)
                {
                    if (categories.Any(c => c.Id == categoryId))
                    {
                        query = query.Where(t => t.CategoryId == categoryId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid category ID, no filter applied.");
                    }
                }

                // Фильтр по минимальной сумме (без Math.Abs)
                Console.Write("Min sum (empty - skip): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal minAmount))
                {
                    query = query.Where(t =>
                        t.Type == TransactionType.Income && t.Amount >= minAmount ||
                        t.Type == TransactionType.Expense && t.Amount <= -minAmount);
                }

                // Фильтр по максимальной сумме (без Math.Abs)
                Console.Write("Max sum (empty - skip): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal maxAmount))
                {
                    query = query.Where(t =>
                        t.Type == TransactionType.Income && t.Amount <= maxAmount ||
                        t.Type == TransactionType.Expense && t.Amount >= -maxAmount);
                }

                // Фильтр по типу транзакции
                Console.Write("Type (1-Income, 2-Expense, 0-All): ");
                var typeChoice = InputValidator.GetIntInput(0, 2);
                if (typeChoice != 0)
                {
                    var selectedType = (TransactionType)(typeChoice - 1);
                    query = query.Where(t => t.Type == selectedType);
                }

                // Выполнение запроса и преобразование в список
                var results = query
                    .OrderByDescending(t => t.Date)
                    .ToList();

                // Вывод результатов
                Console.WriteLine("\n=== SEARCH RESULTS ===");
                foreach (var t in results)
                {
                    var accountType = t.Account switch
                    {
                        PersonalAccount _ => "Personal",
                        SharedAccount _ => "Shared",
                        _ => "Unknown"
                    };

                    var categoryName = _context.Categories
                        .FirstOrDefault(c => c.Id == t.CategoryId)?.Name ?? "Without a category";

                    Console.WriteLine($"[{accountType}] {t.Account.Name}");
                    Console.WriteLine($"{t.Id}. {t.Date:dd.MM.yyyy} | " +
                        $"{(t.Type == TransactionType.Income ? "+" : "-")}{Math.Abs(t.Amount)} | " +
                        $"{categoryName} | " +
                        $"{t.Description}");
                    Console.WriteLine($"Created by: {GetUserName(t.CreatedByUserId)}\n");
                }

                Console.WriteLine($"\nTransactions found: {results.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}");
            }
            finally
            {
                Console.ReadKey();
            }
        }
        private string GetUserName(int userId)
        {
            var user = _context.Users.Find(userId);
            return user?.Username ?? "Unknown User";
        }

        ////////////////////////////////////////////////////////
        ///
        private void ManageBudgets()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BUDGET MANAGEMENT ===");
                Console.WriteLine("1. Set Budget Limit");
                Console.WriteLine("2. View Current Budgets");
                Console.WriteLine("3. View Notifications");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                var choice = InputValidator.GetIntInput(0, 3);
                switch (choice)
                {
                    case 1:
                        SetBudgetLimit();
                        break;
                    case 2:
                        ViewCurrentBudgets();
                        break;
                    case 3:
                        ViewNotifications();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void SetBudgetLimit()
        {
            var categories = _categoryService.GetAllCategories();

            Console.WriteLine("Available categories:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.Id}. {cat.Name}");
            }

            Console.Write("Select category ID: ");
            int categoryId = InputValidator.GetIntInput(1, int.MaxValue);

            Console.Write("Enter monthly limit: ");
            decimal limit = InputValidator.GetDecimalInput("", 0.01m, 1000000m);

            _budgetService.SetBudgetLimit(_currentUser.Id, categoryId, limit);
            Console.WriteLine("Budget limit updated!");
            Console.ReadKey();
        }

        private void ViewCurrentBudgets()
        {
            var budgets = _budgetService.GetCurrentBudgets(_currentUser.Id);

            Console.WriteLine("\n=== CURRENT BUDGETS ===");
            foreach (var budget in budgets)
            {
                var category = _categoryService.GetCategory(budget.CategoryId);
                Console.WriteLine($"{category.Name}:");
                Console.WriteLine($"  Limit: {budget.Limit:C}");
                Console.WriteLine($"  Spent: {budget.CurrentSpending:C}");
                Console.WriteLine($"  Progress: {budget.CurrentSpending / budget.Limit * 100}%");
            }
            Console.ReadKey();
        }

        private void ViewNotifications()
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == _currentUser.Id && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            Console.WriteLine("\n=== NOTIFICATIONS ===");
            foreach (var n in notifications)
            {
                Console.WriteLine($"[{n.CreatedAt:g}] {n.Message}");
                n.IsRead = true;
            }
            _context.SaveChanges();
            Console.ReadKey();
        }
        ///////////////////////////////////////////
        ///
        private void ManageReports()
        {
            var reportService = new ReportService(_context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ОТЧЕТАМИ ===");
                Console.WriteLine("1. Отчет за текущий месяц");
                Console.WriteLine("2. Отчет за предыдущий месяц");
                Console.WriteLine("3. Отчет за произвольный период");
                Console.WriteLine("4. Сохранить последний отчет в файл");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var choice = InputValidator.GetIntInput(0, 4);

                try
                {
                    switch (choice)
                    {
                        case 1: // Текущий месяц
                            var currentReport = reportService.GenerateMonthlyReport(
                                _currentUser.Id, DateTime.Now);
                            DisplayReport(currentReport);
                            break;

                        case 2: // Предыдущий месяц
                            var prevMonthReport = reportService.GenerateMonthlyReport(
                                _currentUser.Id, DateTime.Now.AddMonths(-1));
                            DisplayReport(prevMonthReport);
                            break;

                        case 3: // Произвольный период
                            var start = InputValidator.GetDateInput("Начальная дата (yyyy-MM-dd): ");
                            var end = InputValidator.GetDateInput("Конечная дата (yyyy-MM-dd): ");
                            var customReport = reportService.GenerateCustomReport(
                                _currentUser.Id, start, end);
                            DisplayReport(customReport);
                            break;

                        case 4:
                            reportService.SaveLastReportToFile();
                            Console.WriteLine("\nОтчет успешно сохранен в папку 'reports'");
                            Console.ReadKey();
                            break;

                        case 0:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ReadKey();
                }
            }
        }

        private void DisplayReport(Report report)
        {
            Console.Clear();
            Console.WriteLine(report.GetFormattedReport());
            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
    }
}
