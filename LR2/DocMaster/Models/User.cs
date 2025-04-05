using DocMaster.Roles.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models
{
    public class User : IRoleChangeObserver
    {
        public string Username { get; set; }
        public UserRole CurrentRole { get; private set; }

        public User(string username, UserRole initialRole)
        {
            Username = username;
            SetRole(initialRole);
        }
        public void SetRole(UserRole newRole)
        {
            CurrentRole = newRole;
            Console.WriteLine($"{Username}'s role changed to {newRole}");
        }
        public void Update(UserRole newRole)
        {
            Console.WriteLine($"[Notification] {Username}: Your role has been changed to {newRole}");
        }
    }
}
