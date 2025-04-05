using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles.Strategies
{
    public class AdminStrategy : IRoleStrategy
    {
        public bool CanEdit => true;
        public bool CanSave => true;
        public bool CanManageUsers => true;

        public void HandleRoleChange(UserRole newRole)
        {
            Console.WriteLine($"Admin role changed to {newRole}");
        }

    }
}
