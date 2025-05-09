using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Domain.Entities
{
    public class TransactionEditHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public DateTime EditedAt { get; set; }

        [Required]
        public int EditedByUserId { get; set; }

        [Required]
        public decimal OldAmount { get; set; }

        [Required]
        public decimal NewAmount { get; set; }

        [Required]
        public int OldCategoryId { get; set; }

        [Required]
        public int NewCategoryId { get; set; }

        public string OldDescription { get; set; }

        public string NewDescription { get; set; }

        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }

        public TransactionEditHistory() { }

        public TransactionEditHistory(int transactionId, int editorId, decimal oldAmount, decimal newAmount,
                                     int oldCategory, int newCategory,
                                     string oldDesc, string newDesc)
        {
            TransactionId = transactionId;
            EditedAt = DateTime.Now;
            EditedByUserId = editorId;
            OldAmount = oldAmount;
            NewAmount = newAmount;
            OldCategoryId = oldCategory;
            NewCategoryId = newCategory;
            OldDescription = oldDesc;
            NewDescription = newDesc;
        }
    }
}
