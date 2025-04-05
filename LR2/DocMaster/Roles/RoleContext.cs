using DocMaster.Roles.Observers;
using DocMaster.Roles.Strategies;

namespace DocMaster.Roles
{
    public class RoleContext : IRoleChangeObserver
    {
        private IRoleStrategy _strategy;
        private readonly RoleChangeNotifier _notifier = new();

        public UserRole CurrentRole { get; private set; }

        public RoleContext()
        {
            _notifier.Subscribe(this);
            SetRole(UserRole.Viewer);
        }
        public void SetRole(UserRole newRole)
        {
            CurrentRole = newRole;
            _strategy = CreateStrategy(newRole);
            _notifier.Notify(newRole);
        }
        private IRoleStrategy CreateStrategy(UserRole role) => role switch
        {
            UserRole.Editor => new EditorStrategy(),
            UserRole.Admin => new AdminStrategy(),
            _ => new ViewerStrategy()
        };
        public void Update(UserRole newRole)
        {
            //Console.WriteLine($"Role updated to {newRole}");
            //SetRole(newRole);
        }
        public bool CanEditDocument => _strategy.CanEdit;
        public bool CanSaveDocument => _strategy.CanSave;
        public bool CanManageUsers => _strategy.CanManageUsers;
    }
}
