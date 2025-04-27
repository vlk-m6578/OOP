using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities.Accounts
{
    public class SharedAccount : Account
    {
        public int CreatorUserId { get; set; }
        public List<int> MemberUserIds { get; set; } = new List<int>();
        public int SharedAccountId { get; set; } // Для связи один-ко-многим
        public List<AccountHistoryEntry> History { get; set; } = new List<AccountHistoryEntry>();

        private SharedAccount() { }

        public SharedAccount(string name, int creatorId) : base(name)
        {
            CreatorUserId = creatorId;
            MemberUserIds.Add(creatorId);
        }
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
        public bool RemoveMember(int userId, int removerUserId)
        {
            if(userId == CreatorUserId)
            {
                return false;
            }

            if(removerUserId != CreatorUserId)
            {
                return false;
            }

            bool removed = MemberUserIds.Remove(userId);
            if(removed)
            {
                LogHistory(removerUserId, "Member Removed", $"Removed user ID: {userId}");
            }
            return removed;
        }
        public void ViewMembers(Action<string> outputHandler)
        {
            outputHandler($"Creator: User ID: {CreatorUserId}");
            outputHandler("Members:");
            foreach (int memberId in MemberUserIds.Where(id => id != CreatorUserId))
            {
                outputHandler($"- User ID: {memberId}");
            }
        }
    }
}
