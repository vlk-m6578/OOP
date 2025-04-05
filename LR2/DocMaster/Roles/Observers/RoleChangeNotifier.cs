using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Roles.Observers
{
    public class RoleChangeNotifier
    {
        private readonly List<IRoleChangeObserver> _observers = new();

        public void Subscribe(IRoleChangeObserver observer)
        {
            _observers.Add(observer);
        }
        public void Unsubscribe(IRoleChangeObserver observer)
        {
            _observers.Remove(observer);
        }
        public void Notify(UserRole newRole)
        {
            foreach(var observer in _observers)
            {
                observer.Update(newRole);
            }
        }

    }
}
