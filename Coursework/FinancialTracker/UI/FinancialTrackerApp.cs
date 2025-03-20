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

                        break;
                    case 3:

                        break;
                }
            }
            //
        }
    }
}
