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

            Console.WriteLine("1) Log in");
            Console.WriteLine("2) Sign up");
            Console.WriteLine("3) Sign out");
            Console.WriteLine("Choose: ");
        }
    }
}
