using FinancialTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities.Accounts
{
    public class SharedAccount : Account
    {
        public int CreatorUserId { get; set; }
        [Required]
        public string MemberUserIds { get; set; }  // Храним ID через запятую

        [NotMapped]
        public List<int> MemberUserIdsList
        {
            get => MemberUserIds.Split(',').Select(int.Parse).ToList();
            set => MemberUserIds = string.Join(",", value);
        }
        public int SharedAccountId { get; set; } // Для связи один-ко-многим
        private readonly AppDbContext _context;
        public List<AccountHistoryEntry> History { get; set; } = new List<AccountHistoryEntry>();

        private SharedAccount() { }

        public SharedAccount(string name, int creatorId) : base(name)
        {
            CreatorUserId = creatorId;
            MemberUserIds = creatorId.ToString();

            History = new List<AccountHistoryEntry>();
        }
        public bool IsCreator(int userId) => CreatorUserId == userId;
        private void AddMember(int userId)
        {
            if (!MemberUserIdsList.Contains(userId))
                MemberUserIdsList.Add(userId);
        }
        public void InviteMember(int inviterId, int invitedUserId)
        {
            LogHistory(inviterId, "Member Invited", $"Invited user: {invitedUserId}");
        }
        public void LogHistory(int userId, string action, string details)
        {
            History.Add(new AccountHistoryEntry
            {
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow,
                SharedAccountId = this.Id
            });
            //_context.SaveChanges();
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

            bool removed = MemberUserIdsList.Remove(userId);
            if(removed)
            {
                LogHistory(removerUserId, "Member Removed", $"Removed user ID: {userId}");
            }
            return removed;
        }
        public void ViewMembers(Action<string> outputHandler, Func<int, string> getUserName)
        {
            outputHandler($"Creator: {getUserName(CreatorUserId)}");
            outputHandler("Members:");

            // Используем MemberUserIdsList вместо MemberUserIds и убираем дубликаты
            var uniqueMembers = MemberUserIdsList
                .Where(id => id != CreatorUserId)
                .Distinct()
                .ToList();

            foreach (int memberId in uniqueMembers)
            {
                outputHandler($"- {getUserName(memberId)} (ID: {memberId})");
            }
        }

        public bool CanEditTransaction(int userId)
        {
            return MemberUserIdsList.Contains(userId);
        }
    }
}
