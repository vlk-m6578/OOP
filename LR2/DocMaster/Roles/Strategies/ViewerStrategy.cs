using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles.Strategies
{
    public class ViewerStrategy : IRoleStrategy
    {
        public bool CanEdit => false;
        public bool CanSave => false;
        public bool CanManageUsers => false;
        public void HandleRoleChange(UserRole newRole)
        {
            Console.WriteLine($"Viewer role changed to {newRole}");
        }
    }
}
