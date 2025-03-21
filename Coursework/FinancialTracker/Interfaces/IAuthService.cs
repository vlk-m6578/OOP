using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Interfaces
{
    public interface IAuthService
    {
        public bool Register(string password);
        //public void Login();
        //public void ResetPassword();
        public void ActivateAccount();
        public void DeactivateAccount();
    }
}
