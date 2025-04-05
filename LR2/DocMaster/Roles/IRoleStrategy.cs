
namespace DocMaster.Roles
{
    public interface IRoleStrategy
    {
        bool CanEdit { get; }
        bool CanSave { get; }
        bool CanManageUsers { get; }
        void HandleRoleChange(UserRole newRole);

    }
}
