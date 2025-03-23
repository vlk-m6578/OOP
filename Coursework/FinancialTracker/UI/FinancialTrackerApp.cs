using FinancialTracker.Entities;
using FinancialTracker.Utilities;
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

                        break;
                    case 2:
                        Registration();
                        break;
                    case 3:

                        break;
                    case 4:
                        isRun = false;
                        break;
                }
                Console.WriteLine("\nGoodbye!");
                Console.ReadKey();
            }
            //
        }
        private void Registration()
        {
            Console.Clear();
            Console.WriteLine("================================================== REGISTRATION ==================================================");

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
    }
}
