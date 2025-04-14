
namespace FinancialTracker.Entities
{
    public class TransactionEditHistory
    {
        public DateTime EditedAt { get; }
        public int EditedByUserId { get; }
        public decimal OldAmount { get; }
        public decimal NewAmount { get; }
        public int OldCategoryId { get; }
        public int NewCategoryId { get; }
        public string OldDescription { get; }
        public string NewDescription { get; }
        public TransactionEditHistory(int editorId, decimal oldAmount, decimal newAmount, int oldCategory, int newCategory, string oldDesc, string newDesc)
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
