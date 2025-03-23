using System;
using FinancialTracker.Interfaces;
using FinancialTracker.Utilities;

namespace FinancialTracker.Entities
{
    public class User : IAuthService
    {
        private static int _lastId = 0;
        public int Id { get; }
        public string Username { get; private set; } 
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        //List
        //List
        public User(string username, string email)
        {
            Id = ++_lastId;
            Username = username;
            Email = email;
            IsActive = true;
        }
        public bool Register(string password)
        {
            PasswordHash = PasswordHasher.Hash(password);
            return true;
        }


        public void ActivateAccount() => IsActive = true;
        public void DeactivateAccount() => IsActive = false;

        public static List<User> Users { get; } = new List<User>();
    }
}
