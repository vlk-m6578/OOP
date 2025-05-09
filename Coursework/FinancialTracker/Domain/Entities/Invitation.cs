using FinancialTracker.Domain.Entities.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Domain.Entities
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("SharedAccount")]
        public int SharedAccountId { get; set; }

        [ForeignKey("InvitedUser")]
        public int InvitedUserId { get; set; }

        [ForeignKey("InviterUser")]
        public int InviterUserId { get; set; }


        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public virtual SharedAccount SharedAccount { get; set; }
        public virtual User InvitedUser { get; set; }
        public virtual User InviterUser { get; set; }
    }
}