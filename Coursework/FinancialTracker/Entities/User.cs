using System;
using FinancialTracker.Interfaces;

namespace FinancialTracker.Entities
{
    public class User : IAuthService
    {
        private int id;
        private string username;
        private string email;
        private string passwordHash;
        private bool isActive;
        //List
        //List
        public User(int id, string usernamee, string email)
        {
            this.id = id;
            this.username = usernamee;
            this.email = email;
        }
        public bool Register(string password)
        {

        }


        public void ActivateAccount() => isActive = true;
        public void DeactivateAccount() => isActive = false;
    }
}
