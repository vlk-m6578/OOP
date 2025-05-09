using FinancialTracker.Domain.Entities.Accounts;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entities
{
    public class AccountHistoryEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Action { get; set; }

        public string Details { get; set; }

        public int SharedAccountId { get; set; }
        public SharedAccount SharedAccount { get; set; }

        public AccountHistoryEntry() { }

        public AccountHistoryEntry(int userId, string action, string details)
        {
            Timestamp = DateTime.Now;
            UserId = userId;
            Action = action;
            Details = details;
        }

    }
}
