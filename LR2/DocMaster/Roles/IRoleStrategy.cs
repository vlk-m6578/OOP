using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
