using FinancialTracker.Entities;
using FinancialTracker.Utilities;
using FinancialTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialTracker.Entities.Accounts;
using System.Security.Principal;

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

            var accounts = _accountService.GetPersonalAccounts(_currentUser.Id );
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
            }

            foreach(var a in accounts )
            {
                Console.WriteLine($"ID: {a.Id} | Name: {a.Name} | Balance: {a.Balance:C}");
            }

            Console.Write("Enter account ID to delete: ");
            int accountId = InputValidator.GetIntInput(1, int.MaxValue);

            if(_accountService.DeletePersonalAccount(accountId, _currentUser.Id))
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

            Console.Write("Enter account name: ");
            var accountName = Console.ReadLine();

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
                if(sharedAccounts.Count == 0)
                {
                    Console.WriteLine("No shared accounts found!");
                    Console.Write("Press any key...");
                    Console.ReadKey();
                    return;
                }

                foreach (var acc in sharedAccounts)
                {
                    Console.WriteLine($"ID: {acc.Id} | Name: {acc.Name} | Members: {acc.MemberUserIds.Count}");
                }

                Console.Write("Enter account ID to manage (0 to back): ");
                int accountId = InputValidator.GetIntInput(0, int.MaxValue);
                if (accountId == 0) return;

                var account = sharedAccounts.FirstOrDefault(a => a.Id == accountId) as SharedAccount;
                if(account == null)
                {
                    HandleError("Account not found");
                    continue;
                }

                ManageSharedAccount(account);
            }
        }
        private void ManageSharedAccount(SharedAccount account)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== MANAGING ACCOUNT {account.Name} ===");
                Console.WriteLine("1. Invite Member");
                Console.WriteLine("2. Remove Member");
                Console.WriteLine("3. View Members");
                Console.WriteLine("4. View History");
                Console.WriteLine("0. Back");

                int choice = InputValidator.GetIntInput(0, 4);
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
                    case 0:
                        return;
                }
            }
        }
        private void InviteMember(SharedAccount account)
        {
            Console.Write("Enter user email or username to invite: ");
            string identifier = Console.ReadLine().Trim();

            var user = User.Users.FirstOrDefault(u => u.Email == identifier || u.Username == identifier); 

            if(user == null)
            {
                HandleError("User not found");
                return;
            }

            account.AddMember(user.Id);
            account.LogHistory(_currentUser.Id, "Member Invited", $"Invited user: {user.Username}");
            Console.WriteLine($"User {user.Username} invited successfully!");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
        private void ViewSharedAccountHistory(SharedAccount account)
        {
            Console.WriteLine("\n=== ACCOUNT HISTORY ===");
            foreach (var entry in account.History)
            {
                Console.WriteLine($"[{entry.Timestamp}] {entry.Action}: {entry.Details}");
            }
            Console.ReadKey();
        }
        private void ViewOperationHistory()
        {
            Console.Clear();
            Console.WriteLine("=== VIEW OPERATION HISTORY ===");

            Console.Write("Enter account ID: ");
            int accountId = InputValidator.GetIntInput(1, int.MaxValue);

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
            Console.Write("Enter user ID to remove: ");
            int userId = InputValidator.GetIntInput(1, int.MaxValue);

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
            account.ViewMembers(message => Console.WriteLine(message));
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
