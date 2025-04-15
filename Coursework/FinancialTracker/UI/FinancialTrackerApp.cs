using FinancialTracker.Entities;
using FinancialTracker.Utilities;
using FinancialTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.UI
{
    public class FinancialTrackerApp
    {
        private Menu _menu;
        private User _currentUser;
        private readonly PasswordRecoveryService _service = new PasswordRecoveryService();
        private readonly AccountService _accountService = new AccountService();
        public FinancialTrackerApp()
        {
            _menu = new Menu();
        }
        public void Run()
        {
            bool isRun = true;

            while(isRun)
            {
                _menu.ShowStartMenu();
                var choice = InputValidator.GetIntInput(1, 4);

                switch(choice)
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
            var email = InputValidator.GetValidEmail();
            var password = InputValidator.GetValidPassword();

            var newUser = new User(username, email);
            if (newUser.Register(password))
            {
                User.Users.Add(newUser);
                Console.WriteLine(" -----> Registration successful! Auto-login...");
                _currentUser = newUser;
                ShowDashboard();
            }
        }
        private void Login() 
        {
            Console.Clear();
            Console.WriteLine("================================================================ LOGIN ==============================================================");

            Console.Write("Enter username/email: ");
            string login = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            var user = User.Users.Find(u => (u.Username == login || u.Email ==  login) && u.PasswordHash == PasswordHasher.Hash(password));

            if(user != null && user.IsActive) 
            {
                _currentUser = user;
                Console.WriteLine($"Welcome back {user.Username}!");
            }
            else
            {
                HandleError("Inactive account.");
            }
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

            var user = User.Users.FirstOrDefault(u => u.Email == email);
            if(user == null)
            {
                HandleError("Email not found in system.");
                return;
            }

            var code = _service.GenerateRecoveryPassword(email);
            Console.WriteLine($"\nGenerated recovery code: {code}");
            Console.Write("Enter the 6-digit code: ");
            var inputCode = Console.ReadLine();

            if(!_service.ValidateCode(email, inputCode))
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
            while(inDashboard)
            {
                _menu.ShowDashboardMenu();
                int choice = InputValidator.GetIntInput(1, 6);

                switch (choice)
                {
                    case 1:
                        ManageAccounts();
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:
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
                case 0:
                    break;
            }

        }
        private void CreatePersonalAccount()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE PERSONAL ACCOUNT ===");

            Console.Write("Enter account name: ");
            var accountName = Console.ReadLine();

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

            var accounts = _accountService.GetPersonalAccount(_currentUser.Id );
            if(accounts.Count == 0)
            {
                Console.WriteLine("No personal accounts found!");
                Console.Write("Press any key...");
                Console.ReadKey();
            }

            foreach(var acc in accounts )
            {
                Console.WriteLine($"ID: {acc.Id} | Name: {acc.Name} | Balance: {acc.Balance:C}");
            }

            Console.Write("Enter account Id to edit: ");
            int accountId = InputValidator.GetIntInput(1, int.MaxValue);

            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            if(account == null )
            {
                HandleError("Account not found");
                return;
            }

            Console.Write("Enter new account name: ");
            string newName = Console.ReadLine().Trim();

            if(_accountService.UpdatePersonalAccountName(accountId, newName, _currentUser.Id))
            {
                Console.WriteLine("Account updated successfully!");
            }
            else
            {
                HandleError("Failed to update account");
            }

            Console.WriteLine("No personal accounts found!");
            Console.Write("Press any key...");
        }
    }
}
