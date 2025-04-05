using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles.Strategies
{
    public class EditorStrategy : IRoleStrategy
    {
        public bool CanEdit => true;
        public bool CanSave => true;
        public bool CanManageUsers => false;

        public void HandleRoleChange(UserRole newRole)
        {
            Console.WriteLine($"Editor role changed to {newRole}");
        }
    }
}
