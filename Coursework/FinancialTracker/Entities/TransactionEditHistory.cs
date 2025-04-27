
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Entities
{
    public class TransactionEditHistory
    {
        [Key]
        public int Id { get; set; }

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

        // Конструктор для EF Core
        private TransactionEditHistory() { }

        public TransactionEditHistory(int editorId, decimal oldAmount, decimal newAmount,
                                     int oldCategory, int newCategory,
                                     string oldDesc, string newDesc)
        {
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
