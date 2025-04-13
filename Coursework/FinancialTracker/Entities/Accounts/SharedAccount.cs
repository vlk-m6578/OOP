using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities.Accounts
{
    public class SharedAccount : Account
    {
        public int CreatorUserId { get; }
        public List<int> MemberUserIds { get; } = new List<int>();
        public List<AccountHistoryEntry> History { get; } = new List<AccountHistoryEntry>(); 
        public SharedAccount(int id, string name, int creatorId) : base(id, name)
        {
            CreatorUserId = creatorId;
            MemberUserIds.Add(creatorId);
        }
        public void AddMember(int userId)
        {
            if (!MemberUserIds.Contains(userId))
                MemberUserIds.Add(userId);
        }
        public void LogHistory(int userId, string action, string details)
        {
            History.Add(new AccountHistoryEntry(userId, action, details));
        }
        public override void ApplyTransaction(Transaction transaction)
        {
            base.ApplyTransaction(transaction);
        }
    }
}
