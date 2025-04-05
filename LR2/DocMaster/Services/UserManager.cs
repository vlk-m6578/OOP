using DocMaster.Models;

namespace DocMaster.Services
{
    public class UserManager
    {
        private readonly List<User> _users = new();
        public void AddUser(User user) => _users.Add(user);
        public void ChangeUserRole(string username, UserRole newRole)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            user?.SetRole(newRole);
        }
        public IEnumerable<User> GetAllUsers() => _users.AsReadOnly();
        public User GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
