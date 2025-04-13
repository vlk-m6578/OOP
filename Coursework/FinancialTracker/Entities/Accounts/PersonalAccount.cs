using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities.Accounts
{
    public class PersonalAccount : Account
    {
        public int UserId { get; }
        public PersonalAccount(int id, string name, int userId) : base(id, name)
        {
            UserId = userId;
        }

        public override void ApplyTransaction(Transaction transaction)
        {

        }
    }
}
