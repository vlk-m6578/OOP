using DocMaster.Roles.Observers;
using DocMaster.Roles.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles
{
    public class RoleContext : IRoleChangeObserver
    {
        private UserRole _currentRole;
        private IRoleStrategy _strategy;
        private readonly RoleChangeNotifier _notifier = new();

        public RoleContext()
        {
            _notifier.Subscribe(this);
            SetRole(UserRole.Viewer);
        }
        public void SetRole(UserRole newRole)
        {
            _currentRole = newRole;
            _strategy = CreateStrategy(newRole);
            _notifier.Notify(newRole);
        }
        private IRoleStrategy CreateStrategy(UserRole role) => role switch
        {
            UserRole.Editor => new EditorStrategy(),
            UserRole.Admin => new AdminStrategy(),
            UserRole.Viewer => new ViewerStrategy()
        };
        public void Update(UserRole newRole)
        {
            Console.WriteLine($"Role updated to {newRole}");
            SetRole(newRole);
        }
        public bool CanEditDocument => _strategy.CanEdit;
        public bool CanSaveDocument => _strategy.CanSave;
        public bool CanManageUsers => _strategy.CanManageUsers;
    }
}
