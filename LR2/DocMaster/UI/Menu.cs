using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.UI
{
    public class Menu
    {
        public void ShowMainMenu(UserRole role)
        {
            Console.Clear();
            Console.WriteLine("=== DocMaster ===");
            Console.WriteLine("1. Create document" + (role < UserRole.Editor ? " [X]" : ""));
            Console.WriteLine("2. Open document");
            Console.WriteLine("3. View document");

            if (role >= UserRole.Editor)
            {
                Console.WriteLine("4. Edit document");
                Console.WriteLine("5. Save document");
                Console.WriteLine("6. Text formatting");
            }

            if (role == UserRole.Admin)
            {
                Console.WriteLine("7. User management");
                Console.WriteLine("8. System settings");
            }

            Console.WriteLine("9. Exit");
            Console.Write("Choice: ");
        }
        public void ShowAdminMenu()
        {
            Console.WriteLine("\n=== Admin Dashboard ===");
            Console.WriteLine("1. Change user rights");
            Console.WriteLine("2. View all users");
            Console.WriteLine("3. Block document");
            Console.Write("Choice: ");
        }
    }
}
