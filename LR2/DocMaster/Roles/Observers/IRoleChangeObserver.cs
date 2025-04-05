using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles.Observers
{
    public interface IRoleChangeObserver
    {
        void Update(UserRole newRole);
    }
}
