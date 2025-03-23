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
                var choice = InputValidator.GetIntInput(1, 3);

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
            Console.WriteLine("\nGoodbye!");
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
                //ShowDashboard();
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
    }
}
