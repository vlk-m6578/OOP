using DocMaster.Roles.Observers;

namespace DocMaster.Models
{
    public class User : IRoleChangeObserver
    {
        public string Username { get; }
        public UserRole CurrentRole { get; private set; }

        public User(string username, UserRole initialRole)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            CurrentRole = initialRole;
        }
        public void SetRole(UserRole newRole, RoleChangeNotifier notifier)
        {
            var oldRole = CurrentRole;
            CurrentRole = newRole;
            notifier.Notify(this, newRole);
        }
        public void OnRoleChanged(User changedUser, UserRole newRole)
        {
            //if (changedUser.Username == this.Username)
            //{
            //    Console.WriteLine($"[SYSTEM] Your role was changed to {newRole}");
            //}
            if (this.CurrentRole == UserRole.Admin)
            {
                Console.WriteLine($"[ADMIN NOTIFICATION] User {changedUser.Username} role changed to {newRole}");
            }
        }
    }
}
