using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.UI
{
    public class Menu
    {
        public void ShowStartMenu()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Financial Tracker <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign up");
            Console.WriteLine("3. Password recovery");
            Console.WriteLine("4. Exit");
            Console.Write("Option: ");
        }
        public void ShowDashboardMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Dashboard <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Account Management");
            Console.WriteLine("2. Transaction Management");
            Console.WriteLine("3. Categories");
            Console.WriteLine("4. Budgets");
            Console.WriteLine("5. Reports");
            Console.WriteLine("6. Profile Settings"); //
            Console.WriteLine("7. Logout");
            Console.Write("Select section: ");
        }
        public void ShowAccountManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Account Management <-------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Create Personal Account");
            Console.WriteLine("2. Edit Personal Account");
            Console.WriteLine("3. Delete Personal Account");
            Console.WriteLine("4. Create Shared Account");
            Console.WriteLine("5. Manage Shared Accounts");
            Console.WriteLine("6. Edit Shared Account");
            Console.WriteLine("7. Delete Shared Account");
            Console.WriteLine("0. Back");
            Console.Write("Action: ");
        }
        public void ShowSharedAccount()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Shared Accounts <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Invite Member");
            Console.WriteLine("2. Remove Member");
            Console.WriteLine("3. View Access Rights");
            Console.WriteLine("4. View Shared History");
            Console.WriteLine("0. Back");
            Console.Write("Action: ");
        }
        public void ShowTransactionMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Transaction Management <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. New Transaction");
            Console.WriteLine("2. Edit Transaction");
            Console.WriteLine("3. Delete Transaction");
            Console.WriteLine("4. View Edit History");
            Console.WriteLine("5. Search Transaction");
            Console.WriteLine("0. Back");
            Console.Write("Action: ");
        }
        public void ShowTransactionTypeMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Transaction Type <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Income");
            Console.WriteLine("2. Expense");
            Console.Write("Type: ");
        }
        public void ShowCategoriesMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------> Categories <----------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Create Category");
            Console.WriteLine("2. Delete Category");
            Console.WriteLine("3. View all categories");
            Console.WriteLine("0. Back");
            Console.Write("Action: ");
        }
        public void ShowReportsMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Financial Reports <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Current Month Report");
            Console.WriteLine("2. Previous Month Report");
            Console.WriteLine("3. Custom Period Report");
            Console.WriteLine("4. Category Summary");
            Console.WriteLine("5. Budget Alerts");
            Console.WriteLine("0. Back");
            Console.Write("Type: ");

        }
        public void ShowProfileSettingsMenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Profile Settings <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Change Username");
            Console.WriteLine("2. Change Email");
            Console.WriteLine("3. Deactivate Account");
            Console.WriteLine("0. Back");
            Console.Write("Action: ");
        }
        public void ShowRecoverymenu()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------> Password Recovery <--------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("1. Send Recovery Code");
            Console.WriteLine("2. Enter Recovery Code");
            Console.WriteLine("0. Back");
            Console.Write("Select action: ");
        }
    }
}
