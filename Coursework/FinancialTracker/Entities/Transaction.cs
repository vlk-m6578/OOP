
namespace FinancialTracker.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int CreatedByUserId { get; set; }
        public string Description {  get; set; }
        public List<TransactionEditHistory> EditHistory { get; } = new List<TransactionEditHistory>();
        public TransactionType Type { get; set; }
        public bool IsDeleted { get; set; }
        public Transaction(int id, decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type) 
        {
            Id = id;
            Date = DateTime.Now;
            CategoryId = categoryId;
            AccountId = accountId;
            Amount = amount;
            CreatedByUserId = userId;
            Description = description;
            Type = type;
        }
        public void Update(decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            EditHistory.Add(new TransactionEditHistory(
                editorId,
                Amount, newAmount,
                CategoryId, newCategoryId,
                Description, newDescription
                ));
            Amount = newAmount;
            CategoryId = newCategoryId;
            Description = newDescription;
        }
    }
}
